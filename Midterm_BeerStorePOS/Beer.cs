using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Midterm_BeerStorePOS
{
    class Beer
    {
        //properties
        public string BeerName { set; get; }
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

        public static void RemoveBeer(string FileName, List<Beer> BeerList)
        {
            {
                bool repeat = true;
                while (repeat)
                {
                    Console.WriteLine("Which beer would you like to remove?"); ///create way to go back to main menu

                    string Input = Console.ReadLine();

                    if (BeerList.Find(x => x.BeerName == Input) != null)
                    {

                        BeerList.Remove(BeerList.Find(x => x.BeerName == Input));
                        StreamWriter Writer = new StreamWriter(FileName);

                        foreach (Beer beer in BeerList)
                        {
                            Writer.WriteLine($"{beer.BeerName}, {beer.BeerStyle}, {beer.BeerDescription}, {beer.BeerPrice}");
                        }

                        Writer.Close();
                        repeat = false;
                    }
                    else
                    {
                        Console.WriteLine($"{Input} is not a country on this list!");

                    }
                }
            }
        }

        public static string NewBeerString()
        {
            Console.Write("Beer Name: ");
            string name = Validation.CheckForEmptyString(Console.ReadLine());
            Console.Write("Style: ");
            string style = Validation.CheckForEmptyString(Console.ReadLine());
            Console.Write("Description: ");
            string description = Validation.CheckForEmptyString(Console.ReadLine());
            Console.Write("Price: ");
            string price = Validation.CheckBeerPriceEntry(Console.ReadLine());
            string NewBeerString = $"{name}, {style}, {description}, {price}";

            Console.WriteLine($"{name} successfully added to inventory!");
            System.Threading.Thread.Sleep(1000);

            return NewBeerString;
        }
    }
}
