using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DBR.Model
{
    /// <summary>
    /// A látogató osztálya
    /// </summary>
    public class Visitor : Person
    {
        #region Fields

        /// <summary>
        /// A látogató pénze
        /// </summary>
        private int _money;

        /// <summary>
        /// A látogató hangulata
        /// </summary>
        private int _mood;

        /// <summary>
        /// A látogató jóllakottsága 
        /// </summary>
        private int _satiety;

        /// <summary>
        /// A látogató türelmi ideje 
        /// </summary>
        private int _patienceTime;

        #endregion

        #region Properties

        /// <summary>
        /// A látogató pénzének lekérdezése
        /// </summary>
        public int Money { get { return _money; } }

        /// <summary>
        /// A látogató hangulatának lekérdezése
        /// </summary>
        public int Mood { get { return _mood; } }

        /// <summary>
        /// A látogató jóllakottságának lekérdezése
        /// </summary>
        public int Satiety { get { return _satiety; } }

        public int PatienceTime { get { return _patienceTime; } set { _patienceTime = value; } }

        public bool WentHome { get; set; }

        #endregion

        #region Events

        public event EventHandler<int> PaidFee;

        public event EventHandler HasBadMood;

        public event EventHandler IsHungry;

        #endregion

        #region Constructor

        /// <summary>
        /// A látogató osztály konstruktora, inicializálja az adatagokat
        /// </summary>
        /// <param name="x">A látogató x koordinátája</param>
        /// <param name="y">A látogató y koordinátája</param>
        /// <param name="money">A látogató pénze</param>
        /// <param name="mood">A látogató hangulata</param>
        /// <param name="satiety">A látogató jóllakottsága</param>
        public Visitor(int x, int y, int money, int mood, int satiety, int maxid)
            : base(x, y, maxid)
        {
            _money = money;
            _mood = mood;
            _satiety = satiety;
            _patienceTime = 10;

            WentHome = false;
        }

        /// <summary>
        /// A látogató osztály copy konstruktora
        /// </summary>
        /// <param name="visitor">A látogató osztály egy esete</param>
        public Visitor(Visitor visitor)
            : this(visitor._posX, visitor._posY, visitor.Money, visitor.Mood, visitor.Satiety, visitor.SpriteID) { }

        #endregion

        #region Methods

        /// <summary>
        /// A látogató jóllakottságának növekedése evéskor
        /// </summary>
        public void Eating(int effect)
        {
            _satiety += _satiety + effect > 100 ? 100 - _satiety : effect;
        }

        /// <summary>
        /// A látogató hangulatának növekedése játszáskor
        /// </summary>
        public void Playing(int effect)
        {
            _mood += _mood + effect > 100 ? 100 - _mood : effect;
        }

        /// <summary>
        /// A látogató hangulatának növekedése növény közelépen
        /// </summary>
        public void PlantEffect(int effect)
        {
            Playing(effect);
        }

        /// <summary>
        /// A látogató hangulatának és jóllakottságának változásai
        /// </summary>
        public void MoodChanges()
        {
            _mood -= _mood - 1 == 0 ? 0 : 1;
            _satiety -= _satiety - 1 == 0 ? 0 : 1;

            if (_state == PersonCondition.Waiting)
            {
                if (_patienceTime == 0)
                {
                    _mood -= _mood - 1 == 0 ? 0 : 1;
                    if (_mood <= 0)
                    {
                        OnHavingBadMood();
                    }

                    if (_satiety < 20)
                    {
                        _mood -= _mood - 1 == 0 ? 0 : 1;
                        OnBeingHungry();
                    }
                }

                else if (_patienceTime > 0)
                    _patienceTime--;
            }
            else
            {
                if (_satiety < 20)
                {
                    _mood -= _mood - 1 == 0 ? 0 : 1;
                }
                _patienceTime = 10;
            }
        }


        public bool PayFee(int fee)
        {
            if (_money < fee)
                return false;

            _money -= fee;

            OnPaidFee(fee);

            return true;
        }

        private void OnPaidFee(int fee)
        {
            PaidFee?.Invoke(this, fee);
        }

        private void OnHavingBadMood()
        {
            HasBadMood?.Invoke(this, EventArgs.Empty);
        }

        private void OnBeingHungry()
        {
            IsHungry?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
