using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var now = DateTime.Now;
            var thirtyDaysAfter = now.AddDays(-31);

            Console.WriteLine(now + " " + thirtyDaysAfter);
        }
    }
}
