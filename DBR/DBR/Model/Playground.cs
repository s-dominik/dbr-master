using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DBR.Model
{
    /// <summary>
    /// A vidámparki játékok osztálya
    /// </summary>
    public class Playground : Building
    {
        #region Fields

        /// <summary>
        /// A játék minimum kihasználtsága
        /// </summary>
        private int _minUtilization;

        /// <summary>
        /// A játék üzemelésének ideje
        /// </summary>
        private int _rideDuration;

        /// <summary>
        /// Az aktuális hátralévő idő a menetből
        /// </summary>
        private int _rideTime;

        /// <summary>
        /// Az aktuális hátralévő idő a javításból;
        /// </summary>
        private int _repairTime;

        /// <summary>
        /// A játék elromlásának esélye
        /// </summary>
        private double _chanceToBroke;

        /// <summary>
        /// A játékhoz megérkezett a karbantartó
        /// </summary>
        private bool _repairmanArrived;

        /// <summary>
        /// A játékhoz elinduló karbantartó
        /// </summary>
        private bool _repairmanOnTheWay;

        /// <summary>
        /// A jelenlegi karbantartó indexe
        /// </summary>
        private int _currentRepairman;

        #endregion

        #region Properties

        /// <summary>
        /// A játék minimumkihasználtságának lekérdezése
        /// </summary>
        public int MinUtilization { get { return _minUtilization; } }

        /// <summary>
        /// A játék üzemelési idejének lekérdezése
        /// </summary>
        public int RideDuration { get { return _rideDuration; } }

        /// <summary>
        /// Az aktuális hárlaévő idő a menetből
        /// </summary>
        public int RideTime { get { return _rideTime; } set { _rideTime = value; } }

        public int RepairTime { get { return _repairTime; } set { _repairTime = value; } }

        public int ChanceToBroke { get { return (int)(_chanceToBroke * 100); } set { _chanceToBroke = (double)value / 100; } }

        public int CurrentRepairman { get { return _currentRepairman; } set { _currentRepairman = value; } }

        public bool RepairmanArrivedProp { get { return _repairmanArrived; } set { _repairmanArrived = value; } }

        public bool RepairmanOnTheWayProp { get { return _repairmanOnTheWay; } set { _repairmanOnTheWay = value; } }

        public Visitor[] UsingVisitors { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Ajáték javításraszorulását jelző esemény
        /// </summary>
        public event EventHandler<int[]> RepairingNeeded;

        /// <summary>
        /// Ajáték javításraszorulását jelző esemény
        /// </summary>
        public event EventHandler<int> RepairingEnded;

        public event EventHandler<int> RideStarted;

        #endregion

        #region Constructors

        /// <summary>
        /// A játék osztály konstruktora, inicializálja az adattagokat
        /// </summary>
        /// <param name="x">Az játék balfelső sarkának x koordinátája</param>
        /// <param name="y">Az játék balfelső sarkának y koordinátája</param>
        /// <param name="buildPrice">Az játék megépítésének ára</param>
        /// <param name="buildTime">Az játék megépítésének ideje másodpercben</param>
        /// <param name="length">Az játék hossza (az y tengelyen hány egységnyi)</param>
        /// <param name="width">Az játék szélesssége (az x tengelyen hány egységnyi)</param>
        /// <param name="fee">Az üzemeltetés díja amit a vendégek fizetnek</param>
        /// <param name="maintenance">Az játék fenntartásának díja amit a játékos fizet</param>
        /// <param name="capacity">Az játék vendéglátói kapacítása</param>
        /// <param name="minUtilization">A játék minimum kihasználtsága</param>
        /// <param name="rideDuration">A játék üzemelésének ideje</param>
        /// <param name="name">Az játék neve</param>
        public Playground(int x, int y, int buildPrice, int buildTime, int length, int width, int fee, int maintenance, int upkeep, int capacity, int minUtilization, int rideDuration, string name, int effect)
            : base(x, y, buildPrice, buildTime, length, width, fee, maintenance, upkeep, capacity, name, effect)
        {
            _minUtilization = minUtilization;
            _rideDuration = rideDuration;
            _chanceToBroke = 0.02;
            _repairmanArrived = false;
            _repairmanOnTheWay = false;
            _currentRepairman = -1;

            UsingVisitors = new Visitor[capacity];
            _rideTime = _rideDuration;
        }

        /// <summary>
        /// A játék osztály copy konstruktora
        /// </summary>
        /// <param name="playground">A játék egy esete</param>
        public Playground(Playground playground)
            : this(playground._posX, playground._posY, playground.BuildPrice, playground.BuildTime, playground.Length, playground.Width, playground.Fee, playground.Maintenance, playground.Upkeep, playground._capacity, playground.MinUtilization, playground.RideDuration, playground.Name, playground.Effect) { }

        #endregion

        #region Methods

        /// <summary>
        /// A model időkezelőjében meghívandó függvény
        /// </summary>
        public void TimeAdvanced()
        {
            switch (_state)
            {
                case BuildingCondition.Waiting:
                    StartGame();
                    break;
                case BuildingCondition.Functioning:
                    foreach (Visitor visitor in UsingVisitors)
                    {
                        visitor?.Playing(Effect);
                    }

                    if (--_rideTime == 0)
                    {
                        foreach (Visitor visitor in UsingVisitors)
                        {
                            if (visitor != null)
                            {
                                visitor.State = Person.PersonCondition.Ply;
                                visitor.Road = null;
                            }
                        }
                        NumberOfUseres = 0;

                        if (IsBreaking())
                        {
                            _state = BuildingCondition.Broken;
                            _repairTime = _buildTime;
                            //Karbantartó hívása
                            RepairingNeeded?.Invoke(this, new int[2] { Position[0], Position[1] + Length - 1 });
                        }
                        else
                        {
                            CanFunction = false;
                            _state = BuildingCondition.Waiting;
                            _rideTime = _rideDuration;
                            
                            StartGame();
                        }
                    }
                    break;
                case BuildingCondition.Broken:
                    if (!_repairmanOnTheWay) RepairingNeeded?.Invoke(this, new int[2] { Position[0], Position[1] + Length - 1 });
                    if (_repairmanArrived)
                    {
                        if (--_repairTime == 0)
                        {
                            _state = BuildingCondition.Waiting;
                            _repairmanArrived = false;
                            _repairmanOnTheWay = false;
                            RepairingEnded?.Invoke(this, _currentRepairman);
                            StartGame();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// A játék elindítása, ha várakoznk a minimumkihasználtságnál többen
        /// </summary>
        private void StartGame()
        {
            if (WaitingVisitors.Count >= _minUtilization)
            {
                UpkeepPay();

                if (CanFunction)
                {
                    int visitorNumberOnRide = Math.Min(WaitingVisitors.Count, _capacity);
                    UsingVisitors = new Visitor[visitorNumberOnRide];
                    NumberOfUseres = visitorNumberOnRide;

                    for (int i = 0; i < visitorNumberOnRide; i++)
                    {
                        Visitor firstInQueue = WaitingVisitors.Dequeue();

                        firstInQueue.State = Person.PersonCondition.Use;
                        firstInQueue.Start -= GetOutOfRow;
                        firstInQueue.PayFee(_fee);
                        UsingVisitors[i] = firstInQueue;
                    }

                    _rideTime = _rideDuration;
                    _state = BuildingCondition.Functioning;
                    RideStarted?.Invoke(this, _fee);
                }
            }
        }

        /// <summary>
        /// Ajáték elromlik-e egy menet után
        /// </summary>
        /// <returns></returns>
        private bool IsBreaking()
        {
            double x = new Random().NextDouble();
            if (x < _chanceToBroke)
            {
                _chanceToBroke = 0.02;
                return true;
            }
            else
            {
                _chanceToBroke += 0.06;
                return false;
            }
        }

        /// <summary>
        /// Karbantartó megérkezése
        /// </summary>
        public void RepairmanArrived(object sender, EventArgs e)
        {
            _repairmanArrived = true;

        }

        /// <summary>
        /// Karbantartó úton van
        /// </summary>
        public void RepairmanOnTheWay(object sender, EventArgs e)
        {
            _repairmanOnTheWay = true;
        }

        /// <summary>
        /// A jelenlegi karbantartó indexének lekérdezése
        /// </summary>
        public void RepairmanIndex(int index)
        {
            _currentRepairman = index;
        }
        #endregion
    }
}
