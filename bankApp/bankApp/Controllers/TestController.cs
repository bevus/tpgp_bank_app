using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankApp.Models;

namespace BankApp.Controllers
{
    public class TestController : Controller
    {
        private ICustomerRepo customerRepo;
        private IAccountRepo accountRepo;
        private ITransactionRepo transactionRepo;

        private BankContext context;

        public TestController()
        {
            context = new BankContext();
            context.Configuration.LazyLoadingEnabled = false;
            transactionRepo = new EFTransactionRepo(context);
            customerRepo = new EFCustomerRepo(context);
            accountRepo = new EFAccountRepo(context);
        }
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InsertTestTransaction()
        {
            if (transactionRepo.GetTransactions().ToList().Count > 0)
                throw new Exception("table transacion non vide");
            transactionRepo.InsertTransaction(new AccountToAcountTransaction()
            {
                Account = accountRepo.GetAccountByID(9),
                Amount = 5000,
                Date = DateTime.Now,
                TransactionType = TransactionType.CREDIT,
                Title = "virement - hacene kedjar"
            });

            transactionRepo.InsertTransaction(new AccountToAcountTransaction()
            {
                Account = accountRepo.GetAccountByID(9),
                Title = "virement",
                Amount = 1260,
                Date = DateTime.Now,
                TransactionType = TransactionType.DEBIT
            });

            transactionRepo.InsertTransaction(new AgencyTransaction
            {
                Account = accountRepo.GetAccountByID(9),
                Agency = "LCL Paris 5",
                Amount = 5000,
                Date = DateTime.Now,
                TransactionType = TransactionType.CREDIT
            });
            transactionRepo.InsertTransaction(new AgencyTransaction
            {
                Account = accountRepo.GetAccountByID(9),
                Agency = "LCL Paris 6",
                Amount = 500,
                Date = DateTime.Now,
                TransactionType = TransactionType.DEBIT
            });

            transactionRepo.InsertTransaction(new AgencyTransaction
            {
                Account = accountRepo.GetAccountByID(9),
                Agency = "LCL Paris 7",
                Amount = 5000,
                Date = DateTime.Now,
                TransactionType = TransactionType.CREDIT
            });

            transactionRepo.InsertTransaction(new CDTransaction()
            {
                Account = accountRepo.GetAccountByID(9),
                Amount = 520,
                Date = DateTime.Now,
                TransactionType = TransactionType.DEBIT,
                AgencyName = "LCL",
                CashDispanserName = "CDT-1",
                CdType = CDType.INSIDE
            });

            transactionRepo.InsertTransaction(new CDTransaction()
            {
                Account = accountRepo.GetAccountByID(9),
                Amount = 560,
                Date = DateTime.Now,
                TransactionType = TransactionType.DEBIT,
                AgencyName = "BNP",
                CashDispanserName = "CDT-BNP-1",
                CdType = CDType.OUTSIDE
            });

            transactionRepo.Save();
            return RedirectToAction("Transactions", "Transaction");
        }
    }
}