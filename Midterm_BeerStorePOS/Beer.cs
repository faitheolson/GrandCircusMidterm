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

            while (CurrentLine != null && CurrentLine != "")
            {
                    BeerList.Add(CurrentLine);
                    CurrentLine = Reader.ReadLine();
            }
            
            Reader.Close();

            return BeerList;
        }

        //add beer to the productlist file
        public static List<Beer> AddReWriteBeerSelection (string FileName, string Input, List<Beer> BeerSelection)
        {
            string[] temp = Input.Split(',');//Break new beer Input into parts using commas to put into BeerSelection List Constructor
            BeerSelection.Add(new Beer(temp[0], temp[1], temp[2], temp[3])); //new beer is added to BeerSelection List
           
            StreamWriter Writer = new StreamWriter(FileName);
            foreach (Beer item in BeerSelection)//All beers in Beer Selection List are written to txt file- overwriting old list to include new beers to prevent any blank lines from being inserted between lines
            {
                Writer.WriteLine($"{item.BeerName},{item.BeerStyle},{item.BeerDescription},{item.BeerPrice}");
            }
            Writer.Close();
            return BeerSelection;
        }

        public static void RemoveBeer(string FileName, List<Beer> BeerSelection)//Changed List name to BeerSelection throughout entire method for consistency. 
        {
            {

                bool repeat = true;
                while (repeat)
                {
                    Console.WriteLine("Which beer would you like to remove?"); ///create way to go back to main menu

                    string Input = Console.ReadLine();

                    if (BeerSelection.Find(x => x.BeerName == Input) != null)
                    {

                        BeerSelection.Remove(BeerSelection.Find(x => x.BeerName == Input));
                        Console.WriteLine($"{Input} successfully removed from inventory.");
                        System.Threading.Thread.Sleep(1500);

                        StreamWriter Writer = new StreamWriter(FileName);

                        foreach (Beer beer in BeerSelection)
                        {
                            Writer.WriteLine($"{beer.BeerName},{beer.BeerStyle},{beer.BeerDescription},{beer.BeerPrice}");
                        }

                        Writer.Close();
                        repeat = false;
                    }
                    else
                    {
                        Console.WriteLine($"{Input} is not in the inventory. Are you sure you entered the name correctly?");

                    }
                }
            }
        }

        public static string NewBeerString()
        {

            Console.WriteLine();
            Console.Write("Beer Name: ");
            string name = Validation.CheckForEmptyString(Console.ReadLine());
            Console.Write("Style: ");
            string style = Validation.CheckForEmptyString(Console.ReadLine());
            Console.Write("Description: ");
            string description = Validation.CheckForEmptyString(Console.ReadLine());
            Console.Write("Price: ");
            string price = Validation.CheckBeerPriceEntry(Console.ReadLine());
            string NewBeerString = $"{name},{style},{description},{price}";

            Console.WriteLine($"{name} successfully added to inventory!");
            System.Threading.Thread.Sleep(1000);

            return NewBeerString;
        }
        public static void FormatBeerTextFile(string FileName)//This is called only once at the beginning so that first added beer will be appended on new line.
        {
            StreamWriter Writer = new StreamWriter(FileName, true);
            Writer.WriteLine();
            Writer.Close();
        }
    }
}
