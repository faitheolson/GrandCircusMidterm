using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Midterm_BeerStorePOS
{
    class POSApp
    {
        public ConsoleKey UserInput { get; set; }

        public void StartApp()
        {
            bool repeat = true;
            List<Cart> CartItems = new List<Cart>();

            while (repeat)
            {
                List<Beer> BeerSelection = CreateList();

                PrintMenu();
                UserInput = Validation.ValidateSelection("Please enter your selection by number:");
                if (UserInput == ConsoleKey.D1 || UserInput == ConsoleKey.NumPad1)
                {
                    Console.Clear();
                    DisplaySelection(BeerSelection);
                    AddToCart(BeerSelection, CartItems);
                }
                else if (UserInput == ConsoleKey.D2 || UserInput == ConsoleKey.NumPad2)
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
                    if (CartItems.Count > 0)
                    {
                        ConsoleKey WantToExit = Validation.CheckYorN("There are items in the cart. Are you sure you want to exit? <Y or N?>");

                        if (WantToExit == ConsoleKey.Y)
                        {
                            repeat = false;
                            Console.Clear();
                            Console.WriteLine("Goodbye!");
                        }
                        else
                        {
                            Console.Clear();
                            PrintMenu();
                        }
                    }
                    else
                    {
                        repeat = false;
                        Console.Clear();
                        Console.WriteLine("Goodbye!");
                    }
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
            Console.WriteLine($"{"BEER",-25}{"STYLE",-10}{"PACKAGE",-20}{"PRICE",-10}");
            Console.WriteLine(new string('-', 75));

            for (int i = 0; i < BeerSelection.Count; i++)
            {
                Console.WriteLine($"{i + 1,-4}{BeerSelection[i].BeerName,-20}{BeerSelection[i].BeerStyle,-10}{BeerSelection[i].BeerDescription,-20}" +
                    $"{BeerSelection[i].BeerPrice,-10}");
            }
            Console.WriteLine();
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
                Console.WriteLine();
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
            Console.WriteLine($"{"ITEM",-25}{"PRICE",-15}{"QTY",-15}{"SUBTOTAL",-10}");
            Console.WriteLine(new string('-', 75));

            foreach (Cart item in CartItems)
            {
                Console.WriteLine($"{item.BeerName,-20}\t{item.BeerPrice,-10}\t{item.BeerQty,-10}\t{item.Subtotal,-10}");
            }
            Console.WriteLine();

            //calculate and display costs
            double Subtotal = CalculateCartTotal(CartItems);
            double TaxOwed = (Subtotal * 1.06) - Subtotal;
            double GrandTotal = Subtotal + TaxOwed;

            Console.WriteLine($"SUBTOTAL:\t{Subtotal.ToString("C2")}");
            Console.WriteLine($"TAX:\t\t{TaxOwed.ToString("C2")}");
            Console.WriteLine($"ORDER TOTAL:\t{GrandTotal.ToString("C2")}");

            Console.WriteLine();
            Console.WriteLine("[1]Main Menu\n[2]Check Out\n[3]Empty Cart");

            ConsoleKey CartAction = Validation.ValidateCartAction("Please choose option [1][2] or [3]: ");

            if (CartAction == ConsoleKey.D1 || CartAction == ConsoleKey.NumPad1)
            {
                PrintMenu();
            }
            else if ((CartAction == ConsoleKey.D2 || CartAction == ConsoleKey.NumPad2) && GrandTotal > 0)
            {
                Console.WriteLine();
                GoToCheckout(GrandTotal, CartItems);
            }
            else if ((CartAction == ConsoleKey.D2 || CartAction == ConsoleKey.NumPad2) && GrandTotal == 0)
            {
                Console.Clear();
                Console.WriteLine("You don't have anything in your cart! Going back to main menu.");
                System.Threading.Thread.Sleep(1000);
                PrintMenu();
            }
            else
            {
                ConsoleKey AreYouSure = Validation.CheckYorN("Are you sure you want to empty your shopping cart? (<Y> or <N>)");

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
            Console.WriteLine();
            if (CheckDOB() < 21)
            {
                EmptyCart(CartItems);
                Console.WriteLine("Not old enough to complete purchase! Order cancelled.");
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {
                Console.WriteLine();
                PrintCheckoutMenu();

                ConsoleKey ChoosePaymentMethod = Validation.ValidatePaymentType("Please choose option [1][2][3] or [4]: ");

                if (ChoosePaymentMethod == ConsoleKey.D1 || ChoosePaymentMethod == ConsoleKey.NumPad1)
                {
                    CashPayment(total, CartItems);
                }
                else if (ChoosePaymentMethod == ConsoleKey.D2 || ChoosePaymentMethod == ConsoleKey.NumPad2)
                {
                    CreditPayment(total, CartItems);
                    Console.Read();
                }
                else if (ChoosePaymentMethod == ConsoleKey.D3 || ChoosePaymentMethod == ConsoleKey.NumPad3)
                {
                    CheckPayment(total, CartItems);
                }
                else
                {
                    Console.Clear();
                    PrintMenu();
                }
            }
        }

        public void CashPayment(double total, List<Cart> CartItems)
        {
            Console.WriteLine();
            Console.WriteLine("ENTER CASH TENDERED:");
            double dollars = double.Parse(Validation.ValidateMoneyRecieved(Console.ReadLine()));
            double change = 0;

            while (dollars < total)
            {
                Console.Write("INSUFFICIENT! ENTER CASH TENDERED:");
                dollars = double.Parse(Validation.ValidateMoneyRecieved(Console.ReadLine()));
            }
            change = dollars - total;


            PrintReceipt(CartItems);
            Console.WriteLine($"CASH TENDERED:\t{dollars.ToString(("C2"))}\nCHANGE:\t\t{change.ToString(("C2"))}");
            EmptyCart(CartItems);
            Console.WriteLine("\n\n\n\n<PRESS ANY KEY TO RETURN TO MAIN MENU");
            Console.Read();
        }

        public void CreditPayment(double total, List<Cart> CartItems)
        {
            Console.WriteLine();
            Console.WriteLine("ENTER CARD NUMBER <XXXX-XXXX-XXXX-XXXX, including dashes>:");
            string CardNumber = Validation.ValidateCredNumber(Console.ReadLine()); //prompt
            Console.Write("ENTER CCV: ");
            string CCV = Validation.ValidateCCV(Console.ReadLine()); //prompt
            Console.Write("ENTER EXPIRATION DATE <MM/YY> OR <MM/YYYY>: ");
            string Expiration = Validation.ValidateExpDate(Console.ReadLine()); //prompt, ensure format is acceptable (regex?)
            string lastfour = CardNumber.Substring(CardNumber.Length - 4);

            PrintReceipt(CartItems);
            Console.Write($"***{total.ToString("C2")} CHARGED TO CARD ENDING {lastfour}***");
            EmptyCart(CartItems);
            Console.WriteLine("\n\n\n\n<PRESS ANY KEY TO RETURN TO MAIN MENU");
            Console.Read();
        }

        public void CheckPayment(double total, List<Cart> CartItems)
        {
            Console.WriteLine();
            Console.WriteLine("ENTER CHECK NUMBER <XXXXXXXXX XXXXXX XXX, including spaces>:");
            string CheckNumber = Validation.ValidateCheck(Console.ReadLine());
            string endcheck = CheckNumber.Substring(CheckNumber.Length - 3);

            PrintReceipt(CartItems);
            Console.WriteLine($"***CHECK ENDING IN {endcheck} RECEIVED FOR {total.ToString("C2")}***");
            EmptyCart(CartItems);
            Console.WriteLine("\n\n\n\n<PRESS ANY KEY TO RETURN TO MAIN MENU");
            Console.Read();
        }

        public void EmptyCart(List<Cart> CartItems)
        {
            for (int i = 0; i <= CartItems.Count - 1; i = 0)
            {
                CartItems.Remove(CartItems[i]);
            }
        }

        public void AddNewBeer()
        {
            Beer.AppendBeerList("../../ProductList.txt", Beer.NewBeerString());
            List<Beer> BeerSelection = CreateList();
        }

        public void RunInventoryManager(string FileName, List<Beer> BeerSelection)
        {
            Console.WriteLine($"[1] Add To Inventory\n[2] Remove From Inventory\n[3] Return To Main Menu");
            ConsoleKey ManageInventory = Validation.ValidateInventoryManager("Please select option [1][2] or [3]");

            if (ManageInventory == ConsoleKey.D1 || ManageInventory == ConsoleKey.NumPad1)
            {
                AddNewBeer();
            }
            else if (ManageInventory == ConsoleKey.D2 || ManageInventory == ConsoleKey.NumPad2)
            {
                Console.Clear();
                DisplaySelection(BeerSelection);
                Beer.RemoveBeer(FileName, BeerSelection);
            }
            else
            {
                Console.Clear();
                PrintMenu();
            }
        }

        public double CheckDOB()
        {
            DateTime Birthday = Validation.ValidateDOB("Please enter the buyer's DOB <YYYY/MM/DD>: ");
            TimeSpan Difference;
            double AgeOfBuyer;
            double NumberOfDays;

            Difference = (DateTime.Today - Birthday);
            NumberOfDays = Difference.TotalDays;
            AgeOfBuyer = NumberOfDays / 365.2422;

            return AgeOfBuyer;
        }

        public void PrintReceipt(List<Cart> CartItems)
        {
            Console.Clear();
            Console.WriteLine("THANK YOU FOR YOUR PURCHASE ON " + DateTime.Now+ "\nPLEASE SHOP WITH US AGAIN SOON!");
            Console.WriteLine($"{"ITEM",-25}{"PRICE",-15}{"QTY",-15}{"SUBTOTAL",-10}");
            Console.WriteLine(new string('-', 75));

            foreach (Cart item in CartItems)
            {
                Console.WriteLine($"{item.BeerName,-20}\t{item.BeerPrice,-10}\t{item.BeerQty,-10}\t{item.Subtotal,-10}");
            }
            Console.WriteLine();

            //calculate and display costs
            double Subtotal = CalculateCartTotal(CartItems);
            double TaxOwed = (Subtotal * 1.06) - Subtotal;
            double GrandTotal = Subtotal + TaxOwed;

            Console.WriteLine($"SUBTOTAL:\t{Subtotal.ToString("C2")}");
            Console.WriteLine($"TAX:\t\t{TaxOwed.ToString("C2")}");
            Console.WriteLine($"ORDER TOTAL:\t{GrandTotal.ToString("C2")}");
        }
    }
}
