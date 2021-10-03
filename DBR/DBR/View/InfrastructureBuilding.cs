using DBR.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DBR.View
{
    public partial class InfrastructureBuilding : Form
    {
        #region Fields

        /// <summary>
        /// Infrastruktúrák
        /// </summary>
        public List<BuildableStruct> _infrastructures;

        #endregion

        #region Properties

        /// <summary>
        /// Infrastruktúra ára
        /// </summary>
        public int Price { get; private set; }

        /// <summary>
        /// Infrastruktúra típusa
        /// </summary>
        public int Type { get; private set; }

        /// <summary>
        /// Infrastruktúra Neve
        /// </summary>
        public string BuildingName { get; private set; }

        /// <summary>
        /// Infrastruktúra kapacitása
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Infrastruktúra minimum kapacitása
        /// </summary>
        public int MinCapacity { get; private set; }

        /// <summary>
        /// Infrastruktúra üzemeltetési díja
        /// </summary>
        public int Fee { get; private set; }

        /// <summary>
        /// Infrastruktúra rendszeres fenntartási költsége
        /// </summary>
        public int Maintenance { get; set; }

        /// <summary>
        /// Infrastruktúra alkalmankénti üzemeltetési költsége
        /// </summary>
        public int Upkeep { get; set; }

        /// <summary>
        /// Infrastruktúra hossza
        /// </summary>
        public int BuildingLength { get; private set; }

        /// <summary>
        /// Infrastruktúra szélessége
        /// </summary>
        public int BuildingWidth { get; private set; }

        /// <summary>
        /// Infrastruktúra megépülése
        /// </summary>
        public bool BuildingIsOk { get; private set; }

        /// <summary>
        /// Infrastruktúra építési ideje
        /// </summary>
        public int BuildTime { get; set; }

        /// <summary>
        /// Infrastruktúra menetideje
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Infrastruktúra hatása
        /// </summary>
        public int Effect { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Épületek dialógus konstruktora
        /// </summary>
        /// <param name="mousePos">Az ablak megjelenítésének helye</param>
        public InfrastructureBuilding()
        {
            InitializeComponent();

            BuildingIsOk = false;
        }

        #endregion

        private void InfrastructureBuilding_Load(object sender, EventArgs e)
        {
            Buildable _buildable = new Buildable();
            _infrastructures = _buildable.GetInfrastructures;

            dataGridView1.DataSource = _infrastructures;
            dataGridView1.Columns["Types"].Visible = false;
            dataGridView1.Columns["Capacity"].Visible = false;
            dataGridView1.Columns["Effect"].Visible = false;
            dataGridView1.Columns["Maintenance"].Visible = false;
            dataGridView1.Columns["Upkeep"].Visible = false;
            dataGridView1.Columns["Duration"].Visible = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var selectedInfrastructure = dataGridView1.SelectedRows[0].DataBoundItem as BuildableStruct;

                Type = selectedInfrastructure.Types;
                BuildingName = selectedInfrastructure.Name;
                Price = selectedInfrastructure.Price;
                BuildingLength = selectedInfrastructure.Length;
                BuildingWidth = selectedInfrastructure.Width;
                Capacity = selectedInfrastructure.Capacity;
                BuildTime = selectedInfrastructure.BuildTime;
                Duration = selectedInfrastructure.Duration;
                Effect = selectedInfrastructure.Effect;
                Maintenance = selectedInfrastructure.Maintenance;
                Upkeep = selectedInfrastructure.Upkeep;
                BuildBuilding b = new BuildBuilding(Location, Type, Price, BuildingLength, BuildingWidth, Capacity);
                if (b.ShowDialog() == DialogResult.OK)
                {
                    Fee = b.Fee;
                    MinCapacity = b.MinCapacity;
                    BuildingIsOk = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some error occured: " + ex.Message + " - " + ex.Source);
            }
            this.Close();
        }
    }
}
