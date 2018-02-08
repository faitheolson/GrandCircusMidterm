using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midterm_BeerStorePOS
{
    class POSApp
    {
        public void StartApp()
        {
            PrintMenu();

            DisplaySelection();
            
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

        private void DisplaySelection()
        {
            List<Beer> BeerSelection = new List<Beer>();

            foreach(string b in Beer.GetBeer("../../ProductList.txt"))
            {
                string[] temp = b.Split(',');
                BeerSelection.Add(new Beer(temp[0],temp[1] ,temp[2] ,temp[3]));
            }

            Console.WriteLine($"{"Beer",-20}{"Style",-10}{"Package",-20}{"Price",-10}");
            //continue header format

            for (int i = 0; i< BeerSelection.Count; i++)
            {
                Console.WriteLine($"{i+1,-4}{BeerSelection[i].BeerName,-20}{BeerSelection[i].BeerStyle,-10}{BeerSelection[i].BeerDescription,-20}" +
                    $"{BeerSelection[i].BeerPrice,-10}");
            }
        }

        private void AddToCart()
        {
            //List<Cart> CartItems = new List<Cart>();
            

            //Console.WriteLine("Please make a selection [enter the number]:");
            //int BeerChoice = int.Parse(Console.ReadLine());

            ////validate that choice is on the BeerSelection list

            //Console.WriteLine("Please enter the quantity:");
            //int AmountOfBeer = int.Parse(Console.ReadLine());

            ////validate qty
            //CartItems.Add(new Cart());


        }
    }
}
