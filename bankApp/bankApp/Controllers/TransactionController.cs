using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BankApp.Models;

namespace BankApp.Controllers
{
    public class TransactionController : Controller
    {
        private ICustomerRepo customerRepo;
        private IAccountRepo accountRepo;
        private ITransactionRepo transactionRepo;

        private BankContext context;

        public TransactionController()
        {   
            context = new BankContext();
            transactionRepo = new EFTransactionRepo(context);
            customerRepo = new EFCustomerRepo(context);
            accountRepo = new EFAccountRepo(context);
        }

        public TransactionController(ICustomerRepo customerRepo, IAccountRepo accountRepo, ITransactionRepo transactionRepo, BankContext context)
        {
            this.customerRepo = customerRepo;
            this.accountRepo = accountRepo;
            this.transactionRepo = transactionRepo; 
            this.context = context;
        }

        public Customer TransactionCustomer
        {
            get
            {
                if (Session?[Utils.SessionTransactionCustomer] == null)
                    return null;
                try
                {
                    return Session[Utils.SessionTransactionCustomer] as Customer;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set { Session[Utils.SessionTransactionCustomer] = value; }
        }

        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Transfer()
        {
            if (TransactionCustomer == null) return RedirectToAction("Index", "Home");
            return View(getSelectableAccounts(TransactionCustomer));
        }

        public ActionResult Transactions()
        {
            if (TransactionCustomer == null)
            {
                TempData["error"] = "Client inconnu";
                return RedirectToAction("Index", "Home");
            };
            var customer = TransactionCustomer;
            ViewBag.Solde = 0;
            foreach (var account in customer.Accounts)
            {
                ViewBag.Solde += accountRepo.GetAccountByID(account.ID).Solde;
            }
            var accountsId = customer.Accounts.Select(a => a.ID);
            return View(transactionRepo.GetTransactions().Where(t => 
                    accountsId.Contains(t.Account.ID) &&
                    (DateTime.Now - t.Date).TotalDays < 30
                ).OrderByDescending(t => t.Date).ToList());
        }

        [HttpPost]
        public ActionResult Transfer(TrensferFrom trensferFrom)
        {
            if (TransactionCustomer == null)
            {
                TempData["error"] = "Client inconnu";
                return RedirectToAction("Index", "Home");
            }
            var customer = customerRepo.GetCustomerByID(TransactionCustomer.ID);
            if (ModelState.IsValid)
            {
                var sourceAccount = accountRepo.GetAccountByID(trensferFrom.SourceAccount);
                ValideTrensfer(trensferFrom, customer, sourceAccount);
                if (ModelState.IsValid)
                {
                    var accounts = accountRepo.GetAccounts();
                    var destinationAccount = accounts.FirstOrDefault(account => account.IBAN == trensferFrom.IBAN);
                    var sourceAtaTransaction = new AccountToAcountTransaction
                    {
                        Account = sourceAccount,
                        Source = sourceAccount,
                        Destination = destinationAccount,
                        Date = DateTime.Now,
                        Amount = trensferFrom.Amount,
                        TransactionType = TransactionType.DEBIT,
                        Title = trensferFrom.Label + " - " + trensferFrom.DestinationFullName
                    };
                    transactionRepo.InsertTransaction(sourceAtaTransaction);
                    sourceAccount.Debit(trensferFrom.Amount);
                    if (destinationAccount != null)
                    {
                        var destinationAtaTransaction = new AccountToAcountTransaction
                        {
                            Account = destinationAccount,
                            Date = DateTime.Now,
                            Amount = trensferFrom.Amount,
                            TransactionType = TransactionType.CREDIT,
                            Title = trensferFrom.Label + " - " + trensferFrom.DestinationFullName
                        };
                        transactionRepo.InsertTransaction(destinationAtaTransaction);
                        destinationAccount.Credit(trensferFrom.Amount);
                    }
                    TempData["notice"] = "virement enregistré";
                    accountRepo.Save();
                    return RedirectToAction("Index", "Customer");
                }
            }
            return View(getSelectableAccounts(customer));
        }

        private void ValideTrensfer(TrensferFrom trensferFrom, Customer customer, Account sourceAccount)
        {
            if (trensferFrom.Amount <= 0 || trensferFrom.Amount > 5000)
                ModelState.AddModelError("Amount", "Montant invalid : le montant doit etre compris entre 1 € et 5000 €");
            if (!customer.Accounts.Exists(ac => ac.ID == trensferFrom.SourceAccount))
                ModelState.AddModelError("SourceAcount", "Compte invalide");
            if (customer.Accounts.Single(a => a.ID == trensferFrom.SourceAccount).IBAN == trensferFrom.IBAN)
                ModelState.AddModelError("IBAN", "Compte source et distination son les memes");
            if (sourceAccount.Solde - trensferFrom.Amount < 0)
                ModelState.AddModelError("Amount", "Solde insufissant pour effectuer ce virement");
        }   

        private TrensferFrom getSelectableAccounts(Customer customer)
        {
            var accounts = customer.Accounts.Select(account => new SelectListItem
            {
                Text = account.ID.ToString(), Value = account.ID.ToString()
            }).ToList();
            return new TrensferFrom { AccountsId = accounts };
        }
    }
}