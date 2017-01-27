using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApp.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public List<Account> Accounts { get; set; }
        public Banker Banker { get; set; }
        [ForeignKey("Banker")]
        public int Banker_ID { get; set; }
    }
}