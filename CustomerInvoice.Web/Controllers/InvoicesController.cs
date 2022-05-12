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
    public class InvoicesController : Controller
    {
        private Test_InvoiceConnection db = new Test_InvoiceConnection();

        // GET: Invoices
        public ActionResult Index()
        {
            var invoices = db.Invoices.Include(i => i.Customer);
            return View(invoices.ToList());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }

           // InvoiceDetail det = db.InvoiceDetails.ToList().Where(i => i.InvoiceId == id);
            return View(db.InvoiceDetails.ToList().Where(i => i.InvoiceId == id));
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
