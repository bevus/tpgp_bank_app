using BankApp.Models;

namespace BankApp.Models
{
    public interface ISessionRepo
    {
        Customer GetCustomer();
        Banker GetBanker();
    }
}   