using BankApp.Models;

namespace bankApp.Models
{
    public interface ISessionRepo
    {
        Customer GetCustomer();
        Banker GetBanker();
    }
}   