using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaWestWorld.Core
{
    public class Miner : ReceiveActor
    {
        public class Tick { }
        public class Exit {
            public Action Action { get; }

            public Exit(Action action)
            {
                this.Action = action;
            }
        }

        private readonly IStatusLogger _statusLogger;

        const int PricePerUnitGold = 2;

        public int Id { get;  }
        public string Name { get; }
        public Location Location { get; set; }
        public int MoneyInBank { get; set; }
        public int Thirst { get; set; }
        public int Fatigue { get; set; }
        public int GoldCarried { get; private set; } = 0;

        public Miner(int id, string name, IStatusLogger statusLogger)
        {
            this.Id = id;
            this.Name = name;
            this._statusLogger = statusLogger;
            Become(EnterMineAndDigForNugget);            
        }

        public void AddToGoldCarried(int amount)
        {
            GoldCarried += amount;
        }

        public void IncreaseFatigue() {
            Fatigue++;
            Thirst++;
        }

        public bool IsPocketFull { get => GoldCarried > 5; } 

        public bool Thirsty { get => Thirst > 10; } 

        private void DepositGold() {
            MoneyInBank += GoldCarried * PricePerUnitGold;
            GoldCarried = 0;
        }

        private void EnterMineAndDigForNugget() {

            if (Location != Location.GoldMine)
            {
                _statusLogger.Log($"{Name}: Walkin' to the gold mine");
                Location = Location.GoldMine;
            }

            Receive<Tick>(t => {
                AddToGoldCarried(1);

                IncreaseFatigue();
                
                _statusLogger.Log($"{Name}: Pickin' up a nugget");

                if (IsPocketFull)
                {
                    Self.Tell(new Exit(VisitBankAndDepositGold));
                }

                if (Thirsty)
                {
                    Self.Tell(new Exit(QuenchThirst));
                }
            });
            Receive<Exit>(ev => {
                _statusLogger.Log($"{Name}: Ah'm leavin' the gold mine with mah pockets full o' sweet gold");
                Become(ev.Action);
            });
        }

        private void QuenchThirst() {
            _statusLogger.Log($"{Name}: Boy, ah sure is thusty! Walkin' to the saloon");
            
            Receive<Tick>(ev => {
                Location = Location.Saloon;
                _statusLogger.Log($"{Name}: That's mighty fine sippin' liquor");                
                Thirst = 0;
                Self.Tell(new Exit(EnterMineAndDigForNugget));
            });
            Receive<Exit>(ev => {
                _statusLogger.Log($"{Name}: Leavin' the saloon, feelin' good");
                Become(ev.Action);
            });
        }

        private void VisitBankAndDepositGold() {
            _statusLogger.Log($"{Name}: Goin' to the bank. Yes siree");
            
            Receive<Tick>(ev => {
                Location = Location.Bank;

                DepositGold();

                _statusLogger.Log($"{Name}: Depositin' gold. Total savings now: {MoneyInBank}");
                Self.Tell(new Exit(EnterMineAndDigForNugget));
            });
            Receive<Exit>(ev => {
                _statusLogger.Log($"{Name}: Leavin' the bank");
                Become(ev.Action);
            });
        }
    }
}
