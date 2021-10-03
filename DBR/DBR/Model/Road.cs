using System;
using System.Collections.Generic;
using System.Text;

namespace DBR.Model
{
    /// <summary>
    /// Az út osztálya
    /// </summary>
    public class Road : Infrastructure
    {
        #region Constructors

        /// <summary>
        /// Az út osztály konstruktora, inicializálja az adattagokat
        /// </summary>
        /// <param name="x">Az út balfelső sarkának x koordinátája</param>
        /// <param name="y">Az út balfelső sarkának y koordinátája</param>
        /// <param name="buildPrice">Az út megépítésének ára</param>
        /// <param name="buildTime">Az út megépítésének ideje másodpercben</param>
        /// <param name="length">Az út hossza (az y tengelyen hány egységnyi)</param>
        /// <param name="width">Az út szélesssége (az x tengelyen hány egységnyi)</param>
        public Road(int x, int y, int buildPrice, int buildTime, int length, int width, string name)
            : base(x, y, buildPrice, buildTime, length, width, name) { }

        public Road(Road road)
            : this(road._posX, road._posY, road.BuildPrice, road.BuildTime, road.Length, road.Width, road.Name) { }

        #endregion
    }
}
