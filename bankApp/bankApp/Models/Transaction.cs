using System;

namespace BankApp.Models
{
    public abstract class Transaction
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public Account Account { get; set; }
        public abstract string Label { get; }
    }
}
