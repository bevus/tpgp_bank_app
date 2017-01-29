using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web.Mvc;
using BankApp.Models;
using Moq;
using MvcContrib.TestHelper;

namespace BankApp.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        public HomeController HomeController;
        public Mock<ICustomerRepo> CustomerRepo;
        public Mock<IBankerRepo> BankerRepo;

        public List<Customer> customers;
        public List<Banker> bankers;

        [TestInitialize]
        public void Setup()
        {
            CustomerRepo = new Mock<ICustomerRepo>();
            BankerRepo = new Mock<IBankerRepo>();
            HomeController = new HomeController(
                CustomerRepo.Object,
                BankerRepo.Object
            );

            var controllerBuilder = new TestControllerBuilder();
            controllerBuilder.InitializeController(HomeController);

            customers = new List<Customer>
            {
                new Customer { ID = 1, AccountNumber = "1234", Password = "123456"},
                new Customer { ID = 2, AccountNumber = "2345", Password = "123456"}
            };

            bankers = new List<Banker>
            {
                new Banker { ID = 1, Mail = "banker1@bank.fr", Password = "password"},
                new Banker { ID = 2, Mail = "banker2@bank.fr", Password = "password"}
            };
        }
        [TestMethod()]
        public void HomeControllerTest()
        {
            //Arrange
            //Act
            //Assert
            Assert.IsNotNull(HomeController);
        }

        [TestMethod()]
        public void IndexTest()
        {
            //Arrange
            //Act
            var result = HomeController.Index() as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod()]
        public void GetLoginCustomerTest()
        {
            //Arrange
            //Act
            var result = HomeController.LoginCustomer() as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(result.ViewName, "");
        }


        [TestMethod()]
        public void PostLoginCustomerInvalideModelReturnView()
        {
            //Arrange
            HomeController.ModelState.AddModelError("CustomerNumber", "");
            //Act
            var result = HomeController.LoginCustomer(new LoginCustomerForm()) as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(result.ViewName, "");
        }
        [TestMethod]
        public void PostLoginCustomerInvalideCredentialReturnView()
        {
            // Arrange
            CustomerRepo.Setup(r => r.GetCustomers()).Returns(customers);
            // Act
            var result = HomeController.LoginCustomer(new LoginCustomerForm
            {
                CustomerNumber = "1234",
                Password = "445646"
            }) as ViewResult;
            //Assret
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(result.ViewName, "");
            Assert.IsNull(HomeController.Session[Utils.SessionCustomer]);
        }

        [TestMethod]
        public void PostLoginCustomerValideCredentialRedirect()
        {
            // Arrange
            CustomerRepo.Setup(r => r.GetCustomers()).Returns(customers);
            // Act
            var result = HomeController.LoginCustomer(new LoginCustomerForm
            {
                CustomerNumber = "1234",
                Password = "123456"
            }) as RedirectToRouteResult;
            var sessionCustomer = HomeController.Session[Utils.SessionCustomer] as Customer;
            //Assret
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["controller"], "Customer");
            Assert.IsNotNull(sessionCustomer);
            Assert.IsNull(HomeController.Session[Utils.SessionBanker]);
            Assert.IsNull(HomeController.Session[Utils.SessionAddAccountCustomer]);
            Assert.IsNull(HomeController.Session[Utils.SessionRIBCustomer]);
            Assert.IsNull(HomeController.Session[Utils.SessionTransactionCustomer]);
            Assert.AreEqual(sessionCustomer.ID, customers[0].ID);
        }
        [TestMethod()]
        public void GetLoginBankerTest()
        {
            //Arrange
            //Act
            var result = HomeController.LoginBanker() as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod()]
        public void PostLoginBankerInvalideCredentialReturnView()
        {
            // Arrange
            BankerRepo.Setup(r => r.GetBankers()).Returns(bankers);
            // Act
            var result = HomeController.LoginBanker(new LoginBankerForm
            {
                Email = "john.doe@bank.fr",
                Password = "445646"
            }) as ViewResult;
            //Assret
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(result.ViewName, "");
            Assert.IsNull(HomeController.Session[Utils.SessionBanker]);
        }

        [TestMethod]
        public void PostLoginBankerValideCredentialRedirect()
        {
            // Arrange
            BankerRepo.Setup(r => r.GetBankers()).Returns(bankers);
            // Act
            var result = HomeController.LoginBanker(new LoginBankerForm()
            {
                Email = "banker1@bank.fr",
                Password = "password"
            }) as RedirectToRouteResult;
            var sessionBanker = HomeController.Session[Utils.SessionBanker] as Banker;
            //Assret
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["controller"], "Banker");
            Assert.IsNotNull(sessionBanker);
            Assert.IsNull(HomeController.Session[Utils.SessionCustomer]);
            Assert.IsNull(HomeController.Session[Utils.SessionAddAccountCustomer]);
            Assert.IsNull(HomeController.Session[Utils.SessionRIBCustomer]);
            Assert.IsNull(HomeController.Session[Utils.SessionTransactionCustomer]);
            Assert.AreEqual(sessionBanker.ID, bankers[0].ID);
        }
    }
}