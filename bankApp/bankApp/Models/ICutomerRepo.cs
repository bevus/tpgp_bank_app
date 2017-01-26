using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models
{
    interface  ICutomerRepo
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomerByID(int customerId);
        void InsertCustomer(Customer customer);
        void DeleteCustomer(int customerId);
        void UpdateCustomer(Customer customer);
        void Save();
    }
}
