using DBR.Persistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace DBR.Model
{
    /// <summary>
    /// A játék logikáját végző osztály
    /// </summary>
    public class GameModel
    {
        #region Fields

        /// <summary>
        /// A játéktáblán lévő épületeket eltároló adattag. Ha üres a mező, akkor -1 az értéke, különben az _infrastructures adott indexe. A tábla bal felső sarka a 0,0 indexű elem
        /// 
        /// </summary>
        private int[,] _table;

        /// <summary>
        /// A játékból eltelt idő másodpercben 
        /// </summary>
        private int _gameTime;

        /// <summary>
        /// A játékos pénze
        /// </summary>
        private int _cash;

        /// <summary>
        /// A karbantartó ára
        /// </summary>
        private int _priceOfRepairman;

        /// <summary>
        /// A javítás ára
        /// </summary>
        private int _priceOfRepairing;

        /// <summary>
        /// A vidámpark nyitvatartására szolgáó adattag
        /// </summary>
        private bool _open;

        /// <summary>
        /// Az úthálózat eltárolását szolgáló adattag
        /// </summary>
        private RoadGraph _roadGraph;

        /// <summary>
        /// A játék mentését és betöltését szolgáló adattag
        /// </summary>
        private IDataAccess _dataaccess;

        /// <summary>
        /// A vendégek eltárolását szolgáló adattag
        /// </summary>
        private List<Visitor> _visitors;

        /// <summary>
        /// A karbantartók eltárolását szolgáló adattag
        /// </summary>
        private List<Repairman> _repairmen;

        /// <summary>
        /// Az épületek, utak és növények eltárolását szolgáló lista
        /// </summary>
        private List<Infrastructure> _infrastructures;

        /// <summary>
        /// A vendéglátóhelyek eltárolására szolgáló lista
        /// </summary>
        private List<int> _canteens;

        /// <summary>
        /// A játékok eltárolására szolgáló lista
        /// </summary>
        private List<int> _playgrounds;

        /// <summary>
        /// Az utszakaszok eltárolására szolgáló lista
        /// </summary>
        private List<int> _roads;

        #endregion

        #region Properties

        /// <summary>
        /// Az idő lekérdezése
        /// </summary>
        public int Time { get { return _gameTime; } }

        /// <summary>
        /// A viámpark nyitvatartásának lekérdezése
        /// </summary>
        public bool isOpened { get { return _open; } }

        /// <summary>
        /// A játékos pénzének lekérdezése
        /// </summary>
        public int Cash { get { return _cash; } }

        /// <summary>
        /// A karbantartó árának lekérdezése
        /// </summary>
        public int PriceOfRepairman { get { return _priceOfRepairman; } }

        /// <summary>
        /// Javítás árának lekérdezése
        /// </summary>
        public int PriceOfRepairing { get { return _priceOfRepairing; } }

        /// <summary>
        /// A tábla lekérdezése
        /// </summary>
        public int[,] Table { get { return _table; } }

        /// <summary>
        /// A tábla szélességének lekérdezése
        /// </summary>
        public int TableSizeX { get { return _table.GetLength(1); } }

        /// <summary>
        /// A tábla magasságának lekérdezése
        /// </summary>
        public int TableSizeY { get { return _table.GetLength(0); } }

        /// <summary>
        /// Épületek listájának lekérdezése
        /// </summary>
        public List<Infrastructure> BuildingList { get { return _infrastructures; } }

        /// <summary>
        /// Belépődíj
        /// </summary>
        public int EntranceFee { get; set; }

        /// <summary>
        /// A A vendégek és karbantartók lekérdezése
        /// </summary>
        public List<Repairman> Repairmen { get { return _repairmen; } }

        /// <summary>
        /// A vendégek lekérdezése
        /// </summary>
        public List<Visitor> Visitors { get { return _visitors; } }

        /// <summary>
        /// A vendéglátóhelyek számának lekérdezése
        /// </summary>
        public List<int> Canteens { get { return _canteens; } }

        /// <summary>
        /// A játékok számának lekérdezése
        /// </summary>
        public List<int> Playgrounds { get { return _playgrounds; } }

        /// <summary>
        /// Az útszakaszok számának lekérdezése
        /// </summary>
        public List<int> Roads { get { return _roads; } }

        /// <summary>
        /// Gráf csomópontjainak száma
        /// </summary>
        public int GraphNodeCount { get { return _roadGraph.NodeCount; } }

        public long GraphPendingWorkItemCount { get { return _roadGraph.PendingWorkItemCount; } }

        public long GraphCompletedWorkItemCount { get { return _roadGraph.CompletedWorkItemCount; } }

        public long GraphThreadCount { get { return _roadGraph.ThreadCount; } }

        public List<Point> GraphNodePoints { get { return _roadGraph.FindAllPoints(_ => true); } }

        public List<Tuple<Point, Point>> GraphLines
        {
            get
            {
                List<Tuple<Point, Point>> lines = new List<Tuple<Point, Point>>();
                List<Node> nodes = _roadGraph.FindAllNodes(_ => true);
                foreach (Node node in nodes)
                {
                    foreach (Node neigh in node.Neighbours)
                    {
                        lines.Add(new Tuple<Point, Point>(node.Position, neigh.Position));
                    }
                }

                return lines;
            }
        }

        public float[,] GraphDistancesMatrix { get { return _roadGraph.DistancesMatrix; } }

        public int[,] GraphPredecessorsMatrix { get { return _roadGraph.PredecessorsMatrix; } }

        public RoadGraph Graph { get { return _roadGraph; } }

        #endregion

        #region Events

        /// <summary>
        /// A játéktábla megváltoztatást jelző esemény. 
        /// </summary>
        public event EventHandler<int[,]> TableChanged;

        /// <summary>
        /// A pénzösszeg megváltozását jelző esemény
        /// </summary>
        public event EventHandler<int> CashChanged;

        /// <summary>
        /// A látogatók számának megváltozását jelző esemény. 
        /// </summary>
        public event EventHandler<int> VisitorNumberChanged;

        /// <summary>
        /// A karbantartók számának megváltozását jelző esemény.
        /// </summary>
        public event EventHandler<int> RepairmanNumberChanged;

        /// <summary>
        /// A játékidő megváltozását jelző esemény
        /// </summary>
        public event EventHandler<int> GameTimeChanged;

        /// <summary>
        /// Az építés sikertelenségét jelző esemény
        /// </summary>
        public event EventHandler<string> BuildingUnsuccessful;

        #endregion

        #region Constructor

        /// <summary>
        /// A GameModel osztály üres konstruktora
        /// </summary>
        public GameModel() { }

        /// <summary>
        /// A GameModel osztály konstruktora. Inicializálja az adattagokat.
        /// </summary>
        /// <param name="dataAccess">A játékállás mentését/betöltését végző osztály egy példánya</param>
        public GameModel(IDataAccess dataAccess)
        {
            _dataaccess = dataAccess;
            NewGame();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Látogatók léptetése
        /// </summary>
        /// <param name="tileSize"></param>
        public void StepPeople(int tileSize, int speedMultiplier)
        {
            foreach (Visitor visitor in Visitors)
            {
                if (visitor.State == Person.PersonCondition.Ply)
                    visitor.Step(tileSize, speedMultiplier);
            }

            Visitors.RemoveAll(v => v.WentHome);

            foreach (Repairman repairman in Repairmen)
            {
                if (repairman.State == Person.PersonCondition.Ply || (repairman.State == Person.PersonCondition.Waiting && !repairman.OnDuty))
                    repairman.Step(tileSize, speedMultiplier);
            }
        }

        /// <summary>
        /// Új játék indítása, paraméterek alaphelyzetbe állítása
        /// </summary>
        public void NewGame()
        {
            _roadGraph = new RoadGraph();

            _infrastructures = new List<Infrastructure>();
            _visitors = new List<Visitor>();
            _repairmen = new List<Repairman>();
            _gameTime = 0;
            _priceOfRepairman = 3000;
            _priceOfRepairing = 1000;
            _cash = 100000;
            EntranceFee = 0;
            _open = false;

            _canteens = new List<int>();
            _playgrounds = new List<int>();
            _roads = new List<int>();

            GenerateTable(30, 40);

            OnTableChanged();
            OnGameTimeChanged();
        }

        /// <summary>
        /// Tábla létrehozása és feltöltése alapértelmezett értékekkel
        /// </summary>
        /// <param name="rows">oszlopok száma</param>
        /// <param name="cols">sorok száma</param>
        private void GenerateTable(int rows, int cols)
        {
            _table = new int[rows, cols];
            const int empty = -1;
            const int fence_h = -2;
            const int fence_v = -3;
            const int fence_lt = -4;
            const int fence_rt = -5;
            const int fence_lb = -6;
            const int fence_rb = -7;
            const int gate = -8;
            for (int i = 0; i < TableSizeY; i++)
            {
                for (int j = 0; j < TableSizeX; j++)
                {                    
                    int field;
                    if(i == 0 || i == TableSizeY - 1)
                    {
                        field = fence_h;
                    }
                    else if(j == 0 || j == TableSizeX - 1)
                    {
                        field = fence_v;
                    }
                    else
                    {
                        field = empty;
                    }

                    _table[i, j] = field;
                }
            }

            _table[0, 0] = fence_lt;
            _table[0, TableSizeX - 1] = fence_rt;
            _table[TableSizeY - 1, 0] = fence_lb;
            _table[TableSizeY - 1, TableSizeX - 1] = fence_rb;

            int k = TableSizeY - 1;
            int l = TableSizeX / 2;
            _table[k, l] = _table[k, l + 1] = gate;

            int initRoadX = TableSizeX / 2;
            int initRoadY = TableSizeY - 2;
            BuildingInProgress(4, initRoadX, initRoadY, 0, 0, 0, 0, 1, 1, 0, 0, 0, "Road", 0, 0);
            _infrastructures[0].State = Infrastructure.BuildingCondition.Waiting;
            _roadGraph.AddNode(new Point(initRoadX, initRoadY));
            _roads.Add(0);
        }

        /// <summary>
        /// A játékidő múlását követő metódus.
        /// </summary>
        public void TimeAdvanced()
        {
            _gameTime++;
            OnGameTimeChanged();
            OnCashChanged();

            foreach (Infrastructure inf in _infrastructures)
            {
                if (inf.State == Infrastructure.BuildingCondition.UnderConstruction)
                {
                    inf.Construct();
                }

                if (inf is Building)
                {
                    Building building = inf as Building;
                    building.MaintenancePay();
                    if (building is Playground)
                    {
                        (building as Playground).TimeAdvanced();
                    }
                    else if (building is Canteen)
                    {
                        (building as Canteen).TimeAdvanced();
                    }
                }
            }

            foreach (Repairman repairman in _repairmen)
            {
                if (!repairman.OnDuty && (repairman.Road is null || repairman.Road.Count == 0))
                {
                    NewDestinationForRepairman(repairman);
                }
            }

            if (isOpened)
            {
                if (new Random().Next(2) == 1)
                {
                    int money = new Random().Next(50, 200);

                    int x = TableSizeX / 2;
                    int y = TableSizeY - 2;
                    Visitor newVisitor = new Visitor(x, y, money, new Random().Next(50) + 50, new Random().Next(50) + 50, 14);
                    newVisitor.PaidFee += new EventHandler<int>(CollectFee);
                    newVisitor.HasBadMood += OnNewDestination;
                    newVisitor.IsHungry += OnNewDestination;
                    if (money * 0.8f > EntranceFee && newVisitor.PayFee(EntranceFee))
                    {
                        _visitors.Add(newVisitor);
                        NewDestinationForVisitor(newVisitor);
                        OnVisitorNumberChanged();
                    }
                }

                foreach (Visitor visitor in _visitors)
                {
                    switch (visitor.State)
                    {
                        case Person.PersonCondition.Ply:
                            if (visitor.Road is null || visitor.Road.Count == 0)
                            {
                                NewDestinationForVisitor(visitor);
                            }
                            visitor.MoodChanges();
                            break;
                        case Person.PersonCondition.Waiting:
                            visitor.MoodChanges();
                            break;
                        default:
                            break;
                    }

                    PlantEffectOnVisitors(visitor);
                }
            }
        }

        /// <summary>
        /// Adott mező részleteinek kigyűjtése
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<string> GetDetails(int x, int y)
        {
            List<string> details = new List<string>();

            Infrastructure inf = _infrastructures[_table[y, x]];

            details.Add("--- Buildings ---");
            details.Add(inf.Name);
            details.Add($"State of building: {inf.State}");

            details.Add("--- Visitors ---");
            List<Visitor> selectedVisitors = _visitors.FindAll(v => v.Position.X == x && v.Position.Y == y);
            foreach (var visitor in selectedVisitors)
            {
                details.Add($"Position: {visitor.Position.X}-{visitor.Position.Y}");
                details.Add($"Money: {visitor.Money}");
                details.Add($"Mood: {visitor.Mood}");
                details.Add($"Satiety: {visitor.Satiety}");
                details.Add("-");
            }

            details.Add("--- Repairmen ---");
            List<Repairman> selectedRepairmen = _repairmen.FindAll(r => r.Position.X == x && r.Position.Y == y);
            foreach (var repairman in selectedRepairmen)
            {
                details.Add($"Position: {repairman.Position.X}-{repairman.Position.Y}");
                details.Add($"Is on duty: {repairman.OnDuty}");
                details.Add("-");
            }

            return details;
        }

        /// <summary>
        /// Uticél keresése emberekhez
        /// </summary>
        /// <param name="person">Az ember akinek uticélt adunk</param>
        private void NewDestinationForVisitor(Visitor visitor)
        {
            //TODO: Mindenképp elinduljon valamerre
            Random rnd = new Random();
            if (visitor.Money < 1 || visitor.Mood < 1)
            {
                VisitorGoesHome(visitor, EventArgs.Empty);
                return;
            }

            if (visitor.Satiety < 20 && _canteens.Count > 0)
            {
                Infrastructure inf = _infrastructures[_canteens[rnd.Next(_canteens.Count)]];
                if ((inf as Canteen).Fee < 0.8f * visitor.Money)
                {
                    visitor.Road = _roadGraph.GetShortestPath(visitor.Position, new Point(inf.Position[0], inf.Position[1] + inf.Length - 1));
                    visitor.State = Person.PersonCondition.Ply;
                    visitor.Arrived += (inf as Canteen).PersonArrived;
                }
                return;
            }

            if (_playgrounds.Count > 0)
            {
                Infrastructure inf = _infrastructures[_playgrounds[rnd.Next(_playgrounds.Count)]];
                if ((inf as Playground).Fee < 0.8f * visitor.Money)
                {
                    visitor.Road = _roadGraph.GetShortestPath(visitor.Position, new Point(inf.Position[0], inf.Position[1] + inf.Length - 1));
                    visitor.State = Person.PersonCondition.Ply;
                    visitor.Arrived += (inf as Playground).PersonArrived;
                }
                return;
            }

            if (visitor.Satiety < 100 && _canteens.Count > 0)
            {
                Infrastructure inf = _infrastructures[_canteens[rnd.Next(_canteens.Count)]];
                if ((inf as Canteen).Fee < 0.8f * visitor.Money)
                {
                    visitor.Road = _roadGraph.GetShortestPath(visitor.Position, new Point(inf.Position[0], inf.Position[1] + inf.Length - 1));
                    visitor.State = Person.PersonCondition.Ply;
                    visitor.Arrived += (inf as Canteen).PersonArrived;
                }
                return;
            }
            if (_roads.Count > 0)
            {
                Infrastructure inf = _infrastructures[_roads[rnd.Next(_roads.Count)]];
                visitor.Road = _roadGraph.GetShortestPath(visitor.Position, new Point(inf.Position[0], inf.Position[1]));
                visitor.State = Person.PersonCondition.Ply;
                return;
            }
            else
            {
                VisitorGoesHome(visitor, EventArgs.Empty);
                return;
            }
        }

        /// <summary>
        /// Belépők árának begyűjtése
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CollectFee(object sender, int e)
        {
            _cash += e;

            OnCashChanged();
        }

        /// <summary>
        /// Uticél keresés karbantartókhoz
        /// </summary>
        /// <param name="repairman"></param>
        private void NewDestinationForRepairman(Repairman repairman)
        {
            if (_roads.Count > 0)
            {
                int roadIndex = new Random().Next(_roads.Count);
                repairman.Road = _roadGraph.GetShortestPath(repairman.Position, new Point(_infrastructures[_roads[roadIndex]].Position[0], _infrastructures[_roads[roadIndex]].Position[1]));
            }
        }

        /// <summary>
        /// Látogató hazamenését kezelő függvény
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewDestination(object sender, EventArgs e)
        {
            NewDestinationForVisitor(sender as Visitor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisitorGoesHome(object sender, EventArgs e)
        {
            int x = TableSizeX / 2;
            int y = TableSizeY - 2;
            Visitor visitor = sender as Visitor;
            visitor.Road = _roadGraph.GetShortestPath(visitor.Position, new Point(x, y));
            visitor.State = Person.PersonCondition.Ply;
            visitor.Arrived += RemoveVisitor;
        }

        /// <summary>
        /// Látogató eltávolítása a látogatók gyűjteményéből
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveVisitor(object sender, EventArgs e)
        {
            (sender as Visitor).WentHome = true;
        }

        /// <summary>
        /// Adott infrastructure létrehozása
        /// </summary>
        /// <param name="type">Az építendő infrastruktúra típusa</param>
        /// <param name="x">Az infrastruktúra bal felső sarkának x koordinátája</param>
        /// <param name="y">Az infrastruktúra bal felső sarkának y koordinátája</param>
        /// <param name="price">Az infrastruktúra építései ára</param>
        /// <param name="length">Az infrastruktúra hossza (y)</param>
        /// <param name="width">Az infrastruktúra szélessége (x)</param>
        /// <param name="fee">Az üzemeltetés díja amit a vendégek fizetnek</param>
        /// <param name="capacity">Az infrastruktúra kapcitása</param>
        /// <param name="minCapacity">Az infrastrktúra minimumkihasználtsága</param>
        /// <param name="name">Az infrastruktúra neve</param>
        public void BuildingInProgress(int type, int x, int y, int price, int maintenance, int upkeep, int buildTime, int length, int width, int fee, int capacity, int minCapacity, string name, int duration, int effect)
        {
            //Pénz, táblaszéle ellenőrzése
            if (_cash < price)
            {
                OnBuildingUnsuccesful("Not enough money");
                return;
            }
            if (x + width >= TableSizeX || y + length >= TableSizeY)
            {
                OnBuildingUnsuccesful("The infrastructure is out of border");
                return;
            }

            for (int j = x; j < x + width; j++)
                for (int i = y; i < y + length; i++)
                {
                    if (_table[i, j] != -1)
                    {
                        OnBuildingUnsuccesful("Cannot build on another infrastructure");
                        return;
                    }
                }


            Infrastructure inf = null;

            switch (type)
            {
                case 1:
                    inf = new Canteen(x, y, price, buildTime, length, width, fee, maintenance, upkeep, capacity, duration, name, effect);
                    (inf as Canteen).MaintenancePaying += OnMaintenancePaying;
                    (inf as Canteen).UpkeepPaying += OnUpkeepPaying;
                    inf.ConstructionComplete += AddNodesToGraph;
                    break;
                case 2:
                    inf = new Playground(x, y, price, buildTime, length, width, fee, maintenance, upkeep, capacity, minCapacity, duration, name, effect);
                    (inf as Playground).MaintenancePaying += OnMaintenancePaying;
                    (inf as Playground).UpkeepPaying += OnUpkeepPaying;
                    (inf as Playground).RideStarted += OnRideStarted;
                    inf.ConstructionComplete += AddNodesToGraph;
                    (inf as Playground).RepairingNeeded += CallRepairman;
                    (inf as Playground).RepairingEnded += SendRepairman;
                    break;
                case 3:
                    inf = new Plant(x, y, price, buildTime, length, width, name);
                    break;
                case 4:
                    inf = new Road(x, y, price, buildTime, length, width, name);
                    inf.ConstructionComplete += AddNodesToGraph;
                    break;
                default:
                    throw new ArgumentException("Unknown type", "type");
            }

            _infrastructures.Add(inf);
            _cash -= inf.BuildPrice;
            OnCashChanged();

            BuildingPlace(inf);
        }



        /// <summary>
        /// Infrastructura helyének létrehozása a táblán
        /// </summary>
        /// <param name="inf">A megépítendő infrastruktúra</param>
        /// <param name="type">Az infrastruktúra típusa</param>
        public void BuildingPlace(Infrastructure inf)
        {
            int ind = _infrastructures.IndexOf(inf);

            for (int i = inf.Position[0]; i < inf.Position[0] + inf.Width; i++)
                for (int j = inf.Position[1]; j < inf.Position[1] + inf.Length; j++)
                {
                    _table[j, i] = ind;
                }
        }

        /// <summary>
        /// A park megnyitása
        /// </summary>
        public void Opening(object sender, int e)
        {
            if (!isOpened)
            {
                _open = true;
                EntranceFee = e;
            }
        }

        /// <summary>
        /// Karbantartó felvétele
        /// </summary>
        public void HireRepairman(object sender, EventArgs e)
        {
            int x = TableSizeX / 2;
            int y = TableSizeY - 2;
            Repairman r = new Repairman(x, y, 4);
            _repairmen.Add(r);
            OnRepairmanNumberChanged();
            NewDestinationForRepairman(_repairmen[_repairmen.Count - 1]);
            r.PersonState(0);
            r.Arrived += Repairing;
            _cash -= _priceOfRepairman;
        }



        /// <summary>
        /// A látogatók hangulatváltozásai növény közelében
        /// </summary>
        public void PlantEffectOnVisitors(Visitor visitor)
        {
            int x = visitor.Position.Y;
            int y = visitor.Position.X;

            if (x > 0 && y > 0 && PlantCheck(x - 1, y - 1)) visitor.PlantEffect(PlantEffectByType((Plant)BuildingList[Table[x - 1, y - 1]]));
            if (x > 0 && PlantCheck(x - 1, y)) visitor.PlantEffect(PlantEffectByType((Plant)BuildingList[Table[x - 1, y]]));
            if (x > 0 && y < TableSizeY - 1 && PlantCheck(x - 1, y + 1)) visitor.PlantEffect(PlantEffectByType((Plant)BuildingList[Table[x - 1, y + 1]]));
            if (y < TableSizeY - 1 && PlantCheck(x, y + 1)) visitor.PlantEffect(PlantEffectByType((Plant)BuildingList[Table[x, y + 1]]));
            if (x < TableSizeX - 1 && y < TableSizeY - 1 && PlantCheck(x + 1, y + 1)) visitor.PlantEffect(PlantEffectByType((Plant)BuildingList[Table[x + 1, y + 1]]));
            if (x < TableSizeX - 1 && PlantCheck(x + 1, y)) visitor.PlantEffect(PlantEffectByType((Plant)BuildingList[Table[x + 1, y]]));
            if (x < TableSizeX - 1 && y > 0 && PlantCheck(x + 1, y - 1)) visitor.PlantEffect(PlantEffectByType((Plant)BuildingList[Table[x + 1, y - 1]]));
            if (y > 0 && PlantCheck(x, y - 1)) visitor.PlantEffect(PlantEffectByType((Plant)BuildingList[Table[x, y - 1]]));
        }

        /// <summary>
        /// Növény mező ellenőrzések
        /// </summary>
        public bool PlantCheck(int x, int y)
        {
            if (Table[x, y] >= 0 && BuildingList[Table[x, y]].GetType().Equals(typeof(Plant))) return true;
            return false;
        }

        /// <summary>
        /// A növények hatása a látogatókra típusuk szerint
        /// </summary>
        public int PlantEffectByType(Plant plant)
        {
            if (plant.Name == "Tree") return 5;
            if (plant.Name == "Bush") return 3;
            if (plant.Name == "Grass") return 1;
            return 0;
        }

        /// <summary>
        /// Játék betöltésére szolgáló függvény
        /// </summary>
        /// <param name="path">A betöltendő fájl elérési útvonala</param>
        /// <returns></returns>
        public async Task LoadGameAsync(String path)
        {
            if (_dataaccess == null)
                throw new InvalidOperationException("No data access provided.");

            GameData gameData = await _dataaccess.LoadAsync(path);

            NewGame();

            //GameModel adatok
            _gameTime = gameData.GameTime;
            _cash = gameData.Cash;
            _open = gameData.Open;

            // Emberek adatok
            for (int i = 0; i < gameData.NumberOfVisitors; i++)
            {
                Visitor visitor = new Visitor(gameData.VisitorProperties[i][0], gameData.VisitorProperties[i][1], gameData.VisitorProperties[i][5],
                                              gameData.VisitorProperties[i][6], gameData.VisitorProperties[i][7], gameData.VisitorProperties[i][9]);
                visitor.State = (Person.PersonCondition)gameData.VisitorProperties[i][4];
                visitor.PatienceTime = gameData.VisitorProperties[i][8];
                visitor.ScreenPosition = new Point(gameData.VisitorProperties[i][2], gameData.VisitorProperties[i][3]);
                visitor.SpriteID = gameData.VisitorProperties[i][9];
                Queue<Point> road = new Queue<Point>();
                for (int j = 11; j < 11 + gameData.VisitorProperties[i][10]; j += 2)
                {
                    road.Enqueue(new Point(gameData.VisitorProperties[i][j], gameData.VisitorProperties[i][j + 1]));
                }
                visitor.RoadForpersistence = road;
                _visitors.Add(visitor);
                //Látogatok eseményei
                visitor.PaidFee += new EventHandler<int>(CollectFee);
                visitor.HasBadMood += OnNewDestination;
                visitor.IsHungry += OnNewDestination;
            }



            for (int i = 0; i < gameData.NumberOfRepairmen; i++)
            {
                Repairman repairman = new Repairman(gameData.RepairmanProperties[i][0], gameData.RepairmanProperties[i][1], gameData.RepairmanProperties[i][6]);
                repairman.State = (Person.PersonCondition)gameData.RepairmanProperties[i][4];
                repairman.OnDuty = gameData.RepairmanProperties[i][5] == 1 ? true : false;
                repairman.ScreenPosition = new Point(gameData.RepairmanProperties[i][2], gameData.RepairmanProperties[i][3]);
                repairman.SpriteID = gameData.RepairmanProperties[i][6];
                List<Point> road = new List<Point>();
                for (int j = 8; j < 8 + gameData.RepairmanProperties[i][7]; j += 2)
                {
                    road.Add(new Point(gameData.RepairmanProperties[i][j], gameData.RepairmanProperties[i][j + 1]));
                }
                _repairmen.Add(repairman);
                repairman.Arrived += Repairing;
            }

            //Infrastruktúra adatok
            for (int i = 0; i < gameData.NumberOfInfrastructures; i++)
            {
                Infrastructure inf;
                int type = gameData.InfrastructureProperties[i][0];
                switch (type)
                {
                    case 1: //Vendéglátóhely betöltése
                        inf = new Canteen(gameData.InfrastructureProperties[i][1], gameData.InfrastructureProperties[i][2], gameData.InfrastructureProperties[i][3],
                                          gameData.InfrastructureProperties[i][4], gameData.InfrastructureProperties[i][5], gameData.InfrastructureProperties[i][6],
                                          gameData.InfrastructureProperties[i][9], gameData.InfrastructureProperties[i][10], gameData.InfrastructureProperties[i][21],
                                          gameData.InfrastructureProperties[i][12], gameData.InfrastructureProperties[i][23], gameData.InfrastructureNames[i], gameData.InfrastructureProperties[i][25]);
                        inf.State = (Infrastructure.BuildingCondition)gameData.InfrastructureProperties[i][7];
                        inf.ConstructionRemainingTime = gameData.InfrastructureProperties[i][8];
                        Canteen canteen = inf as Canteen;
                        canteen.TimeForNextMaintenancePay = gameData.InfrastructureProperties[i][11];

                        Dictionary<Visitor, int> canteenusers = new Dictionary<Visitor, int>(canteen.Capacity);

                        canteen.NumberOfUseres = gameData.InfrastructureProperties[i][26];
                        for (int j = 27; j < gameData.InfrastructureProperties[i][26]*2 + 27; j+=2)
                        {
                            canteenusers.Add(_visitors[gameData.InfrastructureProperties[i][j]], gameData.InfrastructureProperties[i][j+1]);
                        }
                        canteen.ClientsServed = canteenusers;

                        Queue<Visitor> canteeenwaiting = new Queue<Visitor>();
                        for (int j = 27 + gameData.InfrastructureProperties[i][26] * 2 + 1; j < gameData.InfrastructureProperties[i][26]*2 + 28 + gameData.InfrastructureProperties[i][27 + gameData.InfrastructureProperties[i][26]*2]; j++)
                        {
                            canteeenwaiting.Enqueue(_visitors[gameData.InfrastructureProperties[i][j]]);
                            _visitors[gameData.InfrastructureProperties[i][j]].Start += canteen.GetOutOfRow;
                        }
                        canteen.WaitingVisitors = canteeenwaiting;

                        _infrastructures.Add(inf);
                        _canteens.Add(_infrastructures.Count - 1);

                        canteen.MaintenancePaying += OnMaintenancePaying;
                        canteen.UpkeepPaying += OnUpkeepPaying;
                        break;

                    case 2: //Vidámparki játék betöltése
                        string infName;
                        if (gameData.InfrastructureNames[i] == "Ferris_Wheel")
                            infName = "Ferris Wheel";
                        else
                            infName = gameData.InfrastructureNames[i];
                        inf = new Playground(gameData.InfrastructureProperties[i][1], gameData.InfrastructureProperties[i][2], gameData.InfrastructureProperties[i][3],
                                                 gameData.InfrastructureProperties[i][4], gameData.InfrastructureProperties[i][5], gameData.InfrastructureProperties[i][6],
                                                 gameData.InfrastructureProperties[i][9], gameData.InfrastructureProperties[i][10], gameData.InfrastructureProperties[i][21],
                                                 gameData.InfrastructureProperties[i][12], gameData.InfrastructureProperties[i][13], gameData.InfrastructureProperties[i][14], infName, gameData.InfrastructureProperties[i][25]);
                        inf.State = (Infrastructure.BuildingCondition)gameData.InfrastructureProperties[i][7];
                        inf.ConstructionRemainingTime = gameData.InfrastructureProperties[i][8];
                        Playground playground = inf as Playground;
                        playground.TimeForNextMaintenancePay = gameData.InfrastructureProperties[i][11];
                        playground.RideTime = gameData.InfrastructureProperties[i][15];
                        playground.RepairTime = gameData.InfrastructureProperties[i][16];
                        playground.ChanceToBroke = gameData.InfrastructureProperties[i][17];
                        playground.CurrentRepairman = gameData.InfrastructureProperties[i][18];
                        playground.RepairmanArrivedProp = gameData.InfrastructureProperties[i][19] == 1 ? true : false;
                        playground.RepairmanOnTheWayProp = gameData.InfrastructureProperties[i][20] == 1 ? true : false;
                        Visitor[] playgroundusers = new Visitor[playground.Capacity];
                        int playgrounddb = 0;

                        playground.NumberOfUseres = gameData.InfrastructureProperties[i][26];
                        for (int j = 27; j < gameData.InfrastructureProperties[i][26]*2 + 27; j+=2)
                        {
                            playgroundusers[playgrounddb] = _visitors[gameData.InfrastructureProperties[i][j]];
                            playgrounddb++;
                        }
                        playground.UsingVisitors = playgroundusers;

                        Queue<Visitor> playgroundwaiting = new Queue<Visitor>();
                        for (int j = 27 + gameData.InfrastructureProperties[i][26] * 2 + 1; j < gameData.InfrastructureProperties[i][26]*2 + 28 + gameData.InfrastructureProperties[i][27 + gameData.InfrastructureProperties[i][26]*2]; j++)
                        {
                            playgroundwaiting.Enqueue(_visitors[gameData.InfrastructureProperties[i][j]]);
                            _visitors[gameData.InfrastructureProperties[i][j]].Start += playground.GetOutOfRow;
                        }
                        playground.WaitingVisitors = playgroundwaiting;

                        _infrastructures.Add(inf);
                        _playgrounds.Add(_infrastructures.Count - 1);

                        if (playground.State == Infrastructure.BuildingCondition.Broken)
                        {
                            if (playground.RepairmanOnTheWayProp)
                            {
                                _repairmen[playground.CurrentRepairman].Start += playground.RepairmanOnTheWay;
                                _repairmen[playground.CurrentRepairman].Arrived += playground.RepairmanArrived;
                            }
                        }

                        playground.MaintenancePaying += OnMaintenancePaying;
                        playground.UpkeepPaying += OnUpkeepPaying;
                        playground.RideStarted += OnRideStarted;
                        playground.RepairingNeeded += CallRepairman;
                        playground.RepairingEnded += SendRepairman;
                        break;

                    case 3: //Növény betöltése
                        inf = new Plant(gameData.InfrastructureProperties[i][1], gameData.InfrastructureProperties[i][2], gameData.InfrastructureProperties[i][3],
                                        gameData.InfrastructureProperties[i][4], gameData.InfrastructureProperties[i][5], gameData.InfrastructureProperties[i][6], gameData.InfrastructureNames[i]);
                        inf.State = (Infrastructure.BuildingCondition)gameData.InfrastructureProperties[i][7];
                        inf.ConstructionRemainingTime = gameData.InfrastructureProperties[i][8];

                        _infrastructures.Add(inf);
                        break;

                    case 4: //Út betöltése
                        inf = new Road(gameData.InfrastructureProperties[i][1], gameData.InfrastructureProperties[i][2], gameData.InfrastructureProperties[i][3],
                                        gameData.InfrastructureProperties[i][4], gameData.InfrastructureProperties[i][5], gameData.InfrastructureProperties[i][6], gameData.InfrastructureNames[i]);
                        inf.State = (Infrastructure.BuildingCondition)gameData.InfrastructureProperties[i][7];
                        inf.ConstructionRemainingTime = gameData.InfrastructureProperties[i][8];

                        _infrastructures.Add(inf);
                        _roads.Add(_infrastructures.Count - 1);
                        break;
                }
            }

            _table = gameData.Table;
            List<Point> points = new List<Point>();
            for (int i = 0; i < gameData.NodeCount * 2; i += 2)
            {
                points.Add(new Point(gameData.Points[i], gameData.Points[i + 1]));
            }
            _roadGraph.MakeGraph(points, gameData.Neighbourhood);

            foreach (Visitor visitor in _visitors)
            {
                if (visitor.State == Person.PersonCondition.Ply && visitor.Road.Count != 0)
                {
                    Infrastructure infrastructure = _infrastructures[_table[visitor.Road.ToArray()[^1].Y, visitor.Road.ToArray()[^1].X]];
                    if (infrastructure is Building)
                    {
                        visitor.Arrived += (infrastructure as Building).PersonArrived;
                    }
                }
            }

            OnCashChanged();
            OnGameTimeChanged();
            OnTableChanged();
        }

        /// <summary>
        /// Játék mentésére szolgáló függvény
        /// </summary>
        /// <param name="path">A mentési fájl elérési útvonala</param>
        /// <returns></returns>
        public async Task SaveGameAsync(String path)
        {
            if (_dataaccess == null)
                throw new InvalidOperationException("No data access provided.");

            int nodeCount;
            List<Point> points;
            int[,] neighbourhood;
            _roadGraph.MakeSerializable(out nodeCount, out points, out neighbourhood);
            int[] pointsArray = new int[nodeCount * 2];
            int arrayCounter = 0;
            int listCounter = 0;
            foreach (Point point in points)
            {
                pointsArray[arrayCounter] = points[listCounter].X;
                pointsArray[arrayCounter + 1] = points[listCounter].Y;
                arrayCounter += 2;
                listCounter++;
            }

            List<List<int>> infrastructures = new List<List<int>>();
            List<string> infrastructureNames = new List<string>();

            foreach (Infrastructure infrastructure in _infrastructures.GetRange(1, _infrastructures.Count - 1))
            {
                List<int> listItem = new List<int>();
                if (infrastructure is Canteen)
                    listItem.Add(1);
                else if (infrastructure is Playground)
                    listItem.Add(2);
                else if (infrastructure is Plant)
                    listItem.Add(3);
                else if (infrastructure is Road)
                    listItem.Add(4);
                listItem.Add(infrastructure.Position[0]);
                listItem.Add(infrastructure.Position[1]);
                listItem.Add(infrastructure.BuildPrice);
                listItem.Add(infrastructure.BuildTime);
                listItem.Add(infrastructure.Length);
                listItem.Add(infrastructure.Width);
                listItem.Add((int)infrastructure.State);
                listItem.Add(infrastructure.ConstructionRemainingTime);

                var building = infrastructure as Building;
                var playground = infrastructure as Playground;
                var canteen = infrastructure as Canteen;

                if (building != null)
                {
                    listItem.Add(building.Fee);
                    listItem.Add(building.Maintenance);
                    listItem.Add(building.TimeForNextMaintenancePay);
                    listItem.Add(building.Capacity);
                }
                else
                {
                    listItem.Add(-1);
                    listItem.Add(-1);
                    listItem.Add(-1);
                    listItem.Add(-1);
                }

                if (playground != null)
                {
                    listItem.Add(playground.MinUtilization);
                    listItem.Add(playground.RideDuration);
                    listItem.Add(playground.RideTime);
                    listItem.Add(playground.RepairTime);
                    listItem.Add(playground.ChanceToBroke);
                    listItem.Add(playground.CurrentRepairman);
                    if (playground.RepairmanArrivedProp)
                        listItem.Add(1);
                    else listItem.Add(0);
                    if (playground.RepairmanOnTheWayProp)
                        listItem.Add(1);
                    else listItem.Add(0);
                }
                else
                {
                    listItem.Add(-1);
                    listItem.Add(-1);
                    listItem.Add(-1);
                    listItem.Add(-1);
                    listItem.Add(-1);
                    listItem.Add(-1);
                    listItem.Add(-1);
                    listItem.Add(-1);
                }

                if (building != null)
                {
                    listItem.Add(building.Upkeep);
                    listItem.Add(building.CanFunction ? 1 : 0);
                }
                else
                {
                    listItem.Add(-1);
                    listItem.Add(-1);
                }

                if (canteen != null)
                {
                    listItem.Add(canteen.ServingDuration);
                    listItem.Add(canteen.ServingTime);
                }
                else
                {
                    listItem.Add(-1);
                    listItem.Add(-1);
                }

                if(building != null)
                {
                    listItem.Add(building.Effect);
                }
                else
                {
                    listItem.Add(-1);
                }

                // Használók és várakozók mentése
                if (canteen != null)
                {
                    listItem.Add(canteen.ClientsServed.Count);

                    foreach(var user in canteen.ClientsServed)
                    {
                        listItem.Add(_visitors.IndexOf(user.Key));
                        listItem.Add(user.Value);
                    }

                    listItem.Add(canteen.WaitingVisitors.Count);
                    Queue<Visitor> tmp = new Queue<Visitor>();
                    while (canteen.WaitingVisitors.Count > 0)
                    {
                        int index = _visitors.IndexOf(canteen.WaitingVisitors.Dequeue());
                        listItem.Add(index);
                        tmp.Enqueue(_visitors[index]);
                    }
                    canteen.WaitingVisitors = tmp;
                }
                else if (playground != null)
                {
                    listItem.Add(playground.NumberOfUseres);
                    for (int i = 0; i < playground.NumberOfUseres; i++)
                    {
                        listItem.Add(_visitors.IndexOf(playground.UsingVisitors[i]));
                        listItem.Add(-1);
                    }

                    listItem.Add(playground.WaitingVisitors.Count);
                    Queue<Visitor> tmp = new Queue<Visitor>();
                    while (playground.WaitingVisitors.Count > 0)
                    {
                        int index = _visitors.IndexOf(playground.WaitingVisitors.Dequeue());
                        listItem.Add(index);
                        tmp.Enqueue(_visitors[index]);
                    }
                    playground.WaitingVisitors = tmp;
                }
                else
                {
                    listItem.Add(-1);
                    listItem.Add(-1);
                }

                if (infrastructure.Name == "Ferris Wheel")
                    infrastructureNames.Add("Ferris_Wheel");
                else
                    infrastructureNames.Add(infrastructure.Name);

                infrastructures.Add(listItem);
            }

            // Emberek kigyűjtése
            List<List<int>> visitorProperties = new List<List<int>>();
            foreach (Visitor visitor in _visitors)
            {
                List<int> listitem = new List<int>();
                listitem.Add(visitor.Position.X);
                listitem.Add(visitor.Position.Y);
                listitem.Add(visitor.ScreenPosition.X);
                listitem.Add(visitor.ScreenPosition.Y);
                listitem.Add((int)visitor.State);
                listitem.Add(visitor.Money);
                listitem.Add(visitor.Mood);
                listitem.Add(visitor.Satiety);
                listitem.Add(visitor.PatienceTime);
                listitem.Add(visitor.SpriteID);
                listitem.Add(visitor.RoadForpersistence.Count * 2);
                foreach (Point step in visitor.RoadForpersistence)
                {
                    listitem.Add(step.X);
                    listitem.Add(step.Y);
                }

                visitorProperties.Add(listitem);
            }

            List<List<int>> repairmanProperties = new List<List<int>>();
            foreach (Repairman repairman in _repairmen)
            {
                List<int> listitem = new List<int>();
                listitem.Add(repairman.Position.X);
                listitem.Add(repairman.Position.Y);
                listitem.Add(repairman.ScreenPosition.X);
                listitem.Add(repairman.ScreenPosition.Y);
                listitem.Add((int)repairman.State);
                listitem.Add(repairman.OnDuty ? 1 : 0);
                listitem.Add(repairman.SpriteID);
                listitem.Add(repairman.RoadForpersistence.Count * 2);
                foreach (Point step in repairman.RoadForpersistence)
                {
                    listitem.Add(step.X);
                    listitem.Add(step.Y);
                }

                repairmanProperties.Add(listitem);
            }


            await _dataaccess.SaveAsync(path, new GameData(_table, _gameTime, _cash, _open, _infrastructures.Count - 1, infrastructures, infrastructureNames, nodeCount, pointsArray, neighbourhood, _visitors.Count, visitorProperties, _repairmen.Count, repairmanProperties));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Karbantartó elküldése javítás után
        /// </summary>
        public void SendRepairman(object sender, int e)
        {
            Repairman chosenRepairman = _repairmen[e];
            chosenRepairman.Start -= (sender as Playground).RepairmanOnTheWay;
            chosenRepairman.Arrived -= (sender as Playground).RepairmanArrived;
            chosenRepairman.OnDuty = false;
            chosenRepairman.PersonState(0);
            NewDestinationForRepairman(chosenRepairman);
        }

        /// <summary>
        /// Legközelebbi karbantartó hívása
        /// </summary>
        public void CallRepairman(object sender, int[] e)
        {
            float minim = float.PositiveInfinity;
            Point p1 = new Point(e[0], e[1]);
            int index = -1;

            for (int i = 0; i < _repairmen.Count; i++)
            {
                if (!_repairmen[i].OnDuty && _cash > _priceOfRepairing)
                {
                    Point p2 = _repairmen[i].Position;
                    if (_roadGraph.GetDistanceOfPoints(p1, p2) < minim)
                    {
                        minim = _roadGraph.GetDistanceOfPoints(p1, p2);
                        index = i;
                    }
                }
            }
            if (index > -1)
            {
                Repairman chosenRepairman = _repairmen[index];
                chosenRepairman.OnDuty = true;
                chosenRepairman.Start += ((Playground)sender).RepairmanOnTheWay;
                chosenRepairman.Arrived += ((Playground)sender).RepairmanArrived;
                ((Playground)sender).RepairmanIndex(index);
                chosenRepairman.Road = _roadGraph.GetShortestPath(_repairmen[index].Position, p1);
                chosenRepairman.State = Person.PersonCondition.Ply;
            }
        }

        /// <summary>
        /// A játékok javítására szolgáló függvény
        /// </summary>
        public void Repairing(object sender, EventArgs e)
        {
            if (((Repairman)sender).OnDuty)
            {
                _cash -= _priceOfRepairing;
                ((Repairman)sender).PersonState(2);
            }
        }

        /// <summary>
        /// Játékidő változás eseményének kiváltása.
        /// </summary>
        private void OnGameTimeChanged()
        {
            GameTimeChanged?.Invoke(this, _gameTime);
        }

        /// <summary>
        /// Játékos pénzösszegének változását kiváltó esemény.
        /// </summary>
        private void OnCashChanged()
        {
            CashChanged?.Invoke(this, _cash);
        }

        /// <summary>
        /// Látogatók számának változását kiváltó esemény.
        /// </summary>
        private void OnVisitorNumberChanged()
        {
            VisitorNumberChanged?.Invoke(this, _visitors.Count);
        }

        /// <summary>
        /// Karbantartók számának változását kiváltó esemény.
        /// </summary>
        private void OnRepairmanNumberChanged()
        {
            RepairmanNumberChanged?.Invoke(this, _repairmen.Count);
        }

        /// <summary>
        /// Játéktábla változás eseményének kiváltása
        /// </summary>
        private void OnTableChanged()
        {
            TableChanged?.Invoke(this, _table);
        }

        /// <summary>
        /// Egy épület MaintenancePaying event fogadása, levonja a játékos pénzéből a fenntartási díjat
        /// </summary>
        /// <param name="sender">Az eventet küldő épület</param>
        /// <param name="maintenance">A fenntartás díja</param>
        private void OnMaintenancePaying(object sender, int maintenance)
        {
            _cash -= maintenance;
            OnCashChanged();
        }


        /// <summary>
        /// Egy épület UpkeepPaying event fogadása
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpkeepPaying(object sender, int e)
        {
            if (_cash >= e)
            {
                _cash -= e;
                (sender as Building).CanFunction = true;

                OnCashChanged();
            }
        }

        /// <summary>
        /// Egy vidámparkijáték elindulásának eseményét kezelő függvény
        /// </summary>
        /// <param name="sender">Eseményt küldő játék</param>
        /// <param name="fee">A játék üzemeltetésének összege</param>
        private void OnRideStarted(object sender, int fee)
        {
        }

        /// <summary>
        /// Épület építésének meghiúsulását kiváltó esemény
        /// </summary>
        private void OnBuildingUnsuccesful(string message)
        {
            BuildingUnsuccessful?.Invoke(this, message);
        }


        /// <summary>
        /// Csomópontok hozzáadása a gráfhoz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rect"></param>
        private void AddNodesToGraph(object sender, Rectangle rect)
        {
            for (int i = 0; i < rect.Width; i++) // szélesség
            {
                for (int j = 0; j < rect.Height; j++) // hossz
                {
                    _roadGraph.AddNode(new Point(rect.X + i, rect.Y + j));
                }
            }
            Infrastructure inf = sender as Infrastructure;
            int initX = TableSizeX / 2;
            int initY = TableSizeY - 2;
            Queue<Point> road = _roadGraph.GetShortestPath(new Point(initX, initY), new Point(inf.Position[0], inf.Position[1] + inf.Length - 1));
            if (road != null)
            {
                inf.Reachable = true;
                if (inf is Playground)
                {
                    _playgrounds.Add(_infrastructures.FindIndex(x => inf.Position[0] == x.Position[0] && inf.Position[1] == x.Position[1]));
                }
                else if (inf is Canteen)
                {
                    _canteens.Add(_infrastructures.FindIndex(x => inf.Position[0] == x.Position[0] && inf.Position[1] == x.Position[1]));
                }
                else if (inf is Road)
                {
                    _roads.Add(_infrastructures.FindIndex(x => inf.Position[0] == x.Position[0] && inf.Position[1] == x.Position[1]));
                }

                foreach (Infrastructure infrastructure in _infrastructures)
                {
                    if (!infrastructure.Reachable && _roadGraph.GetShortestPath(new Point(initX, initY), new Point(infrastructure.Position[0], infrastructure.Position[1] + infrastructure.Length - 1)) != null)
                    {
                        infrastructure.Reachable = true;
                        if (infrastructure is Playground)
                        {
                            _playgrounds.Add(_infrastructures.FindIndex(x => infrastructure.Position[0] == x.Position[0] && infrastructure.Position[1] == x.Position[1]));
                        }
                        else if (infrastructure is Canteen)
                        {
                            _canteens.Add(_infrastructures.FindIndex(x => infrastructure.Position[0] == x.Position[0] && infrastructure.Position[1] == x.Position[1]));
                        }
                        else if (infrastructure is Road)
                        {
                            _roads.Add(_infrastructures.FindIndex(x => infrastructure.Position[0] == x.Position[0] && infrastructure.Position[1] == x.Position[1]));
                        }
                    }
                }
            }
        }

        #endregion
    }
}
