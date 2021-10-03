using System;
using System.Collections.Generic;
using System.Text;

namespace DBR.Model
{
    /// <summary>
    /// Az épület osztály a Vendéglátóhely és a Játék szülő osztálya
    /// </summary>
    public class Building : Infrastructure
    {
        #region Fields

        /// <summary>
        /// Belépődíj, amit a vendégek fizetnek
        /// </summary>
        protected int _fee;

        /// <summary>
        /// Az épület fenntartásának díja amit a játékos fizet, meghatározott időközönként
        /// </summary>
        private int _maintenance;

        /// <summary>
        /// Az épület üzemeltetési díja, amit a játékos fizet használatonként
        /// </summary>
        protected int _upkeep;

        /// <summary>
        /// A hátralévő idő a következő fenntartási díj fizetéséig
        /// </summary>
        private int _timeForNextMaintenancePay;

        /// <summary>
        /// Az épület vendéglátói kapacítása
        /// </summary>
        protected int _capacity;

        /// <summary>
        /// Az épületet használó vendégek száma
        /// </summary>
        protected int _numberOfUsers;

        #endregion

        #region Properties

        /// <summary>
        /// Az épület belépődíjának lekérdezése, amit a vendégek fizetnek
        /// </summary>
        public int Fee { get { return _fee; } }

        /// <summary>
        /// Az épület fenntartási díjának lekérdezése, amit a játékos fizet meghatározott időnként
        /// </summary>
        public int Maintenance { get { return _maintenance; } }

        /// <summary>
        /// Az épület üzemeltetési díjának lekérdezése, amit a játékos fizet használatonként
        /// </summary>
        public int Upkeep { get { return _upkeep; } }

        /// <summary>
        /// Az épület vendéglátói kapacításának lekérdezése
        /// </summary>
        public int Capacity { get { return _capacity; } }

        /// <summary>
        /// Az épület hatása (hangulatra/jóllakottságra)
        /// </summary>
        public int Effect { get; set; }

        /// <summary>
        /// Sorbanállók lekérdezése
        /// </summary>
        public Queue<Visitor> WaitingVisitors { get; set; }

        public int NumberOfUseres { get { return _numberOfUsers; } set { _numberOfUsers = value; } }

        /// <summary>
        /// A hátralévő idő a következő fenntartási díj fizetéséig
        /// </summary>
        public int TimeForNextMaintenancePay { get { return _timeForNextMaintenancePay; } set { _timeForNextMaintenancePay = value; } }

        /// <summary>
        /// Működőképpeség: igaz, ha a játékos ki tudja fizetni az üzemeltetést
        /// </summary>
        public bool CanFunction { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Az épület fenntartási díjának kifizetése
        /// </summary>
        public event EventHandler<int> MaintenancePaying;

        /// <summary>
        /// Az épület üzemeltetési díjának fizetése
        /// </summary>
        public event EventHandler<int> UpkeepPaying;

        #endregion

        #region Constructor

        /// <summary>
        /// Az épület osztály konstruktora, inicializálja az adattagokat
        /// </summary>
        /// <param name="x">Az épület balfelső sarkának x koordinátája</param>
        /// <param name="y">Az épület balfelső sarkának y koordinátája</param>
        /// <param name="buildPrice">Az épület megépítésének ára</param>
        /// <param name="buildTime">Az épület megépítésének ideje másodpercben</param>
        /// <param name="length">Az épület hossza (az y tengelyen hány egységnyi)</param>
        /// <param name="width">Az épület szélesssége (az x tengelyen hány egységnyi)</param>
        /// <param name="fee">Az üzemeltetés díja amit a vendégek fizetnek</param>
        /// <param name="maintenance">Az épület fenntartásának díja amit a játékos fizet</param>
        /// <param name="capacity">Az épület vendéglátói kapacítása</param>
        /// <param name="name">Az épület neve</param>
        public Building(int x, int y, int buildPrice, int buildTime, int length, int width, int fee, int maintenance, int upkeep, int capacity, string name, int effect)
            : base(x, y, buildPrice, buildTime, length, width, name)
        {
            _fee = fee;
            _maintenance = maintenance;
            _upkeep = upkeep;
            _capacity = capacity;
            _numberOfUsers = 0;
            _timeForNextMaintenancePay = 30;

            WaitingVisitors = new Queue<Visitor>();
            CanFunction = false;
            Effect = effect;
        }

        #endregion

        #region Methods

        /// <summary>
        /// A maintenance díj kifizetése időközönként
        /// </summary>
        public void MaintenancePay()
        {
            if (--_timeForNextMaintenancePay == 0)
            {
                _timeForNextMaintenancePay = 30;
                MaintenancePaying?.Invoke(this, Maintenance);
            }
        }

        public void UpkeepPay()
        {
            UpkeepPaying?.Invoke(this, Upkeep);
        }

        public void PersonArrived(object sender, EventArgs e)
        {
            Visitor visitor = sender as Visitor;
            visitor.Arrived -= PersonArrived;

            if (visitor.Money > Fee)
            {
                visitor.State = Person.PersonCondition.Waiting;
                visitor.Start += GetOutOfRow;
                WaitingVisitors.Enqueue(visitor);
            }
        }

        public void GetOutOfRow(object sender, EventArgs e)
        {
            Visitor visitor = sender as Visitor;
            visitor.Start -= GetOutOfRow;

            Queue<Visitor> tmp = new Queue<Visitor>();
            while (WaitingVisitors.Peek() != visitor)
            {
                tmp.Enqueue(WaitingVisitors.Dequeue());
            }
            WaitingVisitors.Dequeue();
            while (WaitingVisitors.Count > 0)
            {
                tmp.Enqueue(WaitingVisitors.Dequeue());
            }
            WaitingVisitors = tmp;
        }

        #endregion
    }
}
