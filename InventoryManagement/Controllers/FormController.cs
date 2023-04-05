using InventoryManagement.Data;
using InventoryManagement.Models;
using InventoryManagement.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using OrderDetail = InventoryManagement.Models.OrderDetail;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Net;

namespace InventoryManagement.Controllers
{
    public class FormController : Controller
    {
        private readonly ApplicationContext _context;

        public FormController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var model = new Order { OrderDate = DateTime.Now };
            var getSupplier = _context.Supplier.ToList();
            SelectList list = new SelectList(getSupplier, "SupplierId", "SupplierName");
            ViewBag.supplierList = list;


            //product

            var getProduct = _context.Products.ToList();
            SelectList list1 = new SelectList(getProduct, "ProductId", "Name");
            ViewBag.productList = list1;
            return View(model);

        }
        //public ActionResult SaveOrder(string name, OrderDetails[] orders)
        //{
        //    string result = "Error! Order Is Not Complete!";
        //    if (name != null || orders != null)
        //    {
        //        var orderId = Guid.NewGuid();
        //        Order model = new Order();
        //        model.OrderId = orderId;
        //        // model.Name = name;
        //        //model.Address = address;
        //        model.OrderDate = DateTime.Now;
        //        _context.Order.Add(model);

        //        foreach (var item in orders)
        //        {
        //            var orderDetailsId = Guid.NewGuid();
        //            OrderDetails O = new OrderDetails();
        //            O.OrderDetailsId = orderDetailsId;
        //            O.Product = item.Product;
        //            O.Quantity = item.Quantity;
        //            O.Price = item.Price;
        //            O.TotalAmount = item.TotalAmount;
        //            O.OrderId = orderId;
        //            _context.OrderDetails.Add(O);
        //        }
        //        _context.SaveChanges();
        //        result = "Success! Order Is Complete!";
        //    }
        //    return Json(result, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //}





        // Post action for Save data to database
        [HttpPost]



        public JsonResult SaveOrder([FromBody] FormModel jsonInput)
        {

            bool status = false;
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.ParseExact(jsonInput.OrderDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                //Order order = new Order { OrderDate = dt, SupplierId = int.Parse(jsonInput.SupplierId), GrandTotal = jsonInput.GrandTotal };

                Order model = new Order();

                model.OrderDate = dt;
                model.SupplierId = int.Parse(jsonInput.SupplierId);
                model.GrandTotal = decimal.Parse(jsonInput.GrandTotal);
                model.OrderDetails = new List<OrderDetail>();

                //_context.Order.Add(model);
                //_context.SaveChanges();

                //foreach (var item in jsonInput.OrderDetails)
                //{

                //    order.OrderDetails.Add(item);
                //}

                // _context.Order.Add(order);

                foreach (var item in jsonInput.OrderDetails)
                {
                    var orderDetailsId = Guid.NewGuid();

                    OrderDetail orderDetail = new OrderDetail();

                    //O.Product = item.Product;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.Price = item.Rate;
                    orderDetail.TotalAmount = item.TotalAmount;
                    //orderDetail.OrderId =model.OrderId;
                    orderDetail.ProductId = int.Parse(item.ProductId);
                    // orderDetail.Product = item.Product;
                    // _context.OrderDetails.Add(orderDetail);
                    model.OrderDetails.Add(orderDetail);
                }
                _context.Order.Add(model);

                _context.SaveChanges();
                status = true;
                //TempData["message"] = "Added";
                return Json(new { success = true, responseText = "Your message successfuly sent!" }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }

        
            else
            {
                status = false;
                //TempData["Message"] = " Something is wrong.....";

                return Json(new { success = false, responseText = "Something went wrong." }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
            //return Json(new { status = status });
        }

        [HttpPost]
        // public JsonResult AutoComplete(string id)
        //{

        //     //var fetchname = (from Products in _context.Products
        //     //                 where Products.Name.StartsWith(prefix)
        //     //                 select new
        //     //                 {
        //     //                     label = Products.Name,


        //     //                 }).ToList();
        //     // List<Products> prd = new List<Products>();
        //     ////selecting id and name
        //     // var query = (from item in _context.Products

        //     //              select new { item.ProductId, item.Name });
        //     // var query2 = from l in query.AsEnumerable()
        //     //              group l by l.Name into g
        //     //              select new
        //     //              {
        //     //                  product_id = String.Join(",", g.Select(x => x.ProductId.ToString())),
        //     //                  name = g.Key
        //     //              };

        //     //List<string> fetchname = _context.Products.Where(x => x.Name.Contains(prefix)).Select(x => x.Name).ToList();

        //     //return Json(fetchname, System.Web.Mvc.JsonRequestBehavior.AllowGet);


        //     List<Products> allsearch = _context.Products.Where(x => x.Name.Contains(id)).Select(x => new Products
        //     {
        //         ProductId = x.ProductId,
        //         ProductName = x.Name
        //     }).ToList();
        //     return Json(allsearch, System.Web.Mvc.JsonRequestBehavior.AllowGet);


        // }
        [HttpPost]
        public JsonResult AutoComplete()
        {
            var suto = new List<string>();
            suto.Add("java");
            suto.Add("john");
            suto.Add("safari");
            return Json(suto);
        }

    }
}

