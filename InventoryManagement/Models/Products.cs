using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class Products
    {
        public Int32 ProductId { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public String ProductName { get; set; }

        [Required]
        [Display(Name = "Price")]
        public Decimal Price { get; set; }

        [Required]
        [Display(Name = "Category")]
        public Int32 CategoryId { get; set; }

        public String Image { get; set; }

        public String Thumb { get; set; }

        public virtual Category Categories { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
