﻿using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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

        // GET: Customer
        public ActionResult Index()
        {

            return View(customerRepo.GetCustomers());
        }

        public ActionResult Create()
        {
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

        public ActionResult PrintRIB()
        {
            db.Configuration.LazyLoadingEnabled = false;

            Account account = db.Accounts.Find(2);
            //Include("Customer").
            Customer customer = db.Customers.Find(account.Owner_ID);
            var tuple = new Tuple<Account, Customer>(account, customer);
            return View(tuple);
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
            try
            {
                accountRepo.GetAccounts().First(c => c.IBAN == form.IBAN);
                ModelState.AddModelError("IBAN", "IBAN déja attribué");
            }
            catch (Exception){}
        }
    }
}