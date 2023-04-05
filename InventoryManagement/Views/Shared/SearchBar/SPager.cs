using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Views.Shared.SearchBar
{
    public class SPager
    {
        //empty constructor
        public SPager()
        {

        }
        public string SearchText { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
