using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DBR.Model
{
    public class Buildable
    {
        #region Fields

        /// <summary>
        /// Építhető infrastruktúrák
        /// </summary>
        public List<BuildableStruct> _infrastructures;

        #endregion

        #region Property

        /// <summary>
        /// Az építhető infrastruktúrák lekérdezése
        /// </summary>
        public List<BuildableStruct> GetInfrastructures { get { return _infrastructures; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Az buildable osztály konstruktora, inicializálja az adatagokat
        /// </summary>
        public Buildable()
        {
            _infrastructures = new List<BuildableStruct>
            {
                new BuildableStruct()
                {
                    Types = 1,
                    Name = "Canteen",
                    Length = 3,
                    Width = 4,
                    Price = 7000,
                    Maintenance = 20,
                    Upkeep = 30,
                    Capacity = 50,
                    Duration = 10,
                    BuildTime = 6,
                    Effect = 3
                },

                new BuildableStruct()
                {
                    Types = 1,
                    Name = "Pub",
                    Length = 5,
                    Width = 4,
                    Price = 10000,
                    Maintenance = 40,
                    Upkeep = 30,
                    Capacity = 20,
                    Duration = 8,
                    BuildTime = 5,
                    Effect = 2
                },

                new BuildableStruct()
                {
                    Types = 2,
                    Name = "Rollercoaster",
                    Length = 8,
                    Width = 5,
                    Price = 12000,
                    Maintenance = 50,
                    Upkeep = 40,
                    Capacity = 18,
                    Duration = 20,
                    BuildTime = 10,
                    Effect = 5
                },

                new BuildableStruct()
                {
                    Types = 2,
                    Name = "Carousel",
                    Length = 5,
                    Width = 5,
                    Price = 9000,
                    Maintenance = 20,
                    Upkeep = 10,
                    Capacity = 10,
                    Duration = 15,
                    BuildTime = 8,
                    Effect = 2
                },

                new BuildableStruct()
                {
                    Types = 2,
                    Name = "Ferris Wheel",
                    Length = 7,
                    Width = 8,
                    Price = 11000,
                    Maintenance = 50,
                    Upkeep = 30,
                    Capacity = 20,
                    Duration = 22,
                    BuildTime = 12,
                    Effect = 5
                },

                new BuildableStruct()
                {
                    Types = 3,
                    Name = "Tree",
                    Length = 1,
                    Width = 1,
                    Price = 500,
                    Capacity = 0,
                    BuildTime = 6
                },

                new BuildableStruct()
                {
                    Types = 3,
                    Name = "Bush",
                    Length = 1,
                    Width = 1,
                    Price = 300,
                    Capacity = 0,
                    BuildTime = 3
                },

                new BuildableStruct()
                {
                    Types = 3,
                    Name = "Grass",
                    Length = 1,
                    Width = 1,
                    Price = 100,
                    Capacity = 0,
                    BuildTime = 1
                },

                new BuildableStruct()
                {
                    Types = 4,
                    Name = "Road",
                    Length = 1,
                    Width = 1,
                    Price = 1000,
                    Capacity = 0,
                    BuildTime = 4
                }
            };
        }

        #endregion

    }
}
