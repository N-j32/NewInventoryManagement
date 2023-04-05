using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class Product
    {
        [Key]
        [Required(ErrorMessage = "* Id is Required")]
        public int ProductId { get; set; }
        [StringLength(6)]
        [Required(ErrorMessage = "* Code is Required")]
        public string Code { get; set; }

        [Required(ErrorMessage = "* Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Description is Required")]
        [StringLength(60)]
        public string Description { get; set; }

        [Required(ErrorMessage = "* Price is Required")]
        [Column(TypeName ="smallmoney")]
        
        public decimal Cost { get; set; }

        public int Quantity { get; set; }
        [Required]
        [ForeignKey("Units")]
        [Display(Name ="Unit")]
        public int UnitId { get; set; }
        public virtual Unit Units { get; set; }

        
        [ForeignKey("Brands")]
        [Display(Name = "Brand")]
        public int? BrandId{ get; set; }
        public virtual Brand Brands { get; set; }

        [ForeignKey("Catrgories")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public virtual Category Categories { get; set; }


        [ForeignKey("ProductGroups")]
        [Display(Name = "ProductGroup")]
        public int? ProductGroupId { get; set; }
        public virtual ProductGroup ProductGroups { get; set; }
    }
}
