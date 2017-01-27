using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BankApp.Models
{
    public class EFAccountRepo : IAccountRepo
    {
        private BankContext context;
        public EFAccountRepo(BankContext context)
        {
            this.context = context;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return context.Accounts
                .Include(account => account.Owner)
                .Include(account => account.Transactions).ToList();
        }

        public Account GetAccountByID(int accountId)
        {
            return context.Accounts
                .Include(account => account.Owner)
                .Include(account => account.Transactions).Single(o => o.ID == accountId);
        }

        public void InsertAccount(Account account)
        {
            context.Accounts.Add(account);
        }

        public void DeleteAccount(int accountId)
        {
            var account = context.Accounts.Find(accountId);
            context.Accounts.Remove(account);
        }

        public void UpdateAccount(Account account)
        {
            context.Entry(account).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}