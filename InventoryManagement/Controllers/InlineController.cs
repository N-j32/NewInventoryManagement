using InventoryManagement.Data;
using InventoryManagement.Models;
using InventoryManagement.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Controllers
{
    public class InlineController : Controller
    {
        private readonly ApplicationContext _context;

        public InlineController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = new Order { OrderDate = DateTime.Now };
            var getSupplier = _context.Supplier.ToList();
            SelectList list = new SelectList(getSupplier, "SupplierId", "SupplierName");
            ViewBag.supplierList = list;
            return View(model);
        }
        //[HttpPost]
        //public JsonResult SaveOrder(FormModel item)
        //{
        //    //bool status = false;
            //if (ModelState.IsValid)
            //{

            //    Order order = new Order { OrderDate = item.OrderDate, SupplierId = item.SupplierId };
            //    foreach (var i in item.OrderDetails)
            //    {

            //        order.OrderDetails.Add(i);
            //    }
            //    _context.Order.Add(order);
            //    _context.SaveChanges();
            //    status = true;

            //}
            //else
            //{
            //    status = false;
            //}
            //return Json(new { status = status });
        //}
    }
}
