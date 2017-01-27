using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankApp.Models;

namespace BankApp.Controllers
{
    public class BankersGeneratedController : Controller
    {
        private BankContext db = new BankContext();

        // GET: BankersGenerated
        public ActionResult Index()
        {
            return View(db.Bankers.ToList());
        }

        // GET: BankersGenerated/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banker banker = db.Bankers.Find(id);
            if (banker == null)
            {
                return HttpNotFound();
            }
            return View(banker);
        }

        // GET: BankersGenerated/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BankersGenerated/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Mail,Password")] Banker banker)
        {
            if (ModelState.IsValid)
            {
                db.Bankers.Add(banker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(banker);
        }

        // GET: BankersGenerated/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banker banker = db.Bankers.Find(id);
            if (banker == null)
            {
                return HttpNotFound();
            }
            return View(banker);
        }

        // POST: BankersGenerated/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Mail,Password")] Banker banker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(banker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(banker);
        }

        // GET: BankersGenerated/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banker banker = db.Bankers.Find(id);
            if (banker == null)
            {
                return HttpNotFound();
            }
            return View(banker);
        }

        // POST: BankersGenerated/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Banker banker = db.Bankers.Find(id);
            db.Bankers.Remove(banker);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
