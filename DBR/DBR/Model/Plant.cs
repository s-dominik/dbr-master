using System;
using System.Collections.Generic;
using System.Text;

namespace DBR.Model
{
    /// <summary>
    /// A növény osztálya
    /// </summary>
    public class Plant : Infrastructure
    {
        #region Constructor

        /// <summary>
        /// Az növény osztály konstruktora, inicializálja az adattagokat
        /// </summary>
        /// <param name="x">Az növény balfelső sarkának x koordinátája</param>
        /// <param name="y">Az növény balfelső sarkának y koordinátája</param>
        /// <param name="buildPrice">Az növény megépítésének ára</param>
        /// <param name="buildTime">Az növény megépítésének ideje másodpercben</param>
        /// <param name="length">Az növény hossza (az y tengelyen hány egységnyi)</param>
        /// <param name="width">Az növény szélesssége (az x tengelyen hány egységnyi)</param>
        public Plant(int x, int y, int buildPrice, int buildTime, int length, int width, string name)
            : base(x, y, buildPrice, buildTime, length, width, name) { }

        /// <summary>
        /// A növény osztály copy konstruktora
        /// </summary>
        /// <param name="plant">A növény osztály egy esete</param>
        public Plant(Plant plant)
            : this(plant._posX, plant._posY, plant.BuildPrice, plant.BuildTime, plant.Length, plant.Width, plant.Name) { }
        #endregion
    }
}
