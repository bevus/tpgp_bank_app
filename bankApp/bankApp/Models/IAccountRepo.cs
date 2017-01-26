using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models
{
    interface IAccountRepo
    {
        IEnumerable<Account> GetAccounts();
        Account GetAccountByID(int accountId);
        void InsertAccount(Account account);
        void DeleteAccount(int accountId);
        void UpdateAccount(Account account);
        void Save();
    }
}
