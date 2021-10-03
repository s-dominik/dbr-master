using System;
using System.Collections.Generic;
using System.Text;

namespace DBR.Persistance
{
    /// <summary>
    /// Aktuális játékállást tároló osztály
    /// mentéshez és betöltéshez.
    /// </summary>
    public class GameData
    {
        #region Properties

        #region Properties of the whole park
        // Properties from Model
        public int GameTime { get; private set; }

        public int Cash { get; set; }

        public bool Open { get; private set; }

        #endregion

        #region Properties of the infrastructures

        public int[,] Table { get; private set; }
 
        public int NumberOfInfrastructures { get; private set; }

        /// <summary>
        /// Soronként különböző infrastruktúra
        /// -1 az érték, ha nincs neki iylen adata
        /// típus: 1-Canteen; 2-Playground; 3-Plant; 4-Road
        /// Sorban az adatok: típus 0, posX 1, posY 2, buildprice 3, buildTime 4, length 5, width 6,
        ///                   state 7, construction 8, fee 9, maintenance 10, timeForNextMaintenance 11,
        ///                   capacity 12, minUtilization 13, rideDuration 14,
        ///                   rideTime 15, repairTime 16, chanceToBroke% 17,             
        ///                   currentRepairman 18, repairmanArrived 19, repairmanOnTheWay 20, 
        ///                   upkeep 21, canFunction 22, servinDuration 23, servingTime 24, effect 25
        ///                   numberOfUsers 26, users... 2-esével (canteen miatt), numberOfWaiting, waiting...
        /// </summary>
        public List<List<int>> InfrastructureProperties { get; private set; }

        /// <summary>
        /// Az infrastruktúrák nevei
        /// </summary>
        public List<string> InfrastructureNames { get; private set; }

        #endregion

        #region Properties of the people

        public int NumberOfVisitors { get; private set; }

        /// <summary>
        /// Soronként látogatók
        /// Egy sorban az adatok:
        ///     posX 0, posY 1, screenPosition.X 2, screenPosition.Y 3, state 4, money 5, mood 6,
        ///     satiety 7, patienceTime 8,  spriteID 9, roadCount*2 10, road ...
        /// </summary>
        public List<List<int>> VisitorProperties { get; private set; }

        public int NumberOfRepairmen { get; private set; }

        /// <summary>
        /// Soronként karbantartók
        /// Egy sorban az adatok:
        ///     posX 0, posY 1, screenPosition.X 2, screenPosition.Y 3, state 4, onduty 5,
        ///     spriteID 6, roadCount*2 7, road...
        /// </summary>
        public List<List<int>> RepairmanProperties { get; private set; }

        #endregion

        #region Properties from RoadGraph

        public int NodeCount { get; private set; }

        public int[] Points { get; private set; }

        public int[,] Neighbourhood { get; private set; }

        #endregion

        #endregion


        #region Constructor

        /// <summary>
        /// Játékállást tároló osztály konstruktora, inicializálja
        /// az összes property-t
        /// </summary>
        public GameData(int[,] table, int gameTime, int cash, bool open, int numberOfInfrastructures, List<List<int>> infrastructureProperties, List<string> infrastructureNames,
            int nodeCount, int[] points, int[,] neighbourhood, int numberOfVisitors, List<List<int>> visitorProperties, int numberOfRepairmen, List<List<int>> repairmanProperties)
        {
            Table = table;
            GameTime = gameTime;
            Cash = cash;
            Open = open;
            NumberOfInfrastructures = numberOfInfrastructures;
            InfrastructureProperties = infrastructureProperties;
            InfrastructureNames = infrastructureNames;
            NodeCount = nodeCount;
            Points = points;
            Neighbourhood = neighbourhood;
            NumberOfVisitors = numberOfVisitors;
            VisitorProperties = visitorProperties;
            NumberOfRepairmen = numberOfRepairmen;
            RepairmanProperties = repairmanProperties;
        }

        public GameData()
        {
            Table = new int[40, 60];
            for (int i = 0; i < Table.GetLength(0); i++)
            {
                for (int j = 0; j < Table.GetLength(1); j++)
                {
                    if (i == Table.GetLength(0) - 1 && (j == Table.GetLength(1) / 2 || j == Table.GetLength(1) / 2 + 1))
                        Table[i, j] = -3;
                    else if (i == 0 || i == Table.GetLength(0) - 1 || j == 0 || j == Table.GetLength(1) - 1)
                        // pálya széle
                        Table[i, j] = -2;
                    else
                        Table[i, j] = -1;
                }
            }

            int initRoadX = Table.GetLength(1) / 2;
            int initRoadY = Table.GetLength(0) - 2;
            Table[initRoadY, initRoadX] = 0;

            GameTime = 0;
            Cash = 10000;
            Open = false;
            NumberOfInfrastructures = 0;
            InfrastructureProperties = new List<List<int>>();
            InfrastructureNames = new List<string>();
            NodeCount = 1;
            Points = new int[2] { initRoadY, initRoadX };
            Neighbourhood = new int[0,0];
            NumberOfVisitors = 0;
            VisitorProperties = new List<List<int>>();
            NumberOfRepairmen = 0;
            RepairmanProperties = new List<List<int>>();
        }

        #endregion
    }
}
