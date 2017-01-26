using System.Collections.Generic;

namespace BankApp.Models
{
    interface ITransactionRepo
    {
        IEnumerable<Transaction> GetTransactions();
        Transaction GetTransactionByID(int transactionId);
        void InsertTransaction(Transaction transaction);
        void DeleteTransaction(int transactionId);
        void UpdateTransaction(Transaction transaction);
        void Save();
    }
}
