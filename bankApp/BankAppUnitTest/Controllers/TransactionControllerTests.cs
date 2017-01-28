using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.Models;

namespace BankApp.Controllers.Tests
{
    [TestClass()]
    public class TransactionControllerTests
    {
        public TransactionController TransactionController;
        public ICustomerRepo customerRepo;
        public IAccountRepo accountRepo;
        public ITransactionRepo transactionRepo;
        [TestInitialize]
        public void Setup()
        {
            
        }
        [TestMethod()]
        public void TransactionControllerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IndexTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TransferTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TransactionsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TransferTest1()
        {
            Assert.Fail();
        }
    }
}