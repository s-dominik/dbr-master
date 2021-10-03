using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DBR.Model
{
    public class BuildableStruct
    {
        /// <summary>
        /// Az infrastruktúra típusa
        /// </summary>
        public int Types { get; set; }

        /// <summary>
        /// Az infrastruktúra neve
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Az infrastruktúra hossza
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Az infrastruktúra szélessége
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Az infrastruktúra ára
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Az infrastruktúra rendszeres fenntartási költsége
        /// </summary>
        public int Maintenance { get; set; }

        /// <summary>
        /// Az infrastruktúra alkalmankénti üzemeltetési költsége
        /// </summary>
        public int Upkeep { get; set; }

        /// <summary>
        /// Az infrastruktúra kapacitása
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Az infrastruktúra használatának időtartama
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Az infrastruktúra megépítésének ideje
        /// </summary>
        public int BuildTime { get; set; }

        /// <summary>
        /// Az infrastruktúra hatása
        /// </summary>
        public int Effect { get; set; }
    }
}
