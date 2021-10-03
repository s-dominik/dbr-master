using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DBR.Model
{
    /// <summary>
    /// Csomópont osztály az úthálózathoz
    /// </summary>
    public class Node
    {
        #region Fields

        #endregion


        #region Properties

        /// <summary>
        /// Csomópont koordinátái
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Szomszédos csomópontok listája
        /// </summary>
        public List<Node> Neighbours { get; set; }

        #endregion


        #region Constructor

        /// <summary>
        /// A csomópont osztály konstruktora, amely inicializálja
        /// a csomópont koordinátáit a kapott ponttal.
        /// </summary>
        /// <param name="p">A csomópont koordinátái</param>
        public Node(Point p)
        {
            Position = p;

            Neighbours = new List<Node>();
        }

        /// <summary>
        /// A csomópont osztály konstruktora, amely inicializálja
        /// a csomópont koordinátáit a kapott értékekkel.
        /// </summary>
        /// <param name="x">A csomópont abszcisszája</param>
        /// <param name="y">A csomópont ordinátája</param>
        public Node(int x, int y) : this(new Point(x,y))
        {
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Két csomópont távolságát megadó statikus függvény.
        /// Ha nem szomszédosak, akkor a távolság végtelen.
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        public static float Distance(Node n1, Node n2)
        {
            return n1.Distance(n2);
        }

        /// <summary>
        /// Két csomópont távolságát megadó függvény.
        /// Ha nem szomszédosak, akkor a távolság végtelen.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public float Distance(Node other)
        {
            return Neighbours.Contains(other) || other.Neighbours.Contains(this) || this == other
                ? Math.Abs(Position.X - other.Position.X) + Math.Abs(Position.Y - other.Position.Y)
                : float.PositiveInfinity;
        }

        #endregion
    }
}
