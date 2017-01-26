using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BankApp.Models
{
    public class EFTransactionRepo : ITransactionRepo
    {
        private BankContext context;
        public EFTransactionRepo(BankContext context)
        {
            this.context = context;
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            return context.Transactions.ToList();
        }

        public Transaction GetTransactionByID(int transactionId)
        {
            return context.Transactions.Find(transactionId);
        }

        public void InsertTransaction(Transaction transaction)
        {
            context.Transactions.Add(transaction);
        }

        public void DeleteTransaction(int transactionId)
        {
            var transaction = context.Transactions.Find(transactionId);
            context.Transactions.Remove(transaction);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            context.Entry(transaction).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}