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

    }
}
