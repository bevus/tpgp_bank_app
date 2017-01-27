
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApp.Models
{
    public class Account
    {
        public int ID { get; set; }
        public double Solde { get; set; }
        public Customer Owner { get; set; }
        [ForeignKey("Owner")]
        public int Owner_ID { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public List<Transaction> Transactions { get; set; }

        public void Credit(int amount)
        {
            Solde += amount;
        }

        public bool Debit(int amount)
        {
            if(Solde - amount < 0)
                return false;
            Solde -= amount;
            return true;
        }
    }
}
