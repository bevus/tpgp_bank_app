using BankApp.Models;
using System;
using System.Web.Mvc;

namespace BankApp.Controllers
{
    public class CustomerController : Controller
    {
        private BankContext db = new BankContext();
        private ICustomerRepo customerRepo;
        private IAccountRepo accountRepo;
        public CustomerController()
        {
            customerRepo = new EFCustomerRepo(db);
            accountRepo = new EFAccountRepo(db);
        }        
        // GET: Customer
        public ActionResult Index()
        {
            if (Session[Utils.SessionCustomer] == null) return RedirectToAction("Index", "Home");
            Session[Utils.SessionTransactionCustomer] = Session[Utils.SessionCustomer] as Customer;
            return RedirectToAction("Transactions", "Transaction");
        }

        public ActionResult PrintRIB()
        {
            if (Session[Utils.SessionCustomer] == null) return RedirectToAction("Index", "Home");
            var customer = Session[Utils.SessionCustomer] as Customer;
            var account = accountRepo.GetAccountByID(2);
            var tuple = new Tuple<Account, Customer>(account, customer);
            return View(tuple);
        }

        public ActionResult SimulateCredit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PrintPDF([Bind(Include = "Id,Solde")] Account account)
        {
            if (Session[Utils.SessionCustomer] == null) return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        public ActionResult Logout()
        {
            Session[Utils.SessionCustomer] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}