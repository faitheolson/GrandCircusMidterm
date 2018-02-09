using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Midterm_BeerStorePOS
{
    class Beer
    {
        //properties
        public string BeerName {set; get;}
        public string BeerStyle { set; get; }
        public string BeerDescription { set; get; }
        public string BeerPrice { set; get; }

        //constructor
        public Beer(string name, string style, string description, string price)
        {
            BeerName = name;
            BeerStyle = style;
            BeerDescription = description;
            BeerPrice = price;
        }

        //read productlist file and put it into a list
        public static List<string> GetBeer(string FileName)
        {
            StreamReader Reader = new StreamReader(FileName);

            string CurrentLine = Reader.ReadLine();
            List<string> BeerList = new List<string>();

            while (CurrentLine != null)
            {
                BeerList.Add(CurrentLine);
                CurrentLine = Reader.ReadLine();
            }

            Reader.Close();

            return BeerList;
        }
        //add beer to the productlist file
        public static void AppendBeerList(string FileName, string Input)
        {
            StreamWriter Writer = new StreamWriter(FileName, true);

            Writer.WriteLine(Input);

            Writer.Close();

        }

        public static string NewBeerString()
        {
            Console.WriteLine("Please enter beer Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Please enter beer Style:");
            string style = Console.ReadLine();
            Console.WriteLine("Please enter beer description:");
            string description = Console.ReadLine();
            Console.WriteLine("Please enter beer price:");
            string price = Console.ReadLine();
            string NewBeerString = $"{name}, {style}, {description}, {price}";
            return NewBeerString;
        }
    }
}
