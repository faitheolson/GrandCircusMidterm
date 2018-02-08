using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midterm_BeerStorePOS
{
    class Payment
    {
        public double DollarAmount { set; get; }
        public double GrandTotal { set; get; }
        public string CardNumber { set; get; }
        public string CCV { set; get; }
        public string CardExp { set; get; }
        public string CheckNumber { set; get; }

        #region METHODS

        public void CashPayment (double dollars, double total)
        {
            GrandTotal = total;
            DollarAmount = dollars;
            double change = 0;

            if(DollarAmount >= GrandTotal)
            {
                 change = DollarAmount - GrandTotal;
            }
            else
            {
                Console.WriteLine("Insufficient funds!");
            }
        }

        //credit payment
        public void CreditPayment(double total, string accountnumber, string securitycode, string expirationdate)
        {
            GrandTotal = total;
            CardNumber = accountnumber;
            CCV = securitycode;
            CardExp = expirationdate;

            string lastfour = CardNumber.Substring(CardNumber.Length - 4);

            Console.WriteLine($"Thank you! {GrandTotal} has been charged to card ending in {lastfour}.");
        }

        //check payment
        public void CheckPayment(double total, string whowritesacheckthesedays)
        {
            GrandTotal = total;
            CheckNumber = whowritesacheckthesedays;

            Console.WriteLine($"Thank you! Check {CheckNumber} has been received for {GrandTotal}.");
        }
        #endregion

    }
}
