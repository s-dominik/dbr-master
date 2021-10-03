using System;
using System.Collections.Generic;
using System.Drawing;

namespace DBR.Model
{
    public class Person
    {
        #region Enums

        /// <summary>
        /// Az emberek állapota
        /// </summary>
        public enum PersonCondition
        {
            Ply = 0, //Közlekedik
            Use = 1, //Használ
            Waiting = 2, //Várakozik
        }

        #endregion

        #region Fields

        /// <summary>
        /// Az ember X koordinátája
        /// </summary>
        protected int _posX;

        /// <summary>
        /// Az ember Y koordinátája 
        /// </summary>
        protected int _posY;

        /// <summary>
        /// Az emberek állapota
        /// </summary>
        protected PersonCondition _state;

        /// <summary>
        /// Az emberek uta
        /// </summary>
        protected Queue<Point> _road;

        #endregion

        #region Properties

        public int SpriteID { get; set; }

        /// <summary>
        /// Az ember pozíciójának lekérdezése
        /// </summary>
        public Point Position { get { return new Point(_posX, _posY); } private set { _posX = value.X; _posY = value.Y; } }

        public Point ScreenPosition { get; set; }

        /// <summary>
        /// Az ember állapotának lekérdezése
        /// </summary>
        public PersonCondition State { get { return _state; } set { _state = value; } }

        /// <summary>
        /// Az ember utjának lekérdezése és beálítása
        /// </summary>
        public Queue<Point> Road
        {
            get { return _road; }
            set { _road = value; OnStart(this, EventArgs.Empty); }
        }

        public Queue<Point> RoadForpersistence { get { return _road;  } set { _road = value; } }

        public Point ScreenOffset { get; private set; }
        #endregion

        #region Events

        /// <summary>
        /// Az ember indulását jelző esemény. 
        /// </summary>
        public event EventHandler Start;

        /// <summary>
        /// Az ember megérkezését jelző esemény
        /// </summary>
        public event EventHandler Arrived;

        #endregion

        #region Constructors

        /// <summary>
        /// Az ember osztály konstruktora, inicializálja az adatagokat
        /// </summary>
        /// <param name="x">Az ember x koordinátája</param>
        /// <param name="y">Az ember y koordinátája</param>
        public Person(int x, int y, int maxid)
        {
            _posX = x;
            _posY = y;
            _state = PersonCondition.Ply;
            _road = new Queue<Point>();

            ScreenPosition = new Point();

            Random rnd = new Random();
            ScreenOffset = new Point(rnd.Next(25), rnd.Next(25));
            SpriteID = rnd.Next(maxid);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Az ember lépkedése
        /// </summary>
        public void Step(int tileSize, int speedMultiplier)
        {
            if(State == PersonCondition.Ply)
            {
                if (ScreenPosition.X == 0 && ScreenPosition.Y == 0)
                {
                    ScreenPosition = new Point(Position.X * tileSize, Position.Y * tileSize);
                }

                if (Road != null && Road.Count > 0)
                {
                    Point p = Road.Peek();

                    int deltaScreenX = p.X * tileSize - ScreenPosition.X;
                    int deltaScreenY = p.Y * tileSize - ScreenPosition.Y;

                    int dx = deltaScreenX > 0 ? speedMultiplier : deltaScreenX < 0 ? -speedMultiplier : 0;
                    int dy = deltaScreenY > 0 ? speedMultiplier : deltaScreenY < 0 ? -speedMultiplier : 0;

                    ScreenPosition = new Point(ScreenPosition.X + dx,
                                               ScreenPosition.Y + dy);

                    if (ScreenPosition.X == p.X *  tileSize &&
                        ScreenPosition.Y == p.Y * tileSize)
                    {
                        Position = p;
                        Road.Dequeue();
                    }
                }
                else
                {
                    OnArrived(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Az ember állapotának változása
        /// </summary>
        public void PersonState(int personstate)
        {
            switch (personstate)
            {
                case 0:
                    _state = PersonCondition.Ply;
                    break;
                case 1:
                    _state = PersonCondition.Use;
                    break;
                case 2:
                    _state = PersonCondition.Waiting;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Ember elindulás esemény
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStart(object sender, EventArgs e)
        {
            Start?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Ember megérkezés esemény
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnArrived(object sender, EventArgs e)
        {
            Arrived?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}