using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace Midterm_BeerStorePOS
{
    class POSApp
    {
        public ConsoleKey UserInput { get; set; }

        public void StartApp()
        {
            bool repeat = true;

            while (repeat)
            {
                List<Beer> BeerSelection = CreateList();
                List<Cart> CartItems = new List<Cart>();
                PrintMenu();
                UserInput = Validation.ValidateSelection("Please enter your selection by number:"); //changed from string to console key
                if (UserInput == ConsoleKey.D1 || UserInput == ConsoleKey.NumPad1)
                {
                    Console.Clear();
                    DisplaySelection(BeerSelection);
                    AddToCart(BeerSelection, CartItems);
                }
                else if (UserInput == ConsoleKey.D2|| UserInput == ConsoleKey.NumPad2)
                {
                    DisplayCart(CartItems);
                }
                else if (UserInput == ConsoleKey.D3 || UserInput == ConsoleKey.NumPad3)
                {
                    Console.Clear();
                    RunInventoryManager("../../ProductList.txt", BeerSelection);
                    Console.Clear();
                }
                else
                {
                    repeat = false;
                    Console.Clear();
                    Console.WriteLine("Goodbye!");
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

        private List<Beer> CreateList()
        {
            List<Beer> BeerSelection = new List<Beer>();

            foreach (string b in Beer.GetBeer("../../ProductList.txt"))
            {
                string[] temp = b.Split(',');
                BeerSelection.Add(new Beer(temp[0], temp[1], temp[2], temp[3]));
            }
            return BeerSelection;
        }

        private void DisplaySelection(List<Beer> BeerSelection)
        {
            Console.WriteLine($"{"Beer",-20}{"Style",-10}{"Package",-20}{"Price",-10}");
            //continue header format

            for (int i = 0; i < BeerSelection.Count; i++)
            {
                Console.WriteLine($"{i + 1,-4}{BeerSelection[i].BeerName,-20}{BeerSelection[i].BeerStyle,-10}{BeerSelection[i].BeerDescription,-20}" +
                    $"{BeerSelection[i].BeerPrice,-10}");
            }
            
        }

        private void AddToCart(List<Beer> BeerSelection, List<Cart> CartItems)
        {
            bool repeat = true;
            while (repeat)
            {
                //user chooses item
                Console.Write($"Please select item to add to cart <1 - {BeerSelection.Count}>: ");
                string ItemInput = Console.ReadLine();
                while (Validation.ValidateItemChoice(ItemInput, BeerSelection) == false)
                {
                    Console.Write($"Invalid choice! Please select item to add to cart <1 - {BeerSelection.Count}>: ");
                    ItemInput = Console.ReadLine();
                }
                int ItemNumber = int.Parse(ItemInput) - 1;

                //get quantity
                Console.Write("Please enter quantity: ");
                double QuantityInput = double.Parse(Validation.ValidateQuantity(Console.ReadLine()));

                //calc subtotal
                double LineSubtotal = QuantityInput * double.Parse(BeerSelection[ItemNumber].BeerPrice);

                //add item selected to cart
                CartItems.Add(new Cart(BeerSelection[ItemNumber].BeerName, BeerSelection[ItemNumber].BeerStyle,
                BeerSelection[ItemNumber].BeerDescription, BeerSelection[ItemNumber].BeerPrice, QuantityInput, LineSubtotal));

                Console.WriteLine($"ADDED TO CART: {BeerSelection[ItemNumber].BeerName}\tPRICE{BeerSelection[ItemNumber].BeerPrice}\tQTY: {QuantityInput}" +
                    $"\tITEM SUBTOTAL: {LineSubtotal}");

                Console.WriteLine();

                //continue or not
                UserInput = Validation.CheckYorN("Would you like to add another item? <Y or N>");

                if (UserInput == ConsoleKey.N)
                {
                    repeat = false;
                }
                else
                {
                    Console.Clear();
                    DisplaySelection(BeerSelection);
                }
            }
        }

        private void DisplayCart(List<Cart> CartItems)
        {
            //print items added to cart
            Console.Clear();
            Console.WriteLine($"{"ITEM", -20}{"PRICE", -10}{"QTY",-5}{"SUBTOTAL",-10}");

            foreach (Cart item in CartItems)
            {
                Console.WriteLine($"{item.BeerName, -20}\t{item.BeerPrice,-10}\t{item.BeerQty,-5}\t{item.Subtotal,-10}");
            }
            Console.WriteLine();

            //calculate and display costs
            double Subtotal = CalculateCartTotal(CartItems);
            double TaxOwed = (Subtotal * 1.06) - Subtotal;
            double GrandTotal = Subtotal + TaxOwed;

            Console.WriteLine($"SUBTOTAL:\t{Subtotal}");
            Console.WriteLine($"TAX:\t\t{TaxOwed}");
            Console.WriteLine($"ORDER TOTAL:\t{GrandTotal}");

            Console.WriteLine();

            Console.WriteLine("[1]Main Menu\n[2]Check Out\n[3]Empty Cart");

            int ViewCart = int.Parse(Console.ReadLine());

            if (ViewCart == 1)
            {
                PrintMenu();
            }
            else if (ViewCart == 2 && GrandTotal > 0)
            {
                GoToCheckout(GrandTotal, CartItems);
            }
            else if (ViewCart == 2 && GrandTotal == 0)
            {
                Console.Clear();
                Console.WriteLine("You don't have anything in your cart! Going back to main menu.");
                System.Threading.Thread.Sleep(1000);
                PrintMenu();
            }
            else
            {
                Console.WriteLine("Are you sure you want to empty your shopping cart? (<Y> or <N>)");
                ConsoleKey AreYouSure = Console.ReadKey().Key;

                if (AreYouSure == ConsoleKey.Y)
                {
                    EmptyCart(CartItems);
                    DisplayCart(CartItems);
                }
                else
                {
                    Console.Clear();
                    DisplayCart(CartItems);
                }
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
            Console.WriteLine("Please select the number of the payment type:");
            PrintCheckoutMenu();
            int ChoosePaymentMethod = int.Parse(Console.ReadLine());

            if (ChoosePaymentMethod == 1)
            {
                CashPayment(total);
                EmptyCart(CartItems);
                Console.Read();
            }
            else if (ChoosePaymentMethod == 2)
            {
                CreditPayment(total);
                Console.Read();
            }
            else if (ChoosePaymentMethod == 3)
            {
                CheckPayment(total);
                EmptyCart(CartItems);
                Console.Read();
            }
            else
            {
                Console.Clear();
                PrintMenu();
            }
        }

        public void CashPayment(double total)
        {
            Console.WriteLine("How much money have you received?");
            double dollars = double.Parse( Validation.ValidateMoneyRecieved( Console.ReadLine())); // validate this input
            double change = 0;

            if (dollars >= total)
            {
                change = dollars - total;
                Console.WriteLine($"Thank you for your payment of {dollars.ToString(("C2"))}. Your change is {change.ToString(("C2"))}");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Insufficient funds!");
            }
        }

        public void CreditPayment(double total)
        {
            Console.WriteLine("Please enter the card information:");
            string CardNumber = Validation.ValidateCredNumber( Console.ReadLine()); //prompt
            Console.WriteLine("Please enter CCV");
            string CCV = Validation.ValidateCCV (Console.ReadLine()); //prompt
            Console.WriteLine("Please enter the Expiration date on your card:");
            string Expiration = Validation.ValidateExpDate (Console.ReadLine()); //prompt, ensure format is acceptable (regex?)
            string lastfour = CardNumber.Substring(CardNumber.Length - 4);

            Console.WriteLine($"Thank you! {total} has been charged to card ending in {lastfour}.");
        }

        public void CheckPayment(double total)
        {
            Console.WriteLine("Please enter the check number:"); //validate
            string CheckNumber = Validation.ValidateCheck(Console.ReadLine());

            Console.WriteLine($"Thank you! Check {CheckNumber} has been received for {total}.");
        }

        public void EmptyCart(List<Cart> CartItems)
        {
            for (int i = 0; i <= CartItems.Count - 1; i = 0)
            {
                CartItems.Remove(CartItems[i]);
            }
        }

        public void AddNewBeer() //Put this in Add Beer to Inventory Option
        {
            Beer.AppendBeerList("../../ProductList.txt", Beer.NewBeerString());
            List<Beer> BeerSelection = CreateList();
        }

        public void RunInventoryManager(string FileName, List<Beer> BeerSelection)
        {
            Console.WriteLine($"[1] ADD NEW BEER TO INVENTORY\n[2] REMOVE BEER FROM INVENTORY\n[3] RETURN TO MAIN MENU");
            string UserInput = Console.ReadLine();
            while (!Regex.IsMatch(UserInput,@"^1|2|3$"))
            {
                Console.WriteLine("Please enter valid entry!");
            }
            if (UserInput == "1")
            {
                AddNewBeer();
            }
            else if (UserInput == "2")
            {
                Console.Clear();
                DisplaySelection(BeerSelection);
                Beer.RemoveBeer(FileName, BeerSelection);
            }
            else if (UserInput == "3")
            {

            }
        }
    }
}
