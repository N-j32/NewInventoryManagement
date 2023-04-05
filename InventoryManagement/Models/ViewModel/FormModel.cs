using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models.ViewModel
{
    public class FormModel
    {

        public string OrderDate { get; set; }

        public string SupplierId { get; set; }
        public string GrandTotal { get; set; }
        public string ProductId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        


    }

    public class OrderDetail
    {
        public string ProductId { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public Decimal Rate { get; set; }

        public Decimal TotalAmount { get; set; }

        
       
    }
}
