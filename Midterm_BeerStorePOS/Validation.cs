using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Midterm_BeerStorePOS
{
    class Validation
    {
        public static ConsoleKey ValidateSelection(string message)
        {
            //since i changed the logic for the menu to read the key entered, I changed this valdiation
            {
                Console.WriteLine(message);
                ConsoleKey input = Console.ReadKey().Key;
                while (input != ConsoleKey.D1 && input != ConsoleKey.NumPad1 &&
                    input != ConsoleKey.D2 && input != ConsoleKey.NumPad2 &&
                    input != ConsoleKey.D3 && input != ConsoleKey.NumPad3 &&
                    input != ConsoleKey.D4 && input != ConsoleKey.NumPad4)
                {
                    Console.WriteLine($"Invalid input! {message}");
                    input = Console.ReadKey().Key;
                }
                return input;
            }
            //while (true)
            //{

            //    if (Regex.IsMatch(tempInput, "([1-4])"))
            //    {
            //        return tempInput;
            //    }
            //    Console.WriteLine("Please enter valid input (1-4)");
            //    tempInput = Console.ReadLine();
            //}
        }

        //public static string validateAddItem(string tempAddItem)
        //{
        //    while (true)
        //    {
        //        if ((Regex.IsMatch(tempAddItem, "^(Y|N)$")))
        //        {
        //            return tempAddItem;
        //        }
        //        Console.WriteLine("Please enter a vaild answer!");
        //        tempAddItem = Console.ReadLine().ToUpper();
        //    }
        //}
        public static ConsoleKey CheckYorN(string message) // changed Y or N validation from above to a consolekey. we can reuse this for any Y or N choice
        {
            Console.WriteLine(message);
            ConsoleKey input = Console.ReadKey().Key;
            while (input != ConsoleKey.Y && input != ConsoleKey.N)
            {
                Console.WriteLine($"Invalid input! {message}");
                input = Console.ReadKey().Key;
            }
            return input;
        }


        public static bool ValidateItemChoice(string tempItemInput, List<Beer> Inventory) //updated this to take a list to make the length flexible
        {
            //while (true)
            //{
            //    if (tempItemInput >= 1 && tempItemInput <= Inventory.Count) // switch out to beerSelection.count
            //    {
            //        return tempItemInput;
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Please enter valid input! <1-{Inventory.Count}>");
            //        tempItemInput = int.Parse(Console.ReadLine());
            //    }             
            //}
            if (Regex.Match(tempItemInput, @"^[1-9]{1}[0-9]*$").Success)//must contain number starting at 1 or more
            {
                if (int.Parse(tempItemInput) >= 1 && int.Parse(tempItemInput) <= Inventory.Count)//input must be between lowest and highest on list
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;

        }

        public static string ValidateQuantity(string tempQuantity) //fixed spelling :)
        {
            while (true)
            {
                if (Regex.IsMatch(tempQuantity, "(^[1-9]{1}[0-9]*$)"))
                {
                    return tempQuantity;
                }
                Console.Write("Invalid number! Please enter quantity: ");
                tempQuantity = Console.ReadLine();
            }
        }
    }
}
