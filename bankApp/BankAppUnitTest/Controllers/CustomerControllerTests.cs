using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BankApp.Models;
using Moq;
using MvcContrib.TestHelper;
using System.Web.Mvc;
using bankApp.Util;

namespace BankApp.Controllers.Tests
{
    [TestClass()]
    public class CustomerControllerTests
    {
        public CustomerController CustomerController;
        public Mock<ICustomerRepo> CustomerRepo;
        public Mock<IAccountRepo> AccountRepo;
        public Mock<ICredit> Credit;

        public List<Customer> customers;
        public List<Account> accounts;
        public List<Transaction> transactions;
        private List<Banker> bankers;

        [TestInitialize]
        public void Setup()
        {
            CustomerRepo = new Mock<ICustomerRepo>();
            AccountRepo = new Mock<IAccountRepo>();
            Credit = new Mock<ICredit>();

            CustomerController = new CustomerController(
                CustomerRepo.Object,
                AccountRepo.Object,
                Credit.Object
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
                new Customer {FirstName = "fn-2", LastName = "ln-2", AccountNumber = "2222", ID = 2, Banker_ID = 1, Accounts = accounts2, Password = "123456"},
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
            CustomerController.ConnectedCustomer = customers[0];
        }
        [TestMethod()]
        public void CustomerControllerTest()
        {
            Assert.IsNotNull(CustomerController);
        }

        [TestMethod()]
        public void IndexTestConnctedRedirectToTransactions()
        {
            //Arrange
            //Act
            var result = CustomerController.Index() as RedirectToRouteResult;
            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Transactions");
            Assert.AreEqual(result.RouteValues["controller"], "Transaction");
        }

        [TestMethod]
        public void IndexNoConnectedRedirectHome()
        {
            //Arrange
            CustomerController.Session.Clear();
            //Act
            var result = CustomerController.Index() as RedirectToRouteResult;
            //Assert
            TestUtils.AssertRedirectToIndexHome(result);
        }

        [TestMethod()]
        public void PrintRIBCustomerTestConnectedRedirectToPrintRIB()
        { 
            //Arrange
            //Act
            var result = CustomerController.PrintRIBCustomer() as RedirectToRouteResult;
            //Assert
            Assert.AreEqual(result.RouteValues["action"], "PrintRIB");
        }
        public void PrintRIBCustomerTestNotConnectedRedirect()
        {
            CustomerController.Session.Clear();
            //Act
            var result = CustomerController.PrintRIBCustomer() as RedirectToRouteResult;
            //Assert
            TestUtils.AssertRedirectToIndexHome(result);
        }

        [TestMethod()]
        public void PrintRIBTest()
        {
            //Arrange
            CustomerController.Session[Utils.SessionRIBCustomer] = customers[0];
            AccountRepo.Setup(r => r.GetAccounts()).Returns(accounts);
            //Act
            var result = CustomerController.PrintRIB() as ViewResult;
            var model = result.Model as List<Account>;
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(2, model.Count);
        }

        [TestMethod()]
        public void PrintRibNoCustomerRibInSessionRedirect()
        {
            //Arrange
            CustomerController.Session.Clear();
            //Act
            var result = CustomerController.PrintRIBCustomer() as RedirectToRouteResult;
            //Assert
            TestUtils.AssertRedirectToIndexHome(result);
        }

        [TestMethod()]
        public void PrintRIBByAccountTest()
        {
            //Arrange
            CustomerController.Session[Utils.SessionRIBCustomer] = customers[0];
            //Act
            AccountRepo.Setup(r => r.GetAccountByID(accounts[0].ID)).Returns(accounts[0]);
            var result = CustomerController.PrintRIBByAccount(accounts[0].ID) as JsonResult;
            var data = TestUtils.GetJsonValue<string>(result, "IBAN");
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("IBAN-1", data);
        }

        [TestMethod()]
        public void SimulateCreditTestAccept()
        {
            //Arrange
            Credit.Setup(c => c.CheckCredit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            //Act
            var result = CustomerController.SimulateCredit(new SimulateCredit()) as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(result.TempData["notice"], "Votre demende sera acceptée");
        }

        [TestMethod()]
        public void SimulateCreditTestReject()
        {
            //Arrange
            Credit.Setup(c => c.CheckCredit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            //Act
            var result = CustomerController.SimulateCredit(new SimulateCredit()) as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(result.TempData["error"], "Votre demende ne sera pas acceptée");
        }

        [TestMethod()]
        public void LogoutTest()
        {
            //Act
            var result = CustomerController.Logout() as RedirectToRouteResult;
            //Assert
            TestUtils.AssertRedirectToIndexHome(result);
            Assert.IsNull(CustomerController.Session[Utils.SessionCustomer]);
            Assert.IsNull(CustomerController.Session[Utils.SessionAddAccountCustomer]);
            Assert.IsNull(CustomerController.Session[Utils.SessionBanker]);
            Assert.IsNull(CustomerController.Session[Utils.SessionRIBCustomer]);
            Assert.IsNull(CustomerController.Session[Utils.SessionTransactionCustomer]);
        }

        [TestMethod()]
        public void ChangePasswordTest()
        {   //Arrange
            //Act
            var result = CustomerController.ChangePassword();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ChangePasswordNotConnectedRedirectHome()
        {
            //Arrange
            CustomerController.ConnectedCustomer = null;
            //Act
            var result = CustomerController.ChangePassword() as RedirectToRouteResult;
            //Assert
            TestUtils.AssertRedirectToIndexHome(result);
        }

        [TestMethod()]
        public void ChangePasswordOldAndConfirmedAreEquelTest()
        {
            //Arrange
            var form = new ChangePasswordCustomerForm {OldPassword = "123456"};
            //Act
            var result = CustomerController.ChangePassword(form) as RedirectToRouteResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["controller"], "Customer");
            CustomerRepo.Verify(r => r.UpdateCustomer(It.IsAny<Customer>()), Times.Once);
            CustomerRepo.Verify(r => r.Save(), Times.Once);
        }
        [TestMethod()]
        public void ChangePasswordOldAndConfirmedAreNotEquelTest()
        {
            //Arrange
            var oldPassword = CustomerController.ConnectedCustomer.Password;
            var form = new ChangePasswordCustomerForm {OldPassword = "876544"};
            //Act
            var result = CustomerController.ChangePassword(form);
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            CustomerRepo.Verify(r => r.UpdateCustomer(It.IsAny<Customer>()), Times.Never);
            CustomerRepo.Verify(r => r.Save(), Times.Never);
        }
    }
}