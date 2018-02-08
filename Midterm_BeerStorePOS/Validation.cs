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
        public static string validateSelection(string tempInput)
        {

            // UserInput = Validation.validateSelection(UserInput);
            while (true)
            {

                if (Regex.IsMatch(tempInput, "([1-4])"))
                {
                    return tempInput;
                }
                Console.WriteLine("Please enter valid input (1-4)");
                tempInput = Console.ReadLine();
            }
        }

        public static string validateAddItem(string tempAddItem)
        {
            while (true)
            {
                if ((Regex.IsMatch(tempAddItem, "^(Y|N)$")))
                {
                    return tempAddItem;
                }
                Console.WriteLine("Please enter a vaild answer!");
                tempAddItem = Console.ReadLine().ToUpper();
            }
        }

        public static int validateItem(int tempItemInput)
        {
            while (true)
            {
                // Validation.validateItem(ItemInput);
                if (tempItemInput >= 1 && tempItemInput <= 12) // switch out to beerSelection.count
                {
                    return tempItemInput;
                }
                Console.WriteLine("Please enter valid input (1-12)");
                tempItemInput = int.Parse(Console.ReadLine());
            }
        }

        public static string validateQauntity(string tempQauntity)
        {
            while (true)
            {
                // Validation.validateItem(ItemInput);
                if (Regex.IsMatch(tempQauntity, "(^[1-9]{1}[0-9]*$)"))
                {
                    return tempQauntity;
                }
                Console.WriteLine("Please enter valid number");
                tempQauntity = Console.ReadLine();
            }
        }
    }
}
