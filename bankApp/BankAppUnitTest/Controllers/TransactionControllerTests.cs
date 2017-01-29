using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BankApp.Models;
using BankAppUnitTest;
using Moq;
using MvcContrib.TestHelper;

namespace BankApp.Controllers.Tests
{
    [TestClass()]
    public class TransactionControllerTests
    {
        public TransactionController TransactionController;
        public Mock<ICustomerRepo> CustomerRepo;
        public Mock<IAccountRepo> AccountRepo;
        public Mock<ITransactionRepo> TransactionRepo;
        public List<Customer> customers;
        public List<Account> accounts;
        public List<Transaction> transactions; 
        [TestInitialize]
        public void Setup()
        {
            CustomerRepo = new Mock<ICustomerRepo>();
            AccountRepo = new Mock<IAccountRepo>();
            TransactionRepo = new Mock<ITransactionRepo>();
            TransactionController = new TransactionController(
                CustomerRepo.Object,
                AccountRepo.Object,
                TransactionRepo.Object  
            );
            var controllerBuilder = new TestControllerBuilder();
            controllerBuilder.InitializeController(TransactionController);
            var accounts1 = new List<Account>
            {
                new Account {BIC = "BIC-1", IBAN = "IBAN-1", ID = 1, Owner_ID = 1, Solde = 2000},
                new Account {BIC = "BIC-1", IBAN = "IBAN-2", ID = 2, Owner_ID = 1, Solde = 0}
            };
            var accounts2 = new List<Account>
            {
                new Account {BIC = "BIC-1", IBAN = "IBAN-3", ID = 3, Owner_ID = 2, Solde = 2000},
                new Account {BIC = "BIC-1", IBAN = "IBAN-4", ID = 4, Owner_ID = 2, Solde = 0}
            };
            transactions = new List<Transaction>
            {
                new AccountToAcountTransaction {ID = 1, Account = accounts1[0], Amount = 5000, Date = DateTime.Now, TransactionType = TransactionType.CREDIT, Title = "virement - hacene kedjar" },
                new AccountToAcountTransaction {ID = 2, Account = accounts1[0], Amount = 5000, Date = DateTime.Now.AddDays(-31), TransactionType = TransactionType.CREDIT, Title = "virement - hacene kedjar" },
                new AccountToAcountTransaction {ID = 3, Account = accounts2[0], Amount = 1260, Date = DateTime.Now, TransactionType = TransactionType.DEBIT, Title = "virement"}
            };
            customers = new List<Customer>
            {
                new Customer {FirstName = "fn-1", LastName = "ln-1", AccountNumber = "1111", ID = 1, Banker_ID = 1, Accounts = accounts1, Password = "123456"},
                new Customer {FirstName = "fn-2", LastName = "ln-2", AccountNumber = "2222", ID = 1, Banker_ID = 2, Accounts = accounts2, Password = "123456"},
            };
            accounts = new List<Account>();
            accounts.AddRange(accounts1);
            accounts.AddRange(accounts2);
            TransactionController.TransactionCustomer = customers[0];
        }
        [TestMethod()]
        public void TransactionControllerTest()
        {
            var tc = new TransactionController();
            Assert.IsNotNull(tc);
        }

        [TestMethod()]
        public void TransferGetReturnsViewTransferOnValideSessionTest()
        {
            // Arrange
            AccountRepo.Setup(r => r.GetAccounts()).Returns(customers[0].Accounts);
            // Act
            var result = TransactionController.Transfer();
            var vResult = result as ViewResult;
            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(vResult.Model, typeof(TrensferFrom));
            var model = vResult.Model as TrensferFrom;
            Assert.AreEqual(model.AccountsId.Count(), customers[0].Accounts.Count);
        }
        [TestMethod()]
        public void TransferGetRedirectTransferOnInvalideSessionTest()
        {
            // Arrange
            AccountRepo.Setup(r => r.GetAccounts()).Returns(customers[0].Accounts);
            TransactionController.TransactionCustomer = null;
            // Act
            var result = TransactionController.Transfer();
            var rResult = result as RedirectToRouteResult;
            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void TransferPostRedirectTransferOnInvalideSessionTest()
        {
            // Arrange
            TransactionController.TransactionCustomer = null;
            // Act
            var result = TransactionController.Transfer();
            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
        [TestMethod]
        public void ShowViewOnValideSessionInvalideModel()
        {
            // Arrange
            AccountRepo.Setup(r => r.GetAccounts()).Returns(accounts);
            CustomerRepo.Setup(r => r.GetCustomers()).Returns(customers);
            TransactionRepo.Setup(r => r.GetTransactions()).Returns(transactions);
            CustomerRepo.Setup(r => r.GetCustomerByID(1)).Returns(customers[0]);
            TransactionController.ViewData.ModelState.AddModelError("", "");
            // Act
            var result = TransactionController.Transfer(new TrensferFrom());
            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void RedirectOnValideSessionValideModel()
        {
            // Arrange
            AccountRepo.Setup(r => r.GetAccounts()).Returns(accounts);
            CustomerRepo.Setup(r => r.GetCustomers()).Returns(customers);
            TransactionRepo.Setup(r => r.GetTransactions()).Returns(transactions);

            CustomerRepo.Setup(r => r.GetCustomerByID(It.IsAny<int>())).Returns(customers[0]);
            AccountRepo.Setup(r => r.GetAccountByID(It.IsAny<int>())).Returns(accounts[0]);

            var soldeBeforeSource = accounts[0].Solde;
            var destinationAccount = accounts.Find(a => a.IBAN == "IBAN-2");
            var soldeBeforExsitingDestination = destinationAccount.Solde;
            // Act
            var result = TransactionController.Transfer(new TrensferFrom
            {
                Amount = 200,
                SourceAccount = 1,
                IBAN = "IBAN-2",
                Label = "Virement",
                DestinationFullName = "fn"
            });
            var rResult = result as RedirectToRouteResult;
            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(rResult.RouteValues["action"], "Index");
            Assert.AreEqual(rResult.RouteValues["controller"], "Customer");
            Assert.AreEqual(soldeBeforeSource - 200, accounts[0].Solde);
            Assert.AreEqual(soldeBeforExsitingDestination + 200, destinationAccount.Solde);
            TransactionRepo.Verify(r => r.InsertTransaction(It.IsAny<AccountToAcountTransaction>()), Times.Exactly(2));
        }

        [TestMethod]
        public void TransactionRedirectIndexHomeWhenInvalideCustomer()
        {
            //Arrange
            TransactionController.TransactionCustomer = null;
            //Act
            var result = TransactionController.Transactions() as RedirectToRouteResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["controller"], "Home");
        }

        [TestMethod]
        public void TransactionReturnViewWhenValidCustomer()
        {
            //Arrange
            TransactionRepo.Setup(r => r.GetTransactions()).Returns(transactions);
            CustomerRepo.Setup(r => r.GetCustomerByID(It.IsAny<int>())).Returns(customers[0]);
            AccountRepo.Setup(r => r.GetAccountByID(1)).Returns(accounts[0]);
            AccountRepo.Setup(r => r.GetAccountByID(2)).Returns(accounts[1]);
            //Act
            var result = TransactionController.Transactions() as ViewResult;
            var model = result.Model as List<Transaction>;
            //Assert
            Assert.AreEqual((int) result.ViewBag.Solde, accounts[0].Solde + accounts[1].Solde);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(result.ViewName == "");
            Assert.IsFalse(model.Exists(t => t.ID == 2));
        }

        //Arrange

        //Act

        //Assert
    }
}