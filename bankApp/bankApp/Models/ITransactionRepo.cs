using System.Collections.Generic;

namespace BankApp.Models
{
    public interface ITransactionRepo
    {
        IEnumerable<Transaction> GetTransactions();
        Transaction GetTransactionByID(int transactionId);
        void InsertTransaction(Transaction transaction);
        void DeleteTransaction(int transactionId);
        void UpdateTransaction(Transaction transaction);
        void Save();
    }
}
