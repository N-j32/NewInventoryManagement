using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models.ViewModel
{
    public class SearchViewModel
    {
        public Product Product { get; set; }
        public string SearchString { get; set; }
        public string SearchText { get; set; }
        public string searchUnit { get; set; }
        public string searchBrand { get; set; }
    }
}
