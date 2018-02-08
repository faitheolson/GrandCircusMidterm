using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Midterm_BeerStorePOS
{
    class POSApp
    {
        public void StartApp()
        {
            bool repeat = true;
            List<Cart> CartItems = new List<Cart>();
            while (repeat)
            {
                PrintMenu();
                Console.WriteLine("Please enter your selection:");
                string UserInput = Console.ReadLine();
                if (UserInput == "1")
                {
                    Console.Clear();
                    List<Beer> BeerSelection = DisplaySelection();
                    AddToCart(BeerSelection, CartItems);
                }
                else if (UserInput == "2")
                {
                    DisplayCart(CartItems);
                }
                else if (UserInput == "3")
                {
                    repeat = false;
                    Console.WriteLine("GoodBye!");
                }
                else
                {
                    Console.WriteLine("Please enter a valid choice!");
                }
            }
        }

        private void PrintMenu()
        {
            Console.Clear();
            IEnumerable<Menu> ListOfOptions = Enum.GetValues(typeof(Menu)).Cast<Menu>();

            foreach (Menu choice in ListOfOptions)
            {
                Console.WriteLine($"{(int)choice}. {choice}");
            }
        }

        private List<Beer> DisplaySelection()
        {
            List<Beer> BeerSelection = new List<Beer>();

            foreach (string b in Beer.GetBeer("../../ProductList.txt"))
            {
                string[] temp = b.Split(',');
                BeerSelection.Add(new Beer(temp[0], temp[1], temp[2], temp[3]));
            }

            Console.WriteLine($"{"Beer",-20}{"Style",-10}{"Package",-20}{"Price",-10}");
            //continue header format

            for (int i = 0; i < BeerSelection.Count; i++)
            {
                Console.WriteLine($"{i + 1,-4}{BeerSelection[i].BeerName,-20}{BeerSelection[i].BeerStyle,-10}{BeerSelection[i].BeerDescription,-20}" +
                    $"{BeerSelection[i].BeerPrice,-10}");
            }
            return BeerSelection;
        }

        private void AddToCart(List<Beer> BeerSelection, List<Cart> CartItems)
        {
            bool repeat = true;
            while (repeat)
            {
                Console.WriteLine("Please select item to add to cart:");
                int ItemInput = int.Parse(Console.ReadLine()) - 1;
                Console.WriteLine("Please select quantity:");
                double QuantityInput = double.Parse(Validation.validateQauntity(Console.ReadLine()));
                double LineSubtotal = QuantityInput * double.Parse(BeerSelection[ItemInput].BeerPrice);

                CartItems.Add(new Cart(BeerSelection[ItemInput].BeerName, BeerSelection[ItemInput].BeerStyle,
                BeerSelection[ItemInput].BeerDescription, BeerSelection[ItemInput].BeerPrice, QuantityInput, LineSubtotal));

                Console.WriteLine($"You've added {BeerSelection[ItemInput].BeerName}. The price is {BeerSelection[ItemInput].BeerPrice}. Line Subtotal: {LineSubtotal}");
                Console.WriteLine("Would you like to add another item? (Y/N)");

                string UserIntput = Validation.validateAddItem(Console.ReadLine().ToUpper());

                UserIntput = Console.ReadLine().ToUpper();
                while (!Regex.IsMatch(UserIntput, @"^(Y|N)$"))
                {
                    Console.WriteLine("Please enter a vaild answer!");
                    UserIntput = Console.ReadLine().ToUpper();
                }

                if (UserIntput == "N")
                {
                    repeat = false;
                }
                else
                {
                    Console.Clear();
                    DisplaySelection();
                }
            }
        }

        private void DisplayCart(List<Cart> CartItems)
        {
            Console.Clear();
            Console.WriteLine("Your shopping cart contains the following products:");
            foreach (Cart item in CartItems)
            {
                Console.WriteLine($"{item.BeerName,-25}{item.BeerPrice,-5}{item.BeerQty,-5}{item.Subtotal,-5}");
            }
            Console.WriteLine();
            Console.WriteLine($"Your shopping cart total is: {CalculateCartTotal(CartItems)}");
            Console.WriteLine($"Your shopping cart tax is: {CalculateCartTotal(CartItems) * (double).06}");

            double GrandTotal = CalculateCartTotal(CartItems) * 1.06;
            Console.WriteLine($"Your order grand total is: {GrandTotal}");

            Console.WriteLine();

            Console.WriteLine("Would you like to return to the [1]main menu or [2]check out & pay?");

            int ViewCart = int.Parse(Console.ReadLine());

            if (ViewCart == 2)

            {
                GoToCheckout(GrandTotal, CartItems);

            }
        }

        public double CalculateCartTotal(List<Cart> CartItems)
        {
            double CartTotal = 0;
            foreach (Cart item in CartItems)
            {
                CartTotal += item.Subtotal;
            }
            return CartTotal;
        }

        private void PrintCheckoutMenu()
        {
            IEnumerable<PaymentChoices> ListOfPaymentTypes = Enum.GetValues(typeof(PaymentChoices)).Cast<PaymentChoices>();

            foreach (PaymentChoices p in ListOfPaymentTypes)
            {
                Console.WriteLine($"{(int)p}. {p}");
            }
        }

        private void GoToCheckout(double total, List<Cart> CartItems)
        {
            Console.WriteLine("How would you like to pay [please enter number of selection]");
            PrintCheckoutMenu();
            int ChoosePaymentMethod = int.Parse(Console.ReadLine());

            if (ChoosePaymentMethod == 1)
            {
                CashPayment(total);
                EmptyCart(CartItems);
                Console.Read();
            }
            else if(ChoosePaymentMethod == 2)
            {
                CreditPayment(total);
                Console.Read();
            }
            else if(ChoosePaymentMethod == 3)
            {
                CheckPayment(total);
                Console.Read();
            }
            else
            {
                Console.WriteLine("WRONG!");
            }
        }

        public void CashPayment(double total)
        {
            Console.WriteLine("How much money have you received?");
            double dollars = double.Parse(Console.ReadLine()); // validate this input
            double change = 0;

            if (dollars >= total)
            {
                change = dollars - total;
                Console.WriteLine($"Thank you for your payment of {dollars}. Your change is {change}");
            }
            else
            {
                Console.WriteLine("Insufficient funds!");
            }
        }

        public void CreditPayment(double total)
        {
            Console.WriteLine("Please enter the card information:");
            string CardNumber = Console.ReadLine(); //prompt
            string CCV = Console.ReadLine(); //prompt
            string Expiration = Console.ReadLine(); //prompt, ensure format is acceptable (regex?)
            string lastfour = CardNumber.Substring(CardNumber.Length - 4);

            Console.WriteLine($"Thank you! {total} has been charged to card ending in {lastfour}.");
        }

        public void CheckPayment(double total)
        {
            Console.WriteLine("Please enter the check number:"); //validate
            string CheckNumber = Console.ReadLine();

            Console.WriteLine($"Thank you! Check {CheckNumber} has been received for {total}.");
        }

        public void EmptyCart(List<Cart> CartItems)
        {
            for(int i = 0;  i<=CartItems.Count - 1; i = 0)
            {
                CartItems.Remove(CartItems[i]);
            }
        }
    }
}
