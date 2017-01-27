using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
               
                customer.Accounts.Add(account);

                customerRepo.InsertCustomer(customer);
                customerRepo.Save();

                return RedirectToAction("Index");
            }

            return View(customer);
        }



        // GET: CustomersGenerated/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }
        Customer customer = customerRepo.GetCustomerByID((int)id);
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
            Customer customer = customerRepo.GetCustomerByID((int)id);
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


    }
}