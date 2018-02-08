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




        public static int validateItem(int tempItemInput)
        {

            // UserInput = Validation.validateSelection(UserInput);
            while (true)
            {
               // Validation.validateItem(ItemInput);
                if (tempItemInput >=1 && tempItemInput <=12)
                {
                    return  tempItemInput;
                }
                Console.WriteLine("Please enter valid input (1-12)");
                tempItemInput = int.Parse (Console.ReadLine());
            }
        }


    }
}
