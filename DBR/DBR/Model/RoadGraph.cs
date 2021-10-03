using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Linq;

namespace DBR.Model
{
    /// <summary>
    /// Úthálózat osztálya
    /// </summary>
    public class RoadGraph
    {
        #region Fields

        /// <summary>
        /// Az úthálózat csomópontjainak listája
        /// </summary>
        private List<Node> nodes;

        private float[,] distances;
        private int[,] predecessors;

        #endregion


        #region Properties

        /// <summary>
        /// Csúcspontok számát adja vissza
        /// </summary>
        public int NodeCount { get { return nodes.Count; } }

        /// <summary>
        /// Mátrixot reprezentáló sorozat hossza
        /// </summary>
        public int MatrixElementApproximation { get { return predecessors.Length; } }

        public long PendingWorkItemCount { get { return ThreadPool.PendingWorkItemCount; } }

        public long CompletedWorkItemCount { get { return ThreadPool.CompletedWorkItemCount; } }

        public long ThreadCount { get { return ThreadPool.ThreadCount; } }

        public float[,] DistancesMatrix
        {
            get
            {
                return distances;
            }
        }

        public int[,] PredecessorsMatrix
        {
            get
            {
                return predecessors;
            }
        }

        #endregion


        #region Events



        #endregion


        #region Constructor

        /// <summary>
        /// Az úthálózat osztály konstruktora, inicializálja
        /// a csomópontok listáját.
        /// </summary>
        public RoadGraph()
        {
            nodes = new List<Node>();
        }

