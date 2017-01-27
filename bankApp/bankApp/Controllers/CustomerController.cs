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

        [HttpPost]
        [ValidateAntiForgeryToken]
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