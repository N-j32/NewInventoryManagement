﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class Unit
    {
        
        [Key]
        public int UnitId { get; set; }
        [Required(ErrorMessage = "Name is required")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(50)]
        public string Description { get; set; }
    }
}
