using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BankApp.Models
{
    public class DbBankInitializer: DropCreateDatabaseAlways<BankContext>
    {
        protected override void Seed(BankContext context)
        {
            

            //context.Accounts.Add(new Account() { Owner = context.Customers.ToList()[0], Solde = 1000.00 });
            //context.Accounts.Add(new Account() { Owner = context.Customers.ToList()[0], Solde = 2000.00 });
            //context.Accounts.Add(new Account() { Owner = context.Customers.ToList()[1], Solde = 3000.00 });
            //context.Accounts.Add(new Account() { Owner = context.Customers.ToList()[1], Solde = 4000.00 });
            //context.Accounts.Add(new Account() { Owner = context.Customers.ToList()[2], Solde = 1500.00 });
            //context.Accounts.Add(new Account() { Owner = context.Customers.ToList()[2], Solde = 1800.00 });
            //context.Accounts.Add(new Account() { Owner = context.Customers.ToList()[3], Solde = 1900.00 });
            //context.Accounts.Add(new Account() { Owner = context.Customers.ToList()[3], Solde = 2600.00 });
            //context.Accounts.Add(new Account() { Owner = context.Customers.ToList()[4], Solde = 6500.00 });
            //context.Accounts.Add(new Account() { Owner = context.Customers.ToList()[4], Solde = 10000.00 });
            //context.SaveChanges();
            
            

            //context.Agencys.Add(new Agency() { Name = "Villetaneuse" });
            //context.Agencys.Add(new Agency() { Name = "Paris 5" });
            //context.Agencys.Add(new Agency() { Name = "Pontoise" });
            //context.Agencys.Add(new Agency() { Name = "Paris 6" });
            //context.SaveChanges();



            //context.CashDespensers.Add(new CashDespenser() { type = CDType.INSIDE });
            //context.CashDespensers.Add(new CashDespenser() { type = CDType.OUTSIDE });
            //context.CashDespensers.Add(new CashDespenser() { type = CDType.INSIDE });
            //context.CashDespensers.Add(new CashDespenser() { type = CDType.OUTSIDE });
            //context.CashDespensers.Add(new CashDespenser() { type = CDType.INSIDE });
            //context.SaveChanges();

            //context.Transactions.Add(new AccountToAcountTransaction() {
            //    Amount = 50.0,
            //    Date = new DateTime(),
            //    TransactionType = TransactionType.CREDIT,
            //    Source = context.Accounts.ToArray()[0],
            //    Destination = context.Accounts.ToArray()[1]
            //});

            //context.Transactions.Add(new AccountToAcountTransaction()
            //{
            //    Amount = 150.0,
            //    Date = new DateTime(),
            //    TransactionType = TransactionType.CREDIT,
            //    Source = context.Accounts.ToArray()[0],
            //    Destination = context.Accounts.ToArray()[1]
            //});

            //context.Transactions.Add(new AccountToAcountTransaction()
            //{
            //    Amount = 585.0,
            //    Date = new DateTime(),
            //    TransactionType = TransactionType.CREDIT,
            //    Source = context.Accounts.ToArray()[1],
            //    Destination = context.Accounts.ToArray()[0]
            //});
            //context.SaveChanges();

            //context.Transactions.Add(new AgencyTransaction()
            //{
            //    Amount = 60.5,
            //    Date = new DateTime(),
            //    TransactionType = TransactionType.CREDIT,
            //    Agency = context.Agencys.ToArray()[0],
            //    Account = context.Accounts.ToArray()[0],
            //    Type = AgencyOperationType.DEPOSIT
            //});
            //context.Transactions.Add(new AgencyTransaction()
            //{
            //    Amount = 50.0,
            //    Date = new DateTime(),
            //    TransactionType = TransactionType.DEBIT,
            //    Agency = context.Agencys.ToArray()[0],
            //    Account = context.Accounts.ToArray()[0],
            //    Type = AgencyOperationType.DEPOSIT
            //});
            //context.Transactions.Add(new AgencyTransaction()
            //{
            //    Amount = 212.0,
            //    Date = new DateTime(),
            //    TransactionType = TransactionType.DEBIT,
            //    Agency = context.Agencys.ToArray()[0],
            //    Account = context.Accounts.ToArray()[0],
            //    Type = AgencyOperationType.DEPOSIT
            //});
            //context.SaveChanges();
            //context.Transactions.Add(new CDTransaction()
            //{
            //    Amount = 50.0,
            //    Date = new DateTime(),
            //    TransactionType = TransactionType.DEBIT,
            //    CD = context.CashDespensers.ToArray()[0],
            //    Account = context.Accounts.ToArray()[0]
            //});

            //context.Transactions.Add(new CDTransaction()
            //{
            //    Amount = 136.0,
            //    Date = new DateTime(),
            //    TransactionType = TransactionType.DEBIT,
            //    CD = context.CashDespensers.ToArray()[1],
            //    Account = context.Accounts.ToArray()[0]
            //});
            //context.SaveChanges();
            base.Seed(context);
        }
    }
}