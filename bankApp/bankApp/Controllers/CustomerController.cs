using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bankApp.Controllers
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

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account item1, Customer item2)
        {
            Customer customer = new Customer();
            if (ModelState.IsValid)
            {
                customer.FirstName = item2.FirstName;
                customer.LastName = item2.LastName;
                customer.Password = item2.Password;
                customer.Banker_ID = item2.Banker_ID;
                Account account = new Account();
                account.Solde = item1.Solde;
                account.Owner = customer;
                accountRepo.InsertAccount(account);
                //accountRepo.Save();
                customer.Accounts.Add(account);

                customerRepo.InsertCustomer(customer);
                customerRepo.Save();

                return RedirectToAction("Index");
            }

            return View(customer);
        }


    }
}