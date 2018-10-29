using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestCoinService.Model
{
    public class Bid
    {
        public Bid(int id, string item, double price, string name)
        {
            Id = id;
            Item = item;
            Price = price;
            Name = name;
        }

        public int Id { get; set; }
        public string Item { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }

        public Bid()
        {

        }

        public override string ToString()
        {
            return Id + " " + Item + " " + Price + " " + Name;
        }
    }
}
