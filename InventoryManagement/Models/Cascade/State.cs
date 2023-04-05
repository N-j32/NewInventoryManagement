using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models.Cascade
{
    public class State
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //foreignkey

        public Country Country { get; set; }
    }
}
