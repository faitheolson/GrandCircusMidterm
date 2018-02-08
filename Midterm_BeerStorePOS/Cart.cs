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
        public int BeerQty { set; get; }
        public decimal Subtotal { set; get; }

        //constructor
        public Cart(string name, string style, string description, string price, int qty, int sub) : base(name, style, description, price)
        {
            BeerQty = qty;
            Subtotal = qty * decimal.Parse(price);
        }
    }
}
