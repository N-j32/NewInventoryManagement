using InventoryManagement.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Controllers
{
    public class CascadeController : Controller
    {
        private readonly ApplicationContext context;

        public CascadeController(ApplicationContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CascadeDropdown()
        {
            return View();
        }
        //to get countries
        public JsonResult Country()
        {
            var country = context.Countries.ToList();
            return new JsonResult(country);
        }
        //to get state
        public JsonResult State(int id)
        {
            var state = context.States.Where(s=>s.Country.Id==id).ToList();
            return new JsonResult(state);
        }
        //using id we are fetching city
        public JsonResult City(int id)
        {
            var city = context.Cities.Where(s => s.State.Id == id).ToList();
            return new JsonResult(city);
        }
    }
}
