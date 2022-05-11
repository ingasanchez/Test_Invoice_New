using CustomerInvoice.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace CustomerInvoice.Web.Controllers
{
    public class InvoiceViewController : Controller
    {
        private Test_InvoiceConnection db = new Test_InvoiceConnection();

        public delegate Invoice totalOf (InvoiceView inv);
        public totalOf Settotal;
        //Constructor

        public InvoiceViewController()
        {
             Settotal = this.getTotals;
        }

        // GET: InvoiceView
        public ActionResult NewInvoice()
        {
            var invoiceView = new InvoiceView();
            invoiceView.invoice = new Invoice();
            invoiceView.invoiceDetails = new List<InvoiceDetail>();

            
            // guardar la lista de detalle en la sesión para que no se me pierdan los objetos agregados.
            Session["invoiceView"] = invoiceView;

            var list = db.Customers.ToList();
            list.Add(new Customer { Id = 0, CustName = "[Select Customer]" });
            list = list.OrderBy(a => a.CustName).ToList();
            ViewBag.CustomerId = new SelectList(list, "Id", "CustName");

            return View(invoiceView);
        }


        // Save the invoice


        [HttpPost]
        public ActionResult NewInvoice(InvoiceView invoiceView)
        {
            invoiceView = Session["invoiceView"] as InvoiceView;

            var customerId = int.Parse(Request["CustomerId"]);
            var billingDate = DateTime.Parse(Request["BillingDate"]);

            if (customerId == 0)
            {
                callViewBag();
                ViewBag.Error = "Customer must be selected.";

                return View(invoiceView);

            }

            // if customer not exist.
            var customer = db.Customers.Find(customerId);

            if (customer == null)
            {
                callViewBag();
                ViewBag.Error = "Customer doesn't exist.";

                return View(invoiceView);

            }

            // Validate BillingDate

            if (billingDate == null)
            {
                callViewBag();
                ViewBag.Error = "Billing date must be specified.";

                return View(invoiceView);
            }

            // Validate if not Add Details

            if (invoiceView.invoiceDetails.Count == 0)
            {
                callViewBag();
                ViewBag.Error = "Must add at least one detail.";

                return View(invoiceView);
            }


            // Save de invoice
            // Transaction Manager

            int invoinceId = 0;

            using (var trans = db.Database.BeginTransaction()) 
            {
                try
                {
                    /*var invoice = new Invoice
                    {
                        CustomerId = customerId,
                        BillingDate = billingDate,
                        SubTotal = 0,
                        TotalItbis = 0,
                        Total = 0
                    };*/

                    invoiceView.invoice.CustomerId = customerId;
                    invoiceView.invoice.BillingDate = billingDate;

                    db.Invoices.Add(invoiceView.invoice);
                    db.SaveChanges();

                    invoinceId = db.Invoices.ToList().Select(i => i.Id).Max();

                    // Save the Details tetet

                    foreach (var item in invoiceView.invoiceDetails)
                    {
                        var detail = new InvoiceDetail
                        {
                            Qty = item.Qty,
                            Price = item.Price,
                            SubTotal = item.SubTotal,
                            TotalItbis = item.TotalItbis,
                            InvoiceId = invoinceId, 
                            Total = item.Total
                        };
                        db.InvoiceDetails.Add(detail);
                    }

                    db.SaveChanges();

                    trans.Commit();

                }  catch (Exception ex)
                {
                    trans.Rollback();
                    ViewBag.Error = "Error: " + ex.Message;
                    callViewBag();
                    return View(invoiceView);
                }
               
            }


            ViewBag.Message = String.Format("Invoice: {0}, Succesfully Saved.", invoinceId);

            callViewBag();

            invoiceView = new InvoiceView();
            invoiceView.invoice = new Invoice();
            invoiceView.invoiceDetails = new List<InvoiceDetail>();
            Session["invoiceView"] = invoiceView;

            return View(invoiceView);

        }

        public ActionResult AddDetail ()
        {
            return View();
        }

        public void callViewBag()
        {
            var list = db.Customers.ToList();
            list.Add(new Customer { Id = 0, CustName = "[Select Customer]" });
            list = list.OrderBy(a => a.CustName).ToList();
            ViewBag.CustomerId = new SelectList(list, "Id", "CustName");
        }

        [HttpPost]
        public ActionResult AddDetail(InvoiceDetail invoiceDetail )
        {
            var invoiceView = Session["invoiceView"] as InvoiceView;

            if (invoiceDetail.Qty <= 0 )
            {
                callViewBag();
                ViewBag.Error = "Quantity must be greater than zero.";

                return View(invoiceDetail);
            }


            if (invoiceDetail.Price <= 0)
            {
                callViewBag();
                ViewBag.Error = "Price must be greater than zero.";

                return View(invoiceDetail);
            }


            var totalItbis = (Decimal.ToDouble(invoiceDetail.Qty) * Decimal.ToDouble(invoiceDetail.Price)) * 0.18;
            invoiceDetail.TotalItbis = (decimal)totalItbis;
            invoiceDetail.SubTotal = invoiceDetail.Qty * invoiceDetail.Price;
            invoiceDetail.Total = invoiceDetail.SubTotal + (decimal)totalItbis;


            invoiceDetail = new InvoiceDetail
            {
                Qty = invoiceDetail.Qty,
                Price = invoiceDetail.Price,
                TotalItbis = invoiceDetail.TotalItbis,
                SubTotal = invoiceDetail.SubTotal,
                Total = invoiceDetail.Total

            };

            invoiceView.invoiceDetails.Add(invoiceDetail);

            Settotal(invoiceView);

            callViewBag(); /**/

            return View("NewInvoice", invoiceView);
        }

        // Sum Totals

        public Invoice getTotals (InvoiceView invoiceView)
        {
            var invoice = invoiceView.invoice;
            invoice.SubTotal = 0M;
            invoice.TotalItbis = 0M;
            invoice.Total = 0M;

            foreach ( var ls in invoiceView.invoiceDetails)
            {
                invoice.SubTotal += ls.SubTotal;
                invoice.Total += ls.Total;
                invoice.TotalItbis += ls.TotalItbis;
            }

            return invoice;

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