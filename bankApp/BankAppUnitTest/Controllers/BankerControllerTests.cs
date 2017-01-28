using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BankApp.Models;
using Moq;
using MvcContrib.TestHelper;

namespace BankApp.Controllers.Tests
{
    [TestClass()]
    public class BankerControllerTests
    {
        public BankerController BankController;
        public Mock<ICustomerRepo> CustomerRepo;
        public Mock<IAccountRepo> AccountRepo;
        public Mock<IBankerRepo> BankerRepo;
        public List<Customer> customers;
        public List<Account> accounts;
        public List<Transaction> transactions;
        private List<Banker> bankers;

        [TestInitialize]
        public void Setup()
        {
            CustomerRepo = new Mock<ICustomerRepo>();
            AccountRepo = new Mock<IAccountRepo>();
            BankerRepo = new Mock<IBankerRepo>();
            BankController = new BankerController(
                CustomerRepo.Object,
                AccountRepo.Object,
                BankerRepo.Object
            );
            var controllerBuilder = new TestControllerBuilder();
            controllerBuilder.InitializeController(BankController);
            var accounts1 = new List<Account>
            {
                new Account {BIC = "BIC-1", IBAN = "IBAN-1", ID = 1, Owner_ID = 1, Solde = 2000},
                new Account {BIC = "BIC-1", IBAN = "IBAN-2", ID = 2, Owner_ID = 1, Solde = 0}
            };
            var accounts2 = new List<Account>
            {
                new Account {BIC = "BIC-1", IBAN = "IBAN-1", ID = 1, Owner_ID = 2, Solde = 2000},
                new Account {BIC = "BIC-1", IBAN = "IBAN-2", ID = 2, Owner_ID = 2, Solde = 0}
            };

            customers = new List<Customer>
            {
                new Customer {FirstName = "fn-1", LastName = "ln-1", AccountNumber = "1111", ID = 1, Banker_ID = 1, Accounts = accounts1, Password = "123456"},
                new Customer {FirstName = "fn-2", LastName = "ln-2", AccountNumber = "2222", ID = 1, Banker_ID = 2, Accounts = accounts2, Password = "123456"},
            };

            bankers = new List<Banker>
            {
                new Banker() {FirstName = "fn-1", LastName = "ln-1", ID = 1, Mail = "john.doe@bank.fr", Customers = customers, Password = "123456"},
            };
            accounts = new List<Account>();
            accounts.AddRange(accounts1);
            accounts.AddRange(accounts2);
            transactions = new List<Transaction>
            {
                new AccountToAcountTransaction {Account = accounts1[0], Amount = 5000, Date = DateTime.Now, TransactionType = TransactionType.CREDIT, Title = "virement - hacene kedjar" },
                new AccountToAcountTransaction {Account = accounts2[0], Amount = 1260, Date = DateTime.Now, TransactionType = TransactionType.DEBIT, Title = "virement"}
            };
            BankController.Session[Utils.SessionBanker] = bankers[0];
        }
        [TestMethod()]
        public void BankerControllerTest()
        {
            
        }

        [TestMethod()]
        public void IndexTest()
        {
            
        }

        [TestMethod()]
        public void CreateTest()
        {
            
        }

        [TestMethod()]
        public void CreateTest1()
        {
            
        }

        [TestMethod()]
        public void DeleteTest()
        {
            
        }

        [TestMethod()]
        public void DeleteConfirmedTest()
        {
            
        }

        [TestMethod()]
        public void EditTest()
        {
            
        }

        [TestMethod()]
        public void EditTest1()
        {
            
        }

        [TestMethod()]
        public void LogoutTest()
        {
            
        }

        [TestMethod()]
        public void ChangePasswordTest()
        {
            
        }

        [TestMethod()]
        public void ChangePasswordTest1()
        {
            
        }

        [TestMethod()]
        public void CustomerHistoryTest()
        {
            
        }

        [TestMethod()]
        public void CustomerTrensferTest()
        {
            
        }

        [TestMethod()]
        public void CustomerRibTest()
        {
            
        }

        [TestMethod()]
        public void AddAccountTest()
        {
            
        }

        [TestMethod()]
        public void AddAccountTest1()
        {
            
        }

        [TestMethod]
        public void IndexTestConnceted()
        {
            //Arrange
            CustomerRepo.Setup(r => r.GetCustomers()).Returns(customers);
            //Act
            var result = BankController.Index();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}