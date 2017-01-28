using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BankApp.Controllers
{
    public class CustomerController : Controller
    {
        private BankContext db = new BankContext();
        private ICustomerRepo customerRepo;
        private IAccountRepo accountRepo;

        public Customer ConnectedCustomer
        {
            get
            {
                if (Session?[Utils.SessionCustomer] == null)
                    return null;
                try
                {
                    return Session[Utils.SessionCustomer] as Customer;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set { Session[Utils.SessionCustomer] = value; }
        }

        public Customer RIBCustomer
        {
            get
            {
                if (Session?[Utils.SessionRIBCustomer] == null)
                    return null;
                try
                {
                    return Session[Utils.SessionRIBCustomer] as Customer;
                }
                catch (Exception)
                { return null; }
            }
            set { Session[Utils.SessionRIBCustomer] = value; }
        }

        public CustomerController()
        {
            customerRepo = new EFCustomerRepo(db);
            accountRepo = new EFAccountRepo(db);
        }

        public CustomerController(ICustomerRepo customerRepo, IAccountRepo accountRepo)
        {
            this.customerRepo = customerRepo;
            this.accountRepo = accountRepo;
        }

        // GET: Customer
        public ActionResult Index()
        {
            var customer = ConnectedCustomer;
            if (customer == null) return RedirectToAction("Index", "Home");
            Session[Utils.SessionTransactionCustomer] = customer;
            return RedirectToAction("Transactions", "Transaction");
        }

        public ActionResult PrintRIBCustomer()
        {
            var customer = ConnectedCustomer;
            if (customer == null) return RedirectToAction("Index", "Home");
            RIBCustomer = customer;
            return RedirectToAction("PrintRIB");
        }

        public ActionResult PrintRIB()
        {
            if (RIBCustomer == null) return RedirectToAction("Index", "Home");
            return View(accountRepo.GetAccounts().Where(a => a.Owner_ID == RIBCustomer.ID).ToList());
        }
        public ActionResult PrintRIBByAccount(int? id)
        {
            if (id == null || RIBCustomer == null)
                return Json(new {error = true, message = "Compte inconnu"});
            var account = accountRepo.GetAccountByID((int)id);
            if(account == null)
                return Json(new { error = true, message = "Compte inconnu" });
            if(account.Owner_ID != RIBCustomer.ID)
                return Json(new { error = true, message = "Vous n'ests pas le propietaire de ce compte" });
            return Json(new
            {
                RIBCustomer.FirstName,
                RIBCustomer.LastName,
                RIBCustomer.AccountNumber,
                account.BIC,
                account.IBAN
            });
      }

        public ActionResult SimulateCredit()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SimulateCredit(SimulateCredit form)
        {
            if(ModelState.IsValid)
            {
                var client = new bankApp.CheckCreditServiceReference.Service1Client();
                var requestedAmount = form.RequestedAmount;
                var householdIncomes = form.HouseholdIncomes;
                var contribution = form.Contribution;
                var duration = form.Duration;
                if (client.CheckCredit(requestedAmount, householdIncomes, contribution,duration))
                {
                    TempData["notice"] = "Votre demende sera acceptée";
                    return RedirectToAction("SimulateCredit", "Customer");
                }
                TempData["error"] = "Votre demende ne sera pas acceptée";
                return RedirectToAction("SimulateCredit", "Customer");
            }
            return View();
        }

        public ActionResult Logout()
        {
            ConnectedCustomer = null;
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (ConnectedCustomer == null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordCustomerForm form)
        {
            if (ConnectedCustomer == null) return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var customer = ConnectedCustomer;
                if(customer.Password != form.OldPassword)
                    ModelState.AddModelError("OldPassword", "Mot de passe incerrect");
                if (ModelState.IsValid)
                {
                    customer.Password = form.NewPassword;
                    customerRepo.UpdateCustomer(customer);
                    customerRepo.Save();
                    Session[Utils.SessionCustomer] = customer;
                    TempData["notice"] = "Mot de passe mis à jour";
                    return RedirectToAction("Index", "Customer");
                }
            }
            return View();
        }
    }
}