﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class Suppliers
    {
        [Key]
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
