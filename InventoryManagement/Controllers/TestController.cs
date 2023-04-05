using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
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
