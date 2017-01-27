using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BankApp.Models
{
    public class EFCustomerRepo : ICustomerRepo
    {
        private BankContext context;
        public EFCustomerRepo(BankContext context)
        {
            this.context = context;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return context.Customers
                .Include(customer => customer.Accounts)
                .Include(customer => customer.Banker).ToList();
        }

        public Customer GetCustomerByID(int customerId)
        {
            return context.Customers
                .Include(customer => customer.Accounts)
                .Include(customer => customer.Banker).Single(c => c.ID == customerId);
        }

        public void InsertCustomer(Customer customer)
        {
            context.Customers.Add(customer);
        }

        public void DeleteCustomer(int customerId)
        {
            var customer = context.Customers.Find(customerId);
            context.Customers.Remove(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            context.Entry(customer).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}