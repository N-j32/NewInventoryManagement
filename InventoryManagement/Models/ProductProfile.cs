﻿using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class ProductProfile
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "* Name is Required")]

        public string Name { get; set; }
        [Required(ErrorMessage = "* Description is Required")]
        [MaxLength(50)]
        public string Description { get; set; }
    }
}
