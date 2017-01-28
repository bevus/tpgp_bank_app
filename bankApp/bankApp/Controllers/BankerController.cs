using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BankApp.Controllers
{
    public class BankerController : Controller
    {
        private BankContext db = new BankContext();
        private ICustomerRepo customerRepo;
        private IAccountRepo accountRepo;
        private IBankerRepo bankerRepo;


        public BankerController()
        {
            customerRepo = new EFCustomerRepo(db);
            accountRepo = new EFAccountRepo(db);
            bankerRepo = new EFBankerRepo(db);
        }

        public BankerController(ICustomerRepo customerRepo, IAccountRepo accountRepo, IBankerRepo bankerRepo)
        {
            this.customerRepo = customerRepo;
            this.accountRepo = accountRepo;
            this.bankerRepo = bankerRepo;
        }

        // GET: Customer
        public ActionResult Index()
        {
            if (Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            var banker = Session[Utils.SessionBanker] as Banker;
            return View(customerRepo.GetCustomers().Where(c => c.Banker_ID == banker.ID));
        }

        public ActionResult Create()
        {
            if (Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            return View(new AddCustomerForm
            {
                Bankers = bankerRepo.GetBankers().Select(b => new SelectListItem
                {
                    Value = b.ID.ToString(),
                    Text = b.FirstName + " " + b.LastName
                })
            });
        }

        [HttpPost]
        public ActionResult Create(AddCustomerForm form)
        {
            if (Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                validCreateCustomerForm(form);
                if (ModelState.IsValid)
                {
                    var customer = new Customer
                    {
                        FirstName = form.FirstName,
                        LastName = form.LastName,
                        AccountNumber = form.CustomerNumber,
                        Password = form.Password,
                        Banker_ID = form.BankerID,
                        Accounts = new List<Account>()
                    };
                    var account = new Account
                    {
                        Solde = form.Solde,
                        Owner = customer,
                        BIC = form.BIC,
                        IBAN = form.IBAN
                    };
                    accountRepo.InsertAccount(account);
                    customer.Accounts.Add(account);
                    customerRepo.InsertCustomer(customer);
                    customerRepo.Save();
                    return RedirectToAction("Index");
                }
            }

            return View(new AddCustomerForm
            {
                Bankers = bankerRepo.GetBankers().Select(b => new SelectListItem
                {
                    Value = b.ID.ToString(),
                    Text = b.FirstName + " " + b.LastName
                })
            });
        }



        // GET: CustomersGenerated/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = customerRepo.GetCustomerByID((int) id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: CustomersGenerated/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            Customer customer = customerRepo.GetCustomerByID(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: CustomersGenerated/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = customerRepo.GetCustomerByID((int) id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Banker_ID = new SelectList(db.Bankers, "ID", "ID", customer.Banker_ID);
            return View(customer);
        }

        // POST: CustomersGenerated/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Password,Banker_ID")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customerRepo.UpdateCustomer(customer);
                customerRepo.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Banker_ID = new SelectList(db.Bankers, "ID", "ID", customer.Banker_ID);
            return View(customer);
        }

        private void validCreateCustomerForm(AddCustomerForm form)
        {
            if (form.Solde < 50)
                ModelState.AddModelError("Solde", "le solde initial doit etre superieur à 50€");
            try
            {
                customerRepo.GetCustomers().First(c => c.AccountNumber == form.CustomerNumber);
                ModelState.AddModelError("CustomerNumber", "ce numéro utilisateur est déja attribué");
            }
            catch (Exception){}
            validCreateAccount(new AddAccountForm {BIC = form.BIC, IBAN = form.IBAN});
            
        }

        private void validCreateAccount(AddAccountForm form)
        {
            try
            {
                accountRepo.GetAccounts().First(c => c.IBAN == form.IBAN);
                ModelState.AddModelError("IBAN", "IBAN déja attribué");
            }
            catch (Exception) { }
        }

        public ActionResult Logout()
        {
            Session[Utils.SessionBanker] = null;
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordBankerForm form)
        {
            if (Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var banker = Session[Utils.SessionBanker] as Banker;
                if (banker.Password != form.OldPassword)
                    ModelState.AddModelError("OldPassword", "Mot de passe incerrect");
                if (ModelState.IsValid)
                {
                    banker.Password = form.NewPassword;
                    bankerRepo.UpdateBanker(banker);
                    bankerRepo.Save();
                    Session[Utils.SessionBanker] = banker;
                    TempData["notice"] = "Mot de passe mis à jour";
                    return RedirectToAction("Index", "Banker");
                }
            }
            return View();
        }

        public ActionResult CustomerHistory(int? id)
        {
            if (Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            var customer = ValideCustomerForBanker(id);
            if (customer == null)
                return RedirectToAction("Index");
            Session[Utils.SessionTransactionCustomer] = customer;
            return RedirectToAction("Transactions", "Transaction");
        }

        public ActionResult CustomerTrensfer(int? id)
        {
            if (Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            var customer = ValideCustomerForBanker(id);
            if (customer == null)
                return RedirectToAction("Index");
            Session[Utils.SessionTransactionCustomer] = customer;
            return RedirectToAction("Transfer", "Transaction");
        }

        private Customer ValideCustomerForBanker(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "compte spécifié";
                return null;
            }
            var banker = Session[Utils.SessionBanker] as Banker;
            var customer = customerRepo.GetCustomerByID((int) id);
            if (customer == null)
            {
                TempData["error"] = "compte invalide";
                return null;
            }
            if (customer.Banker_ID != banker.ID)
            {
                TempData["error"] = "ce client est géré par un autre banquier";
                return null;
            }
            return customer;
        }

        public ActionResult CustomerRib(int? id)
        {
            if (Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            var customer = ValideCustomerForBanker(id);
            if (customer == null)
                return RedirectToAction("Index");
            Session[Utils.SessionRIBCustomer] = customer;
            return RedirectToAction("PrintRIB", "Customer");
        }
        [HttpGet]
        public ActionResult AddAccount(int? id)
        {
            if(Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            var customer = ValideCustomerForBanker(id);
            if (customer == null)
                return RedirectToAction("Index");
            ViewBag.customer = customer;
            Session[Utils.SessionAddAccountCustomer] = customer;
            return View();
        }
        [HttpPost]
        public ActionResult AddAccount(AddAccountForm form)
        {
            if (Session[Utils.SessionBanker] == null) return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var customer = Session[Utils.SessionAddAccountCustomer] as Customer;
                if (customer == null)
                {
                    TempData["error"] = "client inconnu";
                    return RedirectToAction("Index");
                }
                validCreateAccount(form);
                if (ModelState.IsValid)
                {
                    customer = customerRepo.GetCustomerByID(customer.ID);
                    customer.Accounts.Add(new Account
                    {
                        BIC = form.BIC,
                        IBAN = form.IBAN,
                        Owner = customer,
                        Solde = 0d
                    });
                    customerRepo.Save();
                    TempData["notice"] = "Compte ajouté";
                    Session[Utils.SessionAddAccountCustomer] = null;
                    return RedirectToAction("Index");
                }
                ViewBag.customer = customer;
            }
            return View();
        }
    }
}