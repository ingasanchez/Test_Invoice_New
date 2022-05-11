using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerInvoice.Web.Models;

namespace CustomerInvoice.Web.Controllers
{
    public class CustomersController : Controller
    {
        private Test_InvoiceConnection db = new Test_InvoiceConnection();

        // GET: Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.CustomerType);
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.CustomerTypeId = new SelectList(db.CustomerTypes, "Id", "Description");
            return View();
        }

        // POST: Customers/Create


        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,CustName,Adress,Status,CustomerTypeId")] Customer customer)
        {
            customer.CustomerType = db.CustomerTypes.FirstOrDefault (i=> i.Id == customer.CustomerTypeId);

            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
           }

            ViewBag.CustomerTypeId = new SelectList(db.CustomerTypes, "Id", "Description", customer.CustomerTypeId);
            return View(customer);
        }


        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerTypeId = new SelectList(db.CustomerTypes, "Id", "Description", customer.CustomerTypeId);
            return View(customer);
        }

        // POST: Customers/Edit/5

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,CustName,Adress,Status,CustomerTypeId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerTypeId = new SelectList(db.CustomerTypes, "Id", "Description", customer.CustomerTypeId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
