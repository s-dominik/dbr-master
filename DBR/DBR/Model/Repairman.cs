using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DBR.Model
{
    /// <summary>
    /// A karbantartó osztálya
    /// </summary>
    public class Repairman : Person
    {
        #region Fields

        /// <summary>
        /// A karbantartó dolgozik
        /// </summary>
        private bool _onDuty;

        #endregion

        #region Property

        /// <summary>
        /// A karbantartó dolgozásának lekérdezése
        /// </summary>
        public bool OnDuty { get { return _onDuty; } set { _onDuty = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// A karbantartó osztály konstruktora, inicializálja az adatagokat
        /// </summary>
        /// <param name="x">A karbantartó x koordinátája</param>
        /// <param name="y">A karbantartó y koordinátája</param>
        public Repairman(int x, int y, int maxid)
            : base(x, y, maxid)
        {
            _onDuty = false;
        }

        /// <summary>
        /// A karbantartó osztály copy konstruktora
        /// </summary>
        /// <param name="repairman">A karbantartó osztály egy esete</param>
        public Repairman(Repairman repairman)
            : this(repairman._posX, repairman._posY, repairman.SpriteID) { }

        #endregion

        #region Methods

        public void Repair()
        {
            // Pontosan hogyan?
        }

        #endregion
    }
}
