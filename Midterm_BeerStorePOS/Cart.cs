using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midterm_BeerStorePOS
{
    class Cart:Beer
    {
        //properties
        public double BeerQty { set; get; }
        public double Subtotal { set; get; }

        //constructor
        public Cart(string name, string style, string description, string price, double qty, double sub) : base(name, style, description, price)
        {
            BeerQty = qty;
            Subtotal = sub;
        }
    }
}
