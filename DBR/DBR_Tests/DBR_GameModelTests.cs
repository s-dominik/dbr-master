using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBR.Model;
using DBR.Persistance;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Drawing;

namespace DBR_Tests
{
    [TestClass]
    public class DBR_GameModelTests
    {
        //A tesztelendő modell
        private GameModel _model;
        //Mockolt adatok
        private GameData _mockedData;
        //Az adatelérés mock-ja
        private Mock<IDataAccess> _mock;

        [TestInitialize]
        public void Initialize()
        {
            _mock = new Mock<IDataAccess>();
            _mockedData = new GameData();

            //A mock a LoadAsync műveletben bármilyen paraméterre az előre beállított játéktáblát fogja visszaadni
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<string>())).Returns(() => Task.FromResult(_mockedData));

            //Példányosítjuk a modellt a mock objektummal
            _model = new GameModel(_mock.Object);
        }

        [TestMethod]
        public void TestNewGame()
        {
            _model.NewGame();

            //Gráf csomópontjainak száma
            Assert.AreEqual(_model.GraphNodeCount, 1);

            //Épületek száma
            Assert.AreEqual(_model.BuildingList.Count, 1);

            //Látogatók száma
            Assert.AreEqual(_model.Visitors.Count, 0);

            //Karbantartók száma
            Assert.AreEqual(_model.Repairmen.Count, 0);

            //Idő
            Assert.AreEqual(_model.Time, 0);

            //Karbantartó felváteli díja
            Assert.AreEqual(_model.PriceOfRepairman, 3000);

            //Javítási költség
            Assert.AreEqual(_model.PriceOfRepairing, 1000);

            //Játékos pénze
            Assert.AreEqual(_model.Cash, 100000);

            //Park nyitva van-e
            Assert.IsFalse(_model.isOpened);

            //Vendéglátóhelyek száma
            Assert.AreEqual(_model.Canteens.Count, 0);

            //Játékok száma
            Assert.AreEqual(_model.Playgrounds.Count, 0);

            //Útszakaszok száma
            Assert.AreEqual(_model.Roads.Count, 1);

            //Tábla generálása
            for (int i = 0; i < _model.TableSizeY; i++)
            {
                for (int j = 0; j < _model.TableSizeX; j++)
                {
                    if (i == _model.TableSizeY - 1 && (j == _model.TableSizeX / 2 || j == _model.TableSizeX / 2 + 1))
                        Assert.IsTrue(_model.Table[i, j] < 0);
                    else if (i == 0 || i == _model.TableSizeY - 1 || j == 0 || j == _model.TableSizeX - 1)
                        Assert.IsTrue(_model.Table[i, j] < 0);
                    else if (i == 28 && j == 20)
                        Assert.AreEqual(_model.Table[i, j], 0);
                    else
                        Assert.AreEqual(_model.Table[i, j], -1);
                }
            }
        }

        [TestMethod]
        public async Task LoadTest()
        {
            _model.NewGame();

            await _model.LoadGameAsync(string.Empty);

            for (int i = 0; i < _model.TableSizeY; i++)
            {
                for (int j = 0; j < _model.TableSizeX; j++)
                {
                    Assert.AreEqual(_mockedData.Table.GetValue(i, j), _model.Table.GetValue(i, j));
                }
            }

            List<List<int>> infrastructureProperties = new List<List<int>>();

            Assert.AreEqual(_mockedData.NumberOfInfrastructures, _model.BuildingList.Count-1);

            if (_model.BuildingList.Count > 1)
            {
                int infIndex = 1;
                foreach (Infrastructure infrastructure in _model.BuildingList)
                {
                    Assert.AreEqual(_mockedData.InfrastructureNames[infIndex - 1], _model.BuildingList[infIndex].Name);

                    List<int> infProperties = new List<int>();

                    if (infrastructure is Canteen)
                        infProperties.Add(1);
                    else if (infrastructure is Playground)
                        infProperties.Add(2);
                    else if (infrastructure is Plant)
                        infProperties.Add(3);
                    else if (infrastructure is Road)
                        infProperties.Add(4);
                    infProperties.Add(infrastructure.Position[0]);
                    infProperties.Add(infrastructure.Position[1]);
                    infProperties.Add(infrastructure.BuildPrice);
                    infProperties.Add(infrastructure.BuildTime);
                    infProperties.Add(infrastructure.Length);
                    infProperties.Add(infrastructure.Width);
                    infProperties.Add((int)infrastructure.State);
                    infProperties.Add(infrastructure.ConstructionRemainingTime);

                    var building = infrastructure as Building;
                    var playground = infrastructure as Playground;
                    var canteen = infrastructure as Canteen;

                    if (building != null)
                    {
                        infProperties.Add(building.Fee);
                        infProperties.Add(building.Maintenance);
                        infProperties.Add(building.TimeForNextMaintenancePay);
                        infProperties.Add(building.Capacity);
                    }
                    else
                    {
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                    }

                    if (playground != null)
                    {
                        infProperties.Add(playground.MinUtilization);
                        infProperties.Add(playground.RideDuration);
                        infProperties.Add(playground.RideTime);
                        infProperties.Add(playground.RepairTime);
                        infProperties.Add(playground.ChanceToBroke);
                        infProperties.Add(playground.CurrentRepairman);
                        if (playground.RepairmanArrivedProp)
                            infProperties.Add(1);
                        else infProperties.Add(0);
                        if (playground.RepairmanOnTheWayProp)
                            infProperties.Add(1);
                        else infProperties.Add(0);
                    }
                    else
                    {
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                    }

                    if (building != null)
                    {
                        infProperties.Add(building.Upkeep);
                        infProperties.Add(building.CanFunction ? 1 : 0);
                    }
                    else
                    {
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                    }

                    if (canteen != null)
                    {
                        infProperties.Add(canteen.ServingDuration);
                        infProperties.Add(canteen.ServingTime);
                    }
                    else
                    {
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                    }

                    // Használók és várakozók mentése
                    if (canteen != null)
                    {
                        infProperties.Add(canteen.ClientsServed.Count);

                        foreach(var client in canteen.ClientsServed)
                        {
                            infProperties.Add(_model.Visitors.IndexOf(client.Key));
                            infProperties.Add(client.Value);
                        }

                        infProperties.Add(canteen.WaitingVisitors.Count);
                        Queue<Visitor> tmp = new Queue<Visitor>();
                        while (canteen.WaitingVisitors.Count > 0)
                        {
                            int index = _model.Visitors.IndexOf(canteen.WaitingVisitors.Dequeue());
                            infProperties.Add(index);
                            tmp.Enqueue(_model.Visitors[index]);
                        }
                        canteen.WaitingVisitors = tmp;
                    }
                    else if (playground != null)
                    {
                        infProperties.Add(playground.UsingVisitors.Length);
                        for (int i = 0; i < playground.UsingVisitors.Length; i++)
                        {
                            infProperties.Add(_model.Visitors.IndexOf(playground.UsingVisitors[i]));
                            infProperties.Add(-1);
                        }

                        infProperties.Add(playground.WaitingVisitors.Count);
                        Queue<Visitor> tmp = new Queue<Visitor>();
                        while (playground.WaitingVisitors.Count > 0)
                        {
                            int index = _model.Visitors.IndexOf(playground.WaitingVisitors.Dequeue());
                            infProperties.Add(index);
                            tmp.Enqueue(_model.Visitors[index]);
                        }
                        playground.WaitingVisitors = tmp;
                    }
                    else
                    {
                        infProperties.Add(-1);
                        infProperties.Add(-1);
                    }

                    Assert.AreEqual(_mockedData.InfrastructureProperties[infIndex], infProperties);

                    infIndex++;
                }
            }

            for (int i = 0, j = 0; j < _mockedData.NodeCount * 2; i++, j += 2) 
            {
                Assert.AreEqual(_mockedData.Points[j], _model.GraphNodePoints[i].X);
                Assert.AreEqual(_mockedData.Points[j+1], _model.GraphNodePoints[i].Y);
            }

            RoadGraph loadedGraph = new RoadGraph(_model.Graph);
            List<Point> points = new List<Point>();
            for (int i = 0; i < _mockedData.NodeCount * 2; i += 2)
            {
                points.Add(new Point(_mockedData.Points[i], _mockedData.Points[i + 1]));
            }
            loadedGraph.MakeGraph(points, _mockedData.Neighbourhood);
            for(int i=0; i<_model.Graph.NodeCount; i++)
            {
                for(int j=0; j<_model.Graph.NodeCount; j++)
                {
                    Assert.AreEqual(_model.Graph.PredecessorsMatrix[i, j], loadedGraph.PredecessorsMatrix[i,j]);
                }
            }

            Assert.AreEqual(_model.Visitors.Count, _mockedData.NumberOfVisitors);

            int visitorIndex = 0;
            foreach (Visitor visitor in _model.Visitors)
            {
                List<int> visitorProperties = new List<int>();
                visitorProperties.Add(visitor.Position.X);
                visitorProperties.Add(visitor.Position.Y);
                visitorProperties.Add(visitor.ScreenPosition.X);
                visitorProperties.Add(visitor.ScreenPosition.Y);
                visitorProperties.Add((int)visitor.State);
                visitorProperties.Add(visitor.Money);
                visitorProperties.Add(visitor.Mood);
                visitorProperties.Add(visitor.Satiety);
                visitorProperties.Add(visitor.PatienceTime);
                visitorProperties.Add(visitor.SpriteID);
                visitorProperties.Add(visitor.RoadForpersistence.Count * 2);
                foreach (Point step in visitor.RoadForpersistence)
                {
                    visitorProperties.Add(step.X);
                    visitorProperties.Add(step.Y);
                }

                Assert.AreEqual(visitorProperties, _mockedData.VisitorProperties[visitorIndex]);
                visitorIndex++;
            }

            Assert.AreEqual(_model.Repairmen.Count, _mockedData.NumberOfRepairmen);

            int repairmanIndex = 0;
            foreach (Repairman repairman in _model.Repairmen)
            {
                List<int> repairmanProperties = new List<int>();

                repairmanProperties.Add(repairman.Position.X);
                repairmanProperties.Add(repairman.Position.Y);
                repairmanProperties.Add(repairman.ScreenPosition.X);
                repairmanProperties.Add(repairman.ScreenPosition.Y);
                repairmanProperties.Add((int)repairman.State);
                repairmanProperties.Add(repairman.OnDuty ? 1 : 0);
                repairmanProperties.Add(repairman.SpriteID);
                repairmanProperties.Add(repairman.RoadForpersistence.Count * 2);
                foreach (Point step in repairman.RoadForpersistence)
                {
                    repairmanProperties.Add(step.X);
                    repairmanProperties.Add(step.Y);
                }

                Assert.AreEqual(_mockedData.RepairmanProperties[repairmanIndex], repairmanProperties);

                repairmanIndex++;
            }

            //Ellenőrizzük, hogy meghívták-e a Load műveletet a megadott paraméterrel
            _mock.Verify(dataAccess => dataAccess.LoadAsync(string.Empty), Times.Once());
        }

        [TestMethod]
        public void TestTimeChanges()
        {
            _model.NewGame();

            Assert.AreEqual(_model.Time, 0);

            _model.TimeAdvanced();

            Assert.AreEqual(_model.Time, 1);

            _model.TimeAdvanced();

            Assert.AreEqual(_model.Time, 2);
        }

        [TestMethod]
        public void TestBuilding()
        {
            _model.NewGame();

            //Canteen
            _model.BuildingInProgress(1, 2, 3, 200, 3, 3, 3, 2, 3, 30, 10, 1, "Canteen", 1, 1);

            Assert.AreEqual(_model.BuildingList.Count, 2);

            int x = _model.BuildingList[^1].Position[0];
            int y = _model.BuildingList[^1].Position[1];
            int width = _model.BuildingList[^1].Width;
            int length = _model.BuildingList[^1].Length;

            for (int j = x; j < x + width; j++)
            {
                for (int i = y; i < y + length; i++)
                {
                    Assert.AreEqual(_model.Table[i, j], _model.BuildingList.Count - 1);
                }
            }

            Assert.IsInstanceOfType(_model.BuildingList[^1], typeof(Canteen));

            //Game
            _model.BuildingInProgress(2, 10, 3, 200, 3, 3, 3, 2, 3, 30, 10, 1, "Rollercoaster", 1, 1);

            Assert.AreEqual(_model.BuildingList.Count, 3);

            x = _model.BuildingList[^1].Position[0];
            y = _model.BuildingList[^1].Position[1];
            width = _model.BuildingList[^1].Width;
            length = _model.BuildingList[^1].Length;

            for (int j = x; j < x + width; j++)
            {
                for (int i = y; i < y + length; i++)
                {
                    Assert.AreEqual(_model.Table[i, j], _model.BuildingList.Count - 1);
                }
            }

            Assert.IsInstanceOfType(_model.BuildingList[^1], typeof(Playground));

            //Plant
            _model.BuildingInProgress(3, 20, 3, 200, 3, 3, 3, 2, 3, 30, 10, 1, "Tree", 1, 1);

            Assert.AreEqual(_model.BuildingList.Count, 4);

            x = _model.BuildingList[^1].Position[0];
            y = _model.BuildingList[^1].Position[1];
            width = _model.BuildingList[^1].Width;
            length = _model.BuildingList[^1].Length;

            for (int j = x; j < x + width; j++)
            {
                for (int i = y; i < y + length; i++)
                {
                    Assert.AreEqual(_model.Table[i, j], _model.BuildingList.Count - 1);
                }
            }

            Assert.IsInstanceOfType(_model.BuildingList[^1], typeof(Plant));

            //Road
            _model.BuildingInProgress(4, 30, 3, 200, 3, 3, 3, 2, 3, 30, 10, 1, "Road", 1, 1);

            Assert.AreEqual(_model.BuildingList.Count, 5);

            x = _model.BuildingList[^1].Position[0];
            y = _model.BuildingList[^1].Position[1];
            width = _model.BuildingList[^1].Width;
            length = _model.BuildingList[^1].Length;

            for (int j = x; j < x + width; j++)
            {
                for (int i = y; i < y + length; i++)
                {
                    Assert.AreEqual(_model.Table[i, j], _model.BuildingList.Count - 1);
                }
            }

            Assert.IsInstanceOfType(_model.BuildingList[^1], typeof(Road));

            //Trying to build on a building
            _model.BuildingInProgress(3, 20, 3, 200, 3, 3, 3, 2, 3, 30, 10, 1, "Bush", 1, 1);

            Assert.AreEqual(_model.BuildingList.Count, 5);

            x = _model.BuildingList[^1].Position[0];
            y = _model.BuildingList[^1].Position[1];
            width = _model.BuildingList[^1].Width;
            length = _model.BuildingList[^1].Length;

            for (int j = x; j < x + width; j++)
            {
                for (int i = y; i < y + length; i++)
                {
                    Assert.AreEqual(_model.Table[i, j], 4);
                }
            }

            Assert.IsInstanceOfType(_model.BuildingList[^1], typeof(Road));
        }

        [TestMethod]
        public void TestCashChanges()
        {
            _model.NewGame();

            int sum = _model.Cash;

            _model.BuildingInProgress(1, 30, 3, 200, 3, 3, 3, 2, 3, 30, 10, 1, "Canteen", 1, 1);

            Assert.AreEqual(_model.Cash, sum - _model.BuildingList[^1].BuildPrice);

            sum = _model.Cash;

            _model.EntranceFee = 1000;
            _model.CollectFee(null, _model.EntranceFee);

            Assert.AreEqual(_model.Cash, sum + _model.EntranceFee);
        }

        [TestMethod]
        public void TestPlantEffect()
        {
            _model.NewGame();

            _model.Visitors.Add(new Visitor(20,28, 1000, 90, 90, 2));
            Assert.AreEqual(_model.Visitors[^1].Mood, 90);

            _model.BuildingInProgress(3, 21, 28, 200, 3, 3, 3, 1, 1, 0, 0, 1, "Tree", 1 , 1);
            _model.PlantEffectOnVisitors(_model.Visitors[^1]);
            Assert.AreEqual(_model.Visitors[^1].Mood, 95);
        }

        [TestMethod]
        public void TestVisitorsAndRepairmenNumber()
        {
            _model.NewGame();
            _model.Visitors.Add(new Visitor(20, 28, 1000, 90, 90, 2));
            Assert.AreEqual(_model.Visitors.Count, 1);
            _model.Visitors.Add(new Visitor(20, 28, 1000, 90, 90, 2));
            Assert.AreEqual(_model.Visitors.Count, 2);
            _model.Visitors.Add(new Visitor(20, 28, 1000, 90, 90, 2));
            Assert.AreEqual(_model.Visitors.Count, 3);

            _model.Repairmen.Add(new Repairman(20, 28, 2));
            Assert.AreEqual(_model.Repairmen.Count, 1);
            _model.Repairmen.Add(new Repairman(20, 28, 2));
            Assert.AreEqual(_model.Repairmen.Count, 2);
        }

        [TestMethod]
        public void TestSendAndCallRepairman()
        {
            _model.NewGame();
            _model.BuildingInProgress(2, 21, 24, 1000, 10, 10, 10, 5, 5, 10, 10, 5, "Carousel", 10, 10);
            _model.Graph.AddNode(new Point(21, 28));
            _model.Graph.AddNode(new Point(22, 28));
            _model.Graph.AddNode(new Point(23, 28));
            _model.Graph.AddNode(new Point(24, 28));
            _model.Graph.AddNode(new Point(25, 28));
            Assert.AreEqual(_model.Graph.NodeCount, 6);

            Playground inf = (Playground)_model.BuildingList[^1];
            Repairman rep = new Repairman(20, 28, 2);
            _model.Repairmen.Add(rep);

            _model.SendRepairman(inf, _model.Repairmen.Count - 1);
            Assert.IsFalse(rep.OnDuty);
            Assert.AreEqual(rep.State, Person.PersonCondition.Ply);

            _model.CallRepairman(inf, new int[2] { inf.Position[0], inf.Position[1] + inf.Length - 1} );
            Assert.IsTrue(rep.OnDuty);
            Assert.AreEqual(rep.State, Person.PersonCondition.Ply);
        }

        [TestMethod]
        public void TestRepairing()
        {
            _model.NewGame();
            Repairman rep = new Repairman(20, 28, 2);
            _model.Repairmen.Add(rep);
            rep.OnDuty = true;

            int money = _model.Cash;
            _model.Repairing(rep, EventArgs.Empty);
            Assert.AreEqual(_model.Cash, money - _model.PriceOfRepairing);
            Assert.AreEqual(rep.State, Person.PersonCondition.Waiting);
        }

        [TestMethod]
        public void TestAddNodesToGraph()
        {
            _model.NewGame();
            _model.BuildingInProgress(4, 21, 28, 100, 10, 10, 10, 1, 1, 10, 10, 10, "Road", 10, 10);
            Infrastructure inf = _model.BuildingList[^1];
            Rectangle rect = new Rectangle(inf.Transform.X, inf.Transform.Y + inf.Transform.Height - 1, inf.Transform.Width, 1);

            int k = _model.Graph.NodeCount - 1;
            for (int i = rect.Width; i > 0; i--)
            {
                for (int j = rect.Height; j > 0 ; j--)
                {
                    Assert.AreEqual(rect.X - i, _model.GraphNodePoints[k].X);
                    Assert.AreEqual(rect.Y - j + 1, _model.GraphNodePoints[k].Y);
                }
            }
        }

        [TestMethod]
        public void TestGetShortestPath()
        {
            _model.NewGame();
            _model.Graph.AddNode(new Point(20, 27));
            _model.Graph.AddNode(new Point(20, 26));
            _model.Graph.AddNode(new Point(21, 26));
            _model.Graph.AddNode(new Point(21, 27));

            Assert.AreEqual(_model.Graph.NodeCount, 5);

            Queue<Point> que = _model.Graph.GetShortestPath(new Point(20, 26), new Point(20, 28));
            Point p1 = que.Dequeue();
            Point p2 = que.Dequeue();
            Point p3 = que.Dequeue();
            Assert.AreEqual(p1.X, 20);
            Assert.AreEqual(p1.Y, 26);
            Assert.AreEqual(p2.X, 20);
            Assert.AreEqual(p2.Y, 27);
            Assert.AreEqual(p3.X, 20);
            Assert.AreEqual(p3.Y, 28);
        }

        [TestMethod]
        public void TestGetDistanceOfPoints()
        {
            _model.NewGame();
            _model.Graph.AddNode(new Point(20, 27));
            _model.Graph.AddNode(new Point(20, 26));
            _model.Graph.AddNode(new Point(20, 25));
            _model.Graph.AddNode(new Point(21, 25));
            _model.Graph.AddNode(new Point(21, 26));
            _model.Graph.AddNode(new Point(21, 27));

            Assert.AreEqual(_model.Graph.NodeCount, 7);

            float dis = _model.Graph.GetDistanceOfPoints(new Point(20, 28), new Point(20, 25));
            Assert.AreEqual(dis, (float)3);
        }

        [TestMethod]
        public void TestHireRepairman()
        {
            _model.NewGame();
            DBR.TicketBooth t = new DBR.TicketBooth();
            int money = _model.Cash;
            _model.HireRepairman(t, EventArgs.Empty);

            Assert.AreEqual(_model.Repairmen.Count, 1);
            Assert.AreEqual(_model.Cash, money - _model.PriceOfRepairman);
            Repairman rep = _model.Repairmen[^1];
            Assert.AreEqual(rep.State, Person.PersonCondition.Ply);
        }

        [TestMethod]
        public void TestOpening()
        {
            _model.NewGame();
            DBR.TicketBooth t = new DBR.TicketBooth();
            _model.Opening(t, 100);
            Assert.IsTrue(_model.isOpened);
            Assert.AreEqual(_model.EntranceFee, 100);
        }

        [TestMethod]
        public void TestStep()
        {
            _model.NewGame();
            _model.BuildingInProgress(4, 20, 27, 1000, 10, 10, 10, 1, 1, 10, 10, 10, "Road", 10, 10);
            _model.Graph.AddNode(new Point(30, 37));

            DBR.TicketBooth t = new DBR.TicketBooth();
            _model.HireRepairman(t, EventArgs.Empty);
            Assert.AreEqual(_model.Repairmen.Count, 1);
            
            _model.Repairmen[^1].Road.Enqueue(new Point(20, 27));

            Assert.AreEqual(_model.Repairmen[^1].Position, new Point(20, 28));
            Assert.AreEqual(_model.Repairmen[^1].Road.Count, 2);

            _model.Repairmen[^1].Step(50, 2);

            Assert.AreEqual(_model.Repairmen[^1].Road.Count, 1);
            Assert.AreEqual(_model.Repairmen[^1].Position, new Point(20, 28));
            
            for (int i = 0; i < 26; i++)
            {
                _model.Repairmen[^1].Step(50, 2);
            }

            Assert.AreEqual(_model.Repairmen[^1].Road.Count, 0);
            Assert.AreEqual(_model.Repairmen[^1].Position, new Point(20, 27));
        }

        [TestMethod]
        public void TestVisitorMethods()
        {
            _model.NewGame();
            _model.Visitors.Add(new Visitor(20, 28, 1000, 100, 3, 2));
            Visitor vis = _model.Visitors[^1];
            int sat = vis.Satiety;
            int mood = vis.Mood;
            int money = vis.Money;
            int patienceTime = vis.PatienceTime;

            vis.Eating(10);
            Assert.AreEqual(vis.Satiety, sat + 10 > 100 ? 100 : sat + 10);
            sat = vis.Satiety;

            vis.Playing(10);
            Assert.AreEqual(vis.Mood, mood + 10 > 100 ? 100 : mood + 10);
            mood = vis.Mood;

            vis.PlantEffect(10);
            Assert.AreEqual(vis.Mood, mood + 10 > 100 ? 100 : mood + 10);
            mood = vis.Mood;

            vis.PayFee(100);
            Assert.AreEqual(vis.Money, money - 100);
            money = vis.Money;

            vis.MoodChanges();
            Assert.AreEqual(vis.Mood, mood - 2);
            mood = vis.Mood;
            Assert.AreEqual(vis.Satiety, sat - 1);
            sat = vis.Satiety;
            Assert.AreEqual(vis.PatienceTime, 10);

            vis.State = Person.PersonCondition.Waiting;
            vis.PatienceTime = 0;
            vis.MoodChanges();
            Assert.AreEqual(vis.Mood, mood - 3);
            Assert.AreEqual(vis.Satiety, sat - 1);
        }
    }
}
