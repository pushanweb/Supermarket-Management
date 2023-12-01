using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarketManagementSystem.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Product Code")]
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Price Per Unit (Taka)")]
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Invoice>? Invoice { get; set; }
    }
}
