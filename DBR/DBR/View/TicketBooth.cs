using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DBR
{
    /// <summary>
    /// Jegypéntár beállításait 
    /// megjelenítő párbeszédablak
    /// osztálya
    /// </summary>
    public partial class TicketBooth : Form
    {
        #region Properties

        /// <summary>
        /// Belépődíj
        /// </summary>
        public int EntranceFee { get; private set; }

        /// <summary>
        /// Karbantartó felvételének összege
        /// </summary>
        public int RepairmanFee { get; private set; }

        /// <summary>
        /// Javítás épületenkénti összege
        /// </summary>
        public int PerBuildingFee { get; private set; }

        /// <summary>
        /// Nyitva van-e a park
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// A játékos pénze
        /// </summary>
        public int PlayerCash { get; private set; }

        #endregion


        #region Events

        /// <summary>
        /// Park megnyitását jelző esemény
        /// </summary>
        public event EventHandler<int> ParkOpened;

        /// <summary>
        /// Karbantartó felvételét jelző esemény
        /// </summary>
        public event EventHandler RepairmanHired;

        #endregion


        #region Constructor

        /// <summary>
        /// Jegypénztár dialógus alapértelmezett konstruktora
        /// </summary>
        public TicketBooth() : this(0,0,0, false,0)
        {
            //InitializeComponent();
            //EntranceFee = 0;
            //RepairmanFee = 0;
            //PerBuildingFee = 0;

            //InitializeDisplays();
        }

        /// <summary>
        /// Jegypénztár dialógus paraméteres konstruktora
        /// </summary>
        /// <param name="entranceFee">Belépődíj</param>
        /// <param name="repairmanFee">Karbantartó felvételének díja</param>
        /// <param name="perBuildingFee">Karbantartás épületenkénti díja</param>
        /// <param name="isOpen">Park nyitva van-e</param>
        public TicketBooth(int entranceFee, int repairmanFee, int perBuildingFee, bool isOpen, int cash)
        {
            InitializeComponent();

            EntranceFee = entranceFee;
            RepairmanFee = repairmanFee;
            PerBuildingFee = perBuildingFee;
            PlayerCash = cash;

            btnOpenPark.Enabled = !isOpen;

            InitializeDisplays();
        }

        #endregion

        /// <summary>
        /// Karbantartó felvétele esemény kiváltása gombnyomásra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHireRepairman_Click(object sender, EventArgs e)
        {
            if (PlayerCash > RepairmanFee)
            {
                RepairmanHired?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Not enough money!", "Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Kiírandó értékek inicializálása
        /// </summary>
        private void InitializeDisplays()
        {
            numEntranceFee.Value = EntranceFee;
            lblRepairmanFee.Text = RepairmanFee.ToString();
            lblPerBuildingFee.Text = $"{PerBuildingFee}/épület";
        }

        /// <summary>
        /// Ablak bezárásakor a belépődíj frissítése
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TicketBooth_FormClosing(object sender, FormClosingEventArgs e)
        {
            EntranceFee = Convert.ToInt32(numEntranceFee.Value);
        }

        /// <summary>
        /// Park megnyitásának eseménye
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenPark_Click(object sender, EventArgs e)
        {
            ParkOpened?.Invoke(this, (int)numEntranceFee.Value);
            IsOpen = true;
            btnOpenPark.Enabled = false;
        }
    }
}
