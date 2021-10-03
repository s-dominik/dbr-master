using System;
using System.Drawing;

namespace DBR.Model
{
    /// <summary>
    /// Az infrastruktúra osztály az út, növény és az épület osztályok szülő osztálya
    /// </summary>
    public class Infrastructure
    {
        #region Enums

        /// <summary>
        /// Az infrastruktúra állapota
        /// </summary>
        public enum BuildingCondition
        {
            UnderConstruction = 0, //Építés alatt
            Functioning = 1, //Üzemel
            Waiting = 2, //Várakozik
            Broken = 3 //Elromlott
        }

        #endregion

        #region Fields

        /// <summary>
        /// Az infrastruktúra balfelső sarkának x koordinátája
        /// </summary>
        protected int _posX;

        /// <summary>
        /// Az infrastruktúra balfelső sarkának y koordinátája 
        /// </summary>
        protected int _posY;

        /// <summary>
        /// Az infrastruktúra megépítésének ára
        /// </summary>
        private int _buildPrice;

        /// <summary>
        /// Az infrastruktúra megépítésének ideje másodpercben
        /// </summary>
        protected int _buildTime;

        /// <summary>
        /// Az infrastruktúra hossza (az y tengelyen hány egységnyi)
        /// </summary>
        private int _length;

        /// <summary>
        /// Az infrastruktúra szélesssége (az x tengelyen hány egységnyi)
        /// </summary>
        private int _width;

        /// <summary>
        /// Az infrastruktúra állapota
        /// </summary>
        protected BuildingCondition _state;

        /// <summary>
        /// Az infrastruktúra üzembehelyezéséig hátralévő idő másodpercben
        /// </summary>
        private int _construction;

        /// <summary>
        /// Az infrastruktúra neve
        /// </summary>
        private string _name;

        #endregion

        #region Properties

        /// <summary>
        /// [ELAVULT]
        /// Az infrastruktúra pozíciójának lekérdezése
        /// </summary>
        public int[] Position { get { return new int[2]{_posX,_posY }; } }

        /// <summary>
        /// Az infrastruktúra pozíciójának és méretének lekérdezése
        /// </summary>
        public Rectangle Transform { get; private set; }

        /// <summary>
        /// Az építés árának lekérdezése
        /// </summary>
        public int BuildPrice { get { return _buildPrice; } }

        /// <summary>
        /// Az építés idejének lekérdezése
        /// </summary>
        public int BuildTime { get { return _buildTime; } }

        /// <summary>
        /// Az infrastruktúra hosszának lekérdezése
        /// </summary>
        public int Length { get { return _length; } }

        /// <summary>
        /// Az infrastruktúra szélességének lekérdezése
        /// </summary>
        public int Width { get { return _width; } }

        /// <summary>
        /// Az infrastruktúra állapotának lekérdezése
        /// </summary>
        public BuildingCondition State { get { return _state; } set { _state = value; } }

        /// <summary>
        /// Az infrastruktúra építéséből hátralévő idő
        /// </summary>
        public int ConstructionRemainingTime { get { return _construction; } set { _construction = value; } }

        /// <summary>
        /// Az infrastruktúra nevének lekérdezése
        /// </summary>
        public string Name { get { return _name; } }

        public bool Reachable { get; set; }

        #endregion

        #region Events

        public event EventHandler<Rectangle> ConstructionComplete;

        #endregion

        #region Constructors

        /// <summary>
        /// Az infrastruktúra osztály konstruktora, inicializálja az adatagokat
        /// </summary>
        /// <param name="x">Az infrastruktúra balfelső sarkának x koordinátája</param>
        /// <param name="y">Az infrastruktúra balfelső sarkának y koordinátája</param>
        /// <param name="buildPrice">Az infrastruktúra megépítésének ára</param>
        /// <param name="buildTime">Az infrastruktúra megépítésének ideje másodpercben</param>
        /// <param name="length">Az infrastruktúra hossza (az y tengelyen hány egységnyi)</param>
        /// <param name="width">Az infrastruktúra szélesssége (az x tengelyen hány egységnyi)</param>
        public Infrastructure(int x, int y, int buildPrice, int buildTime, int length, int width, string name)
        {
            _name = name;
            Transform = new Rectangle(x, y, width, length);
            _posX = x;
            _posY = y;
            _buildPrice = buildPrice;
            _buildTime = buildTime;
            _length = length;
            _width = width;
            _state = BuildingCondition.UnderConstruction;
            _construction = _buildTime;
            Reachable = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Az épület megépülése, ha a visszaszámláló eléri a 0-át üzembe kerül
        /// </summary>
        public void Construct()
        {
            if(--_construction == 0)
            {
                _state = BuildingCondition.Waiting;
                ConstructionComplete?.Invoke(this, new Rectangle(Transform.X, Transform.Y + Transform.Height - 1, Transform.Width, 1));
            }
        }

        #endregion
    }
}