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

        public ActionResult PrintRIBCustomer()
        {
            if (Session[Utils.SessionCustomer] == null) return RedirectToAction("Index", "Home");
            Session[Utils.SessionRIBCustomer] = Session[Utils.SessionCustomer] as Customer;
            return RedirectToAction("PrintRIB");
        }

        public ActionResult PrintRIB()
        {
            if (Session[Utils.SessionRIBCustomer] == null) return RedirectToAction("Index", "Home");
            var customer = Session[Utils.SessionRIBCustomer] as Customer;
            return View(customer.Accounts);
        }
        public ActionResult PrintRIBByAccount(int? id)
        {   var error = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
               var customer = Session[Utils.SessionRIBCustomer] as Customer;
             
               var account =customer.Accounts.Find(c => c.ID==id);
                if (account == null)
                {
                    return HttpNotFound();
                }
                return Json(new
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    CustomerID = customer.ID,
                    AccountNumber = customer.AccountNumber,
                    BIC = account.BIC,
                    IBAN = account.IBAN
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                error = true;
            }

            if (error)
                return Json(new { error = true, message = "adresse mail ou mot de passe incorrect" });
            return Json(new { error = false });
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
                bankApp.CheckCreditServiceReference.Service1Client client = new bankApp.CheckCreditServiceReference.Service1Client();
                int RequestedAmount = form.RequestedAmount;
                int HouseholdIncomes = form.HouseholdIncomes;
                int Contribution = form.Contribution;
                int Duration = form.Duration;
                if (client.CheckCredit(RequestedAmount, HouseholdIncomes, Contribution,Duration))
                {
                    TempData["notice"] = "Votre demende sera acceptée";
                    return RedirectToAction("SimulateCredit", "Customer");
                }
                else
                {
                    TempData["error"] = "Votre demende ne sera pas acceptée";
                    //TempData["notice"] = "@<%= span style = color:Red;> Votre demende ne sera pas acceptée </ span >";
                    return RedirectToAction("SimulateCredit", "Customer");
                }

            }
            return View();
        }

        //public static bool CheckCredit(SimulateCredit form)
        //{
        //    double montantEmprunté = (form.RequestedAmount - form.Contribution) * ((1.05 * form.Duration) / (10 * 0.1));
        //    if ((montantEmprunté / form.Duration) <= (form.HouseholdIncomes * 0.03))
        //       return true;
        //    return false;
        //}

        public ActionResult Logout()
        {
            Session[Utils.SessionCustomer] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session[Utils.SessionCustomer] == null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordCustomerForm form)
        {
            if (Session[Utils.SessionCustomer] == null) return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var customer = Session[Utils.SessionCustomer] as Customer;
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