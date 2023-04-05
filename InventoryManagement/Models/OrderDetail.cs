using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailsId { get; set; }

        public int Quantity { get; set; }
        public Decimal Price { get; set; }

        public Decimal TotalAmount { get; set; }

       

        [Required]
        [ForeignKey("Product")]
        [Display(Name = "Product")]
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        [ForeignKey("Order")]
        [Display(Name = "OrderNumber")]
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }

        
    }
}
