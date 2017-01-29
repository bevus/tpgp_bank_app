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
            var banker = new BankerController();
            Assert.IsNotNull(banker);
         }

        [TestMethod]
        public void IndexTestConnceted()
        {   //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            CustomerRepo.Setup(r => r.GetCustomers()).Returns(customers);
            //Act
            var result = BankController.Index();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
      }



        [TestMethod]
        public void IndexTestNotConnceted()
        {
          var result = BankController.Index();
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
         }

        [TestMethod()]
        public void CreateTest()
        {
           //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            BankerRepo.Setup(r => r.GetBankers()).Returns(bankers);
            //Act
            var result = BankController.Create();
            var vResult = (ViewResult)result;
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(vResult.Model, typeof(AddCustomerForm));
         }

        [TestMethod()]
        public void CreatePostTestModelStateIsValideSessionValide()
        {
            //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            AccountRepo.Setup(r => r.InsertAccount(accounts[0]));
            CustomerRepo.Setup(r => r.InsertCustomer(customers[0]));
            CustomerRepo.Setup(r => r.Save());
            //Act
            var result = BankController.Create();
            var vResult = (ViewResult)result;
            ////Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }

        [TestMethod()]
        public void DeleteTest()
        {
            //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            CustomerRepo.Setup(r => r.GetCustomerByID(customers[0].ID)).Returns(customers[0]);
            //Act
            var result = BankController.Delete(customers[0].ID);
            var vResult = (ViewResult)result;
            ////Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(vResult.Model, typeof(Customer));
            Assert.AreEqual(vResult.Model, customers[0]);
     }



        [TestMethod()]
        public void LogoutTest()
        {   //Act
            var result = BankController.Logout();
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
   }

        [TestMethod()]
        public void ChangePasswordTest()
        {   
            //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            //Act
            var result = BankController.ChangePassword();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void ChangePasswordOldAndConfirmedAreEquelTest()
        {
            //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            ChangePasswordBankerForm form = new ChangePasswordBankerForm();
            form.OldPassword = "123456";
             BankerRepo.Setup(r => r.UpdateBanker(bankers[0]));
            BankerRepo.Setup(r => r.Save());
            //Act
            var result = BankController.ChangePassword(form);
            //var vResult = (ViewResult)result;
            ////Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));

        }
        [TestMethod()]
        public void ChangePasswordOldAndConfirmedAreNotEquelTest()
        {
            //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            ChangePasswordBankerForm form = new ChangePasswordBankerForm();
            form.OldPassword = "876544";
            BankerRepo.Setup(r => r.UpdateBanker(bankers[0]));
            BankerRepo.Setup(r => r.Save());
            //Act
            var result = BankController.ChangePassword(form);
           //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
     }

        [TestMethod()]
        public void CustomerHistoryTest()
        {
            //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            CustomerRepo.Setup(r => r.GetCustomerByID(bankers[0].ID)).Returns(customers[0]);
            //Act
            var result = BankController.CustomerHistory(customers[0].ID) as RedirectToRouteResult;
            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Transactions");
            Assert.AreEqual(result.RouteValues["controller"], "Transaction");
      }

        [TestMethod()]
        public void CustomerTrensferTest()
        {
            //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            CustomerRepo.Setup(r => r.GetCustomerByID(bankers[0].ID)).Returns(customers[0]);
            //Act
            var result = BankController.CustomerTrensfer(customers[0].ID) as RedirectToRouteResult;
            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Transfer");
            Assert.AreEqual(result.RouteValues["controller"], "Transaction");
     }

        [TestMethod()]
        public void CustomerRibTest()
        {
            //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            CustomerRepo.Setup(r => r.GetCustomerByID(bankers[0].ID)).Returns(customers[0]);
            //Act
            var result = BankController.CustomerRib(customers[0].ID) as RedirectToRouteResult;
            //Assert
            Assert.AreEqual(result.RouteValues["action"], "PrintRIB");
            Assert.AreEqual(result.RouteValues["controller"], "Customer");
       }

        [TestMethod()]
        public void AddAccountTest()
        {
            //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            CustomerRepo.Setup(r => r.GetCustomerByID(bankers[0].ID)).Returns(customers[0]);
            //Act
            var result = BankController.AddAccount(customers[0].ID);
            //Assert
            Assert.IsInstanceOfType(result,typeof(ViewResult));
            }

        [TestMethod()]
        public void AddAccountTest1()
        {
             //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            AddAccountForm form = new AddAccountForm();
            form.BIC="23YT";
            form.IBAN ="FR9552102565062826225126616";
            CustomerRepo.Setup(r => r.GetCustomerByID(bankers[0].ID)).Returns(customers[0]);
            CustomerRepo.Setup(r => r.Save());
            //Act
            var result = BankController.AddAccount(form) as RedirectToRouteResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod()]
        public void ValideCustomerForBankerTest()
        {
            //Arrange
            BankController.Session[Utils.SessionBanker] = bankers[0];
            CustomerRepo.Setup(r => r.GetCustomerByID(bankers[0].ID)).Returns(customers[0]);
            //Act
            var result = BankController.ValideCustomerForBanker(bankers[0].ID);
            //Assert
            Assert.AreEqual(result.ID,bankers[0].ID);

        }
    }
}