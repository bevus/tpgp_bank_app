using System;
using System.Linq;
using System.Web.Mvc;
using BankApp.Models;

namespace BankApp.Controllers
{
    public class HomeController : Controller
    {
        private ICustomerRepo customerRepo;
        private IBankerRepo bankerRepo;
        private BankContext context;

        public HomeController()
        {
            context = new BankContext();
            customerRepo = new EFCustomerRepo(context);
            bankerRepo = new EFBankerRepo(context);
        }

        public HomeController(ICustomerRepo customerRepo, IBankerRepo bankerRepo, BankContext context)
        {
            this.customerRepo = customerRepo;
            this.bankerRepo = bankerRepo;
            this.context = context;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LoginCustomer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginCustomer(LoginCustomerForm form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customers = customerRepo.GetCustomers();
                    var customer =
                        customers
                            .First(c => form.CustomerNumber == c.AccountNumber);
                    if(customer.Password != form.Password)
                        ModelState.AddModelError("password", "mot de passe incorrect");
                    if (ModelState.IsValid)
                    {
                        Session.Clear();
                        Session[Utils.SessionCustomer] = customer;
                        return RedirectToAction("Index", "Customer");
                    }
                }
                catch (Exception)
                {

                    ModelState.AddModelError("CustomerNumber", "numéro client inconnu");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult LoginBanker()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginBanker(LoginBankerForm form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var banker =
                        bankerRepo.GetBankers()
                            .First(c => c.Mail == form.Email);
                    if (banker.Password != form.Password)
                        ModelState.AddModelError("password", "mot de passe incorrect");
                    if (ModelState.IsValid)
                    {
                        Session.Clear();
                        Session[Utils.SessionBanker] = banker;
                        return RedirectToAction("Index", "Banker");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("CustomerNumber", "numéro client inconnu");
                }
            }
            return View();
        }
    }
}