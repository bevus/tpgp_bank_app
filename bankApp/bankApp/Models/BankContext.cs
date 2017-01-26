using System.Data.Entity;

namespace BankApp.Models
{
    public class BankContext : DbContext
    {
        public BankContext() : base("bank_connection")
        {
           
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Banker> Bankers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AccountToAcountTransaction> AccountToAccountTransactions { get; set; }
        public DbSet<AgencyTransaction> AgencyTransactions { get; set; }
        public DbSet<CDTransaction> CDTransactions { get; set; }
    }
}