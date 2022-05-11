using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerInvoice.Web.Models
{
    public class InvoiceView
    {
        public Invoice invoice { get; set; }
        // atributo para el encabezado de los details
        public InvoiceDetail invoiceDetail { get; set; }
        public List<InvoiceDetail>  invoiceDetails { get; set; }
        public int edad { get; set; }
    }
}