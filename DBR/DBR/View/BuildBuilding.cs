using System;
using System.Drawing;
using System.Windows.Forms;
using DBR.Model;

namespace DBR
{
    public partial class BuildBuilding : Form
    {

        #region Properties

        /// <summary>
        /// Épület ára
        /// </summary>
        public int Price { get; private set; }

        /// <summary>
        /// Épület típusa
        /// </summary>
        public int Type { get; private set; }

        /// <summary>
        /// Épület kapacitása
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Épület minimum kapacitása
        /// </summary>
        public int MinCapacity { get; private set; }

        /// <summary>
        /// Épület üzemeltetési díja
        /// </summary>
        public int Fee { get; private set; }

        /// <summary>
        /// Épület hossza
        /// </summary>
        public int BuildingLength { get; private set; }

        /// <summary>
        /// Épület szélessége
        /// </summary>
        public int BuildingWidth { get; private set; }

        #endregion

        #region Constructor

        public BuildBuilding(Point mousePos, int type, int price, int length, int width, int capacity)
        {
            InitializeComponent();

            Type = type;
            Price = price;
            BuildingLength = length;
            BuildingWidth = width;
            Capacity = capacity;
            MinCapacity = 1;
            Fee = 0;
            labelCapacity.Text = Capacity.ToString();
            labelPrice.Text = Price.ToString();
            labelArea.Text = length + "x" + width;
            if(type == 3 || type == 4)
            {
                labelOnlyCapacity.Visible = false;
                labelCapacity.Visible = false;
                labelTicketPrice.Visible = false;
                trackBarPrice.Visible = false;
                labelTicketPriceValue.Visible = false;
                labelMinPeople.Visible = false;
                trackBarPeople.Visible = false;
                labelMinPeopleValue.Visible = false;
            }
            if(type == 1)
            {
                labelTicketPrice.Visible = false;
                trackBarPrice.Visible = false;
                labelTicketPriceValue.Visible = false;
                labelMinPeople.Visible = false;
                trackBarPeople.Visible = false;
                labelMinPeopleValue.Visible = false;
            }
        }

        #endregion

        #region Event handlers

        private void buttonBuild_Click(object sender, EventArgs e)
        {
            Fee = Convert.ToInt32(labelTicketPriceValue.Text);
            MinCapacity = Convert.ToInt32(labelMinPeopleValue.Text);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void trackBarPrice_Scroll(object sender, EventArgs e)
        {
            labelTicketPriceValue.Text = trackBarPrice.Value.ToString();
        }

        private void trackBarPeople_Scroll(object sender, EventArgs e)
        {
            labelMinPeopleValue.Text = trackBarPeople.Value.ToString();
            trackBarPeople.Maximum = Capacity;
        }

        #endregion

    }
}
