using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaWestWorld.Core
{
    public class Location
    {
        public static readonly Location GoldMine = new Location("Gold Mine");
        public static readonly Location Saloon = new Location("Saloon");
        public static readonly Location Bank = new Location("Bank");

        public Location(string name)
        {
            Name = name;
        }

        public string Name { get; }        
    }
}
