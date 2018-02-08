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
                if (UserInput == "2")
                {
                    DisplayCart(CartItems);
                }
                if (UserInput == "3")
                {
                    repeat = false;
                    Console.WriteLine("GoodBye!");
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
                double QuantityInput = double.Parse(Validation.validateQauntity(Console.ReadLine()));// keep this
                double LineSubtotal = QuantityInput * double.Parse(BeerSelection[ItemInput].BeerPrice);

                CartItems.Add(new Cart(BeerSelection[ItemInput].BeerName, BeerSelection[ItemInput].BeerStyle,
                BeerSelection[ItemInput].BeerDescription, BeerSelection[ItemInput].BeerPrice, QuantityInput, LineSubtotal));

                Console.WriteLine($"You've added {BeerSelection[ItemInput].BeerName}. The price is {BeerSelection[ItemInput].BeerPrice}. Line Subtotal: {LineSubtotal}");
                Console.WriteLine("Would you like to add another item? (Y/N)");
                string UserIntput = Validation.validateAddItem(Console.ReadLine().ToUpper()); // keep this 
                //while (!Regex.IsMatch(UserIntput,@"^(Y|N)$"))
                //{
                //    Console.WriteLine("Please enter a vaild answer!"); Remove this stuff 
                //    UserIntput = Console.ReadLine().ToUpper();
                //}
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
            Console.WriteLine($"Your order grand total is: {CalculateCartTotal(CartItems) * 1.06}");
            Console.WriteLine();
            Console.WriteLine("Would you like to return to the [1]main menu or [2]check out & pay?");
            string UserInput = Console.ReadLine();
            if (UserInput == "2")
            {
                //Go to check out & pay
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
    }
}
