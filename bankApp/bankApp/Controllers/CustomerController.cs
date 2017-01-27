using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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

            return View(customerRepo.GetCustomers());
        }

        public ActionResult PrintRIB()
        {
            db.Configuration.LazyLoadingEnabled = false;

            Account account = db.Accounts.Find(2);
            //Include("Customer").
            Customer customer = db.Customers.Find(account.Owner_ID);
            var tuple = new Tuple<Account, Customer>(account, customer);
            return View(tuple);
        }

        public ActionResult SimulateCredit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrintPDF([Bind(Include = "Id,Solde")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }
    }
}