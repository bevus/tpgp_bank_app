using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.Models;
using Moq;
using MvcContrib.TestHelper;
using System.Web.Mvc;
using System.Web.Helpers;

namespace BankApp.Controllers.Tests
{
    [TestClass()]
    public class CustomerControllerTests
    {
        public CustomerController CustomerController;
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
            CustomerController = new CustomerController(
                CustomerRepo.Object,
                AccountRepo.Object,
                BankerRepo.Object
            );
            var controllerBuilder = new TestControllerBuilder();
            controllerBuilder.InitializeController(CustomerController);
            var accounts1 = new List<Account>
            {
                new Account {BIC = "BIC-1", IBAN = "IBAN-1", ID = 0, Owner_ID = 1, Solde = 2000},
                new Account {BIC = "BIC-1", IBAN = "IBAN-2", ID = 1, Owner_ID = 1, Solde = 0}
            };
            var accounts2 = new List<Account>
            {
                new Account {BIC = "BIC-1", IBAN = "IBAN-3", ID = 2, Owner_ID = 2, Solde = 2000},
                new Account {BIC = "BIC-1", IBAN = "IBAN-4", ID = 3, Owner_ID = 2, Solde = 0}
            };

            customers = new List<Customer>
            {
                new Customer {FirstName = "fn-1", LastName = "ln-1", AccountNumber = "1111", ID = 1, Banker_ID = 1, Accounts = accounts1, Password = "123456"},
                new Customer {FirstName = "fn-2", LastName = "ln-2", AccountNumber = "2222", ID = 1, Banker_ID = 1, Accounts = accounts2, Password = "123456"},
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

        }
        [TestMethod()]
        public void CustomerControllerTest()
        {
            var cc = new CustomerController();
            Assert.IsNotNull(cc);
        }

        [TestMethod()]
        public void IndexTest()
        {//Arrange
            CustomerController.Session[Utils.SessionCustomer] = customers[0];
            //Act
            var result = CustomerController.Index() as RedirectToRouteResult;
            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Transactions");
            Assert.AreEqual(result.RouteValues["controller"], "Transaction");
             }

        [TestMethod()]
        public void PrintRIBCustomerTest()
        { //Arrange
            CustomerController.Session[Utils.SessionCustomer] = customers[0];
            //Act
            var result = CustomerController.PrintRIBCustomer() as RedirectToRouteResult;
            //Assert
            Assert.AreEqual(result.RouteValues["action"], "PrintRIB");
         }
      
        [TestMethod()]
        public void PrintRIBTest()
        {
           //Arrange
            CustomerController.Session[Utils.SessionRIBCustomer] = customers[0];
            //Act
            AccountRepo.Setup(r => r.GetAccounts()).Returns(accounts);
            var result = CustomerController.PrintRIB();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
      }
        
        [TestMethod()]
        public void PrintRIBByAccountTest()
        {
            //Arrange
            CustomerController.Session[Utils.SessionRIBCustomer] = customers[0];
            //Act
            AccountRepo.Setup(r => r.GetAccountByID(accounts[0].ID)).Returns(accounts[0]);
            var result = CustomerController.PrintRIBByAccount(accounts[0].ID) as JsonResult;
            var data = GetJsonValue<string>(result, "IBAN");
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("IBAN-1", data);

        }
        public T GetJsonValue <T> (JsonResult jsonResult, string propertyname)
        {
            var property = jsonResult.Data.GetType().GetProperties().Where(a => string.Compare(a.Name, propertyname) == 0).FirstOrDefault();
            if (property == null)
                throw new Exception();
            return (T)property.GetValue(jsonResult.Data,null);
         }

        [TestMethod()]
        public void SimulateCreditTest()
        {
            //Arrange
            CustomerController.Session[Utils.SessionCustomer] = customers[0];
            SimulateCredit form = new SimulateCredit();
            form.RequestedAmount = 9000;
            form.Contribution = 4000;
            form.HouseholdIncomes = 10000;
            form.Duration = 12;
            //Act
            var result = CustomerController.SimulateCredit(form) as RedirectToRouteResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
         }

        [TestMethod()]
        public void LogoutTest()
        { 
            //Act
            var result = CustomerController.Logout();
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod()]
        public void ChangePasswordTest()
        {   //Arrange
            CustomerController.Session[Utils.SessionCustomer] = customers[0];
            //Act
            var result = CustomerController.ChangePassword();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void ChangePasswordOldAndConfirmedAreEquelTest()
        {
            //Arrange
            CustomerController.Session[Utils.SessionCustomer] = customers[0];
            ChangePasswordCustomerForm form = new ChangePasswordCustomerForm();
            form.OldPassword = "123456";
            CustomerRepo.Setup(r => r.UpdateCustomer(customers[0]));
            CustomerRepo.Setup(r => r.Save());
            //Act
            var result = CustomerController.ChangePassword(form);
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));

        }
        [TestMethod()]
        public void ChangePasswordOldAndConfirmedAreNotEquelTest()
        {
            //Arrange
            CustomerController.Session[Utils.SessionCustomer] = customers[0];
            ChangePasswordCustomerForm form = new ChangePasswordCustomerForm();
            form.OldPassword = "876544";
            CustomerRepo.Setup(r => r.UpdateCustomer(customers[0]));
            CustomerRepo.Setup(r => r.Save());
            //Act
            var result = CustomerController.ChangePassword(form);
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
         }
    }
}