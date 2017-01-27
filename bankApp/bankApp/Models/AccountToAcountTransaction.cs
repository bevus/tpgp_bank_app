using System;

namespace BankApp.Models
{
   public class AccountToAcountTransaction:Transaction
    {
        public Account Source { get; set; }
        public Account Destination { get; set; }
        public string Title { get; set; }

        public override string Label => Title;
    }
}