        public RoadGraph(RoadGraph copy)
        {
            nodes = copy.nodes;
            distances = copy.distances;
            predecessors = copy.predecessors;
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Csomópont hozzáadása a gráfhoz
        /// </summary>
        /// <param name="p"></param>
        public void AddNode(Point p)
        {
            Node node = new Node(p);

            LookUpNeighbours(node);

            nodes.Add(node);

            FloydWarshall();
        }

        /// <summary>
        /// Csomópont keresése adott feltétel alapján.
        /// </summary>
        /// <param name="match">Keresés predikátuma</param>
        /// <returns></returns>
        public Node FindNode(Predicate<Node> match)
        {
            return nodes.Find(match);
        }

        /// <summary>
        /// Az összes, feltételnek eleget tevő csomópont megtalálása
        /// </summary>
        /// <param name="match">Keresés predikátuma</param>
        /// <returns></returns>
        public List<Node> FindAllNodes(Predicate<Node> match)
        {
            return nodes.FindAll(match);
        }

        /// <summary>
        /// Az összes, feltételnek eleget tevő csomópont megtalálása
        /// és koordinátáinak visszaadása.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<Point> FindAllPoints(Predicate<Node> match)
        {
            return nodes.FindAll(match).Select(n => n.Position).ToList();
        }

        /// <summary>
        /// Szomszédok megkeresése a listában.
        /// </summary>
        /// <param name="node">Középpont, aminek a szomszédait keressük</param>
        public void LookUpNeighbours(Node node)
        {
            node.Neighbours = nodes.FindAll(n => Math.Abs(n.Position.X - node.Position.X) + Math.Abs(n.Position.Y - node.Position.Y) == 1);

            foreach (var neigh in node.Neighbours)
            {
                neigh.Neighbours.Add(node);
            }
        }

        /// <summary>
        /// Legrövidebb út megtalálása két pont között, ha van
        /// </summary>
        /// <param name="from">Kiindulópont</param>
        /// <param name="to">Cél</param>
        /// <returns>A két végpont közötti csomópontok listája, ha van, egyébként null</returns>
        public Queue<Point> GetShortestPath(Point from, Point to)
        {
            Queue<Point> path = new Queue<Point>();

            int fromInd = GetIndexOfNode(from);
            int toInd = GetIndexOfNode(to);
            if (fromInd == -1 || toInd == -1 || distances[fromInd, toInd] == float.PositiveInfinity)
                return null;

            path.Enqueue(from);

            while (from != to)
            {
                Node current = nodes[predecessors[GetIndexOfNode(from), GetIndexOfNode(to)]];
                from = new Point(current.Position.X, current.Position.Y);
                path.Enqueue(from);
            }

            return path;
        }

        /// <summary>
        /// Gráf adatainak kiírása egyszerű változókba
        /// </summary>
        /// <param name="nodeCount">Csomópontok száma</param>
        /// <param name="points">Csomópontok koordinátáinak listája</param>
        /// <param name="neighbourhood">Szomszédsági mátrix</param>
        public void MakeSerializable(out int nodeCount, out List<Point> points, out int[,] neighbourhood)
        {
            nodeCount = nodes.Count;
            points = new List<Point>();
            neighbourhood = new int[nodeCount, nodeCount];

            foreach (Node node in nodes)
            {
                points.Add(new Point(node.Position.X, node.Position.Y));
            }

            for (int i = 0; i < nodeCount; i++)
            {
                for (int j = 0; j < nodeCount; j++)
                {
                    if (nodes[i].Neighbours.Contains(nodes[j]))
                        neighbourhood[i, j] = 1;
                }
            }
        }

        /// <summary>
        /// Gráf adatainak betöltése
        /// </summary>
        /// <param name="points">Csomópontok koordinátái</param>
        /// <param name="neighbourhood">Szomszédsági mátrix</param>
        public void MakeGraph(List<Point> points, int[,] neighbourhood)
        {
            nodes = new List<Node>();

            foreach (Point point in points)
            {
                nodes.Add(new Node(point));
            }

            for (int i = 0; i < neighbourhood.GetLength(0); i++)
            {
                for (int j = 0; j < neighbourhood.GetLength(1); j++)
                {
                    if (neighbourhood[i, j] == 1)
                        nodes[i].Neighbours.Add(nodes[j]);
                }
            }

            FloydWarshall();
        }

        /// <summary>
        /// Megadja két csúcspont távolságát a gráfban
        /// </summary>
        /// <param name="from">Kezdő csúcspont</param>
        /// <param name="to">Cél csúcspont</param>
        /// <returns>A távolság, ha kapcsolódnak a gráfon belül, egyébként végtelen</returns>
        public float GetDistanceOfPoints(Point from, Point to)
        {
            int fromInd = nodes.FindIndex(n => n.Position == from);
            int toInd = nodes.FindIndex(n => n.Position == to);

            if (fromInd == -1 || toInd == -1)
                return float.PositiveInfinity;

            return distances[fromInd, toInd];
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Legrövidebb útpárok kiszámolása
        /// </summary>
        private void FloydWarshall()
        {
            int count = nodes.Count;

            distances = new float[count, count];
            predecessors = new int[count, count];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    distances[i, j] = nodes[i].Distance(nodes[j]);
                    predecessors[i, j] = j;
                }
            }

            for (int k = 0; k < count; k++)
            {
                for (int i = 0; i < count; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        if(distances[i,j] > distances[i,k] + distances[k, j])
                        {
                            distances[i, j] = distances[i, k] + distances[k, j];
                            predecessors[i, j] = predecessors[i, k];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Kétdimenziós indexet egydimenzióssá konvertál.
        /// Egy tömbben tárolt mátrix indexelésére alkalmas.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private int LTMIndex(int i, int j)
        {
            if (i == -1 || j == -1)
                return -1;

            if (i < j)
                return LTMIndex(j, i);

            if (i > j)
                return (i * (i - 1)) / 2 + j + 1;

            return 0;
        }

        private int GetIndexOfNode(int x, int y)
        {
            return GetIndexOfNode(new Point(x, y));
        }

        private int GetIndexOfNode(Point point)
        {
            return nodes.IndexOf(nodes.Find(n => n.Position == point));
        }

        #endregion
    }
}
