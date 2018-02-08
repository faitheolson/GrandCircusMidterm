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
            List<Cart> CartItems = new List<Cart>();
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
                //display cart
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

            Console.WriteLine("Please select item to add to cart:");
            int ItemInput = int.Parse(Console.ReadLine()) - 1;
            Console.WriteLine("Please select quantity:");
            double QuantityInput = double.Parse(Console.ReadLine());
            double LineSubtotal = QuantityInput * double.Parse(BeerSelection[ItemInput].BeerPrice);

            CartItems.Add(new Cart(BeerSelection[ItemInput].BeerName, BeerSelection[ItemInput].BeerStyle,
             BeerSelection[ItemInput].BeerDescription, BeerSelection[ItemInput].BeerPrice, QuantityInput, LineSubtotal));

            Console.WriteLine($"You've added{BeerSelection[ItemInput].BeerName}. The price is {BeerSelection[ItemInput].BeerPrice}. Line Subtotal: {LineSubtotal}");
        }
    }
}
