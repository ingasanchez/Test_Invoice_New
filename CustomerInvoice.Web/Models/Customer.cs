//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CustomerInvoice.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        [Required]
        [Display (Name="Customer Name")]
        public string CustName { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Adress { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        [Display(Name = "Customer Type")]
        public int CustomerTypeId { get; set; }
    
        public virtual CustomerType CustomerType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
