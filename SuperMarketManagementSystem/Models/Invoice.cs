using System.ComponentModel.DataAnnotations;

namespace SuperMarketManagementSystem.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        [Display(Name = "Cashier")]
        public string CreatorId { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Display(Name = "Date & Time")]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Display(Name = "Qty Sold")]
        public int QuantitySold { get; set; }
        public int QtyLeft { get; set; }

        [Display(Name = "Amount")]
        public double InvoiceAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public virtual Product? Product { get; set; }
    }
}

namespace SuperMarketManagementSystem
{
    public enum InvoiceStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
