using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DBR.Model
{
    /// <summary>
    /// A vendéglátóhely osztály
    /// </summary>
    public class Canteen : Building
    {
        #region Fields

        private int _servingTime;

        #endregion

        #region Properties

        public Dictionary<Visitor, int> ClientsServed { get; set; }

        public int ServingDuration { get; set; }

        public int ServingTime { get { return _servingTime; } set { _servingTime = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// A vendéglátóhely osztály konstruktora, inicializálja az adattagokat
        /// </summary>
        /// <param name="x">Az vendéglátóhely balfelső sarkának x koordinátája</param>
        /// <param name="y">Az vendéglátóhely balfelső sarkának y koordinátája</param>
        /// <param name="buildPrice">Az vendéglátóhely megépítésének ára</param>
        /// <param name="buildTime">Az vendéglátóhely megépítésének ideje másodpercben</param>
        /// <param name="length">Az vendéglátóhely hossza (az y tengelyen hány egységnyi)</param>
        /// <param name="width">Az vendéglátóhely szélesssége (az x tengelyen hány egységnyi)</param>
        /// <param name="fee">Az üzemeltetés díja amit a vendégek fizetnek</param>
        /// <param name="maintenance">Az vendéglátóhely fenntartásának díja amit a játékos fizet</param>
        /// <param name="capacity">Az vendéglátóhely vendéglátói kapacítása</param>
        /// <param name="name">Az vendéglátóhely neve</param>
        public Canteen(int x, int y, int buildPrice, int buildTime, int length, int width, int fee, int maintenance, int upkeep, int capacity, int servingDuration, string name, int effect)
            : base(x, y, buildPrice, buildTime, length, width, fee, maintenance, upkeep, capacity, name, effect)
        {
            ServingDuration = servingDuration;
            ClientsServed = new Dictionary<Visitor, int>();
        }

        /// <summary>
        /// A vendéglátóhely osztály copy konstruktora
        /// </summary>
        /// <param name="canteeen">A vendéglátóhely egy esete</param>
        public Canteen(Canteen canteeen)
            : this(canteeen._posX, canteeen._posY, canteeen.BuildPrice, canteeen.BuildTime, canteeen.Length, canteeen.Width, canteeen.Fee, canteeen.Maintenance, canteeen.Upkeep, canteeen._capacity, canteeen.ServingDuration, canteeen.Name, canteeen.Effect) { }


        /// <summary>
        /// Idő léptetése
        /// </summary>
        public void TimeAdvanced()
        {
            switch (State)
            {
                case BuildingCondition.Functioning:
                    Function();
                    break;
                case BuildingCondition.Waiting:
                    ServeClients();
                    break;
                case BuildingCondition.Broken:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Emberek kiszolgálásának megkezdése
        /// </summary>
        private void ServeClients()
        {
            if (WaitingVisitors.Count > 0 && ClientsServed.Count < Capacity)
            {
                UpkeepPay();

                if (CanFunction)
                {
                    int visitorNumber = Math.Min(WaitingVisitors.Count, _capacity);

                    for (int i = 0; i < visitorNumber; i++)
                    {
                        Visitor client = WaitingVisitors.Dequeue();
                        client.State = Person.PersonCondition.Use;
                        client.PayFee(Fee);
                        client.Start -= GetOutOfRow;
                        if (!ClientsServed.Keys.Contains(client))
                            ClientsServed.Add(client, ServingDuration);
                        else
                        {
                            client.State = Person.PersonCondition.Ply;
                        }
                    }
                    State = BuildingCondition.Functioning;
                }
            }
        }

        /// <summary>
        /// Emberek kiszolgálása
        /// </summary>
        private void Function()
        {
            for (int i = 0; i < ClientsServed.Count; i++)
            {
                var visitor = ClientsServed.Keys.ToArray()[i];
                visitor.Eating(Effect);
                ClientsServed[visitor] -= 1;

                if (ClientsServed[visitor] <= 0)
                    visitor.State = Person.PersonCondition.Ply;
            }

            var tmp = from p in ClientsServed
                      where p.Key.State == Person.PersonCondition.Ply
                      select p.Key;

            foreach (var p in tmp)
            {
                ClientsServed.Remove(p);
            }

            if (ClientsServed.Count < Capacity)
            {
                ServeClients();
            }
            else if (ClientsServed.Count == 0)
            {
                State = BuildingCondition.Waiting;
            }
        }

        #endregion
    }
}
