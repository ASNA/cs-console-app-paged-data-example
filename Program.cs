using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestPagedDataCS
{
    class Program
    {
        static void Main(string[] args)
        {
            Test t = new Test();
            t.Run();
        }
    }

    class Test
    {
        public void Run()
        {
            Repository repo = new Repository();

            Console.Write("Enter page size: ");
            string consoleInput = Console.ReadLine();
            int pageSize = Convert.ToInt32(consoleInput);

            int pageNumber = 0;

            while(true) {
                Console.Write("Enter page number (or press enter for next page): ");

                consoleInput = Console.ReadLine();
                if (!String.IsNullOrEmpty(consoleInput) && consoleInput.ToLower().Trim().Substring(0, 1) == "q")
                {
                    return;
                }

                if (String.IsNullOrEmpty(consoleInput)) {
                    consoleInput = (pageNumber + 1).ToString();
                }

                if (!Regex.IsMatch(consoleInput, @"^\d+$")) {
                    Console.WriteLine("Please enter a number");
                }

                pageNumber = Convert.ToInt32(consoleInput);

                bool moreRecords = repo.GetPage(pageNumber, pageSize);
                if (moreRecords) {
                    Console.WriteLine("More pages to show...");
                }
                else
                {
                    Console.WriteLine("No more pages to show.");
                }
            }
        }
    }


}
