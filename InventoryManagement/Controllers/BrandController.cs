using InventoryManagement.Interfaces;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        //private readonly ApplicationContext _context;
        private readonly IBrand _repo;
        private readonly ILogger<BrandController> _logger;

        public BrandController(IBrand repo, ILogger<BrandController> logger)
        {
            // _context = context;
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// This method returns the list of the Brand table with sorting , searchbox and pagination functionality
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="SearchText"></param>
        /// <param name="pg"></param>
        /// <param name="pageSize"></param>
        /// <returns>this method returns the list of records in sorted order </returns>
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            ViewData["SortParamName"] = "name";
            ViewData["SortParamDesc"] = "description";
            SortOrder sortOrder;
            string sortProperty;
            

            switch (sortExpression.ToLower())
            {
                case "name_desc":
                    sortOrder = SortOrder.Descending;
                    sortProperty = "name";
                    ViewData["SortParamName"] = "name";
                    
                    ViewData["SortIconName"] = "fa fa-arrow-down";
                    break;

                case "description":
                    sortOrder = SortOrder.Ascending;
                    sortProperty = "description";
                    ViewData["SortIconDesc"] = "fa fa-arrow-up";
                    ViewData["SortParamDesc"] = "description_desc";

                    break;

                case "description_desc":
                    sortOrder = SortOrder.Descending;
                    sortProperty = "description";
                    ViewData["SortIconDesc"] = "fa fa-arrow-down";
                    ViewData["SortParamDesc"] = "description";
                    break;

                default:
                    sortOrder = SortOrder.Ascending;
                    sortProperty = "name";
                    ViewData["SortIconName"] = "fa fa-arrow-up";
                    ViewData["SortParamName"] = "name_desc";
                    break;
            }



            List<Brand> brands = _repo.GetItems(sortProperty, sortOrder, SearchText);
            var pager = new PagerModel(brands.Count, pg, pageSize);
            this.ViewBag.Pager = pager;
            //refill the unitlist with the records of selected page
            brands = brands.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
            TempData["CurrentPage"] = pg;
            return View(brands);




        }

        /// <summary>
        /// This method is used to display the particular record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the Brand object attached to it.</returns>

        public IActionResult Details(int id)
        {
           
            Brand brand = _repo.GetItem(id);
            return View(brand);
        }
        /// <summary>
        /// This method is used to fetch the particular record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the Brand object attached to it</returns>

        public IActionResult Edit(int id)
        {
            Brand brand = _repo.GetItem(id);
            return View(brand);
        }
        /// <summary>
        /// This method checks if the description length is greater than 4 or not and also checks
        /// unit is already exist or not
        /// </summary>
        /// <param name="brand"></param>
        /// <returns>If the condition is satisified and  data is updated in database then it will return the success message or else it will return the error message</returns>
        [HttpPost]
        public IActionResult Edit(Brand brand)
        {

            bool read = false;
            string errMessage = "";
            try
            {
                //for error message
                if (brand.Description.Length < 4 || brand.Description == null)

                    errMessage = String.Format(" description of {0} must be 4 charecters", brand.Name);
                if (_repo.IsBrandNameExist(brand.Name, brand.BrandId) == true)
                    errMessage = String.Format( "{0} Brand Name {1} Already Exist",errMessage,brand.Name);

                if (errMessage == "")
                {
                    brand = _repo.Edit(brand);
                    TempData["SuccessMessage"] = String.Format( "{0}  Unit Saved Successfully",brand.Name);
                    read = true;
                }

            }
            catch (Exception ex)
            {
                errMessage = String.Format("{0} {1} ", errMessage, ex.Message);

            }
            
            TempData["SuccessMessage"] = String.Format("{0} Saved Successfully",brand.Name);
            if (read == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(brand);
            }
            else
            {

                return RedirectToAction(nameof(Index));
            }

        }


        /// <summary>
        /// This method is used to fetch the particular record to be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the Brand object attached to it</returns>


        public IActionResult Delete(int id)
        {
            Brand brand = _repo.GetItem(id);
            return View(brand);
        }
        /// <summary>
        /// Deletes the record and update the database and then it will return to the index page
        /// </summary>
        /// <param name="brand"></param>
        /// <returns>if the delete is successfull it returns the success message else it will return the error message</returns>
        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            string errMessage = "";
            try
            {
                brand = _repo.Delete(brand);
            }
            catch (Exception ex)
            {
                errMessage = String.Format(" {0} {1}", errMessage, ex.Message);
            }
            TempData["SuccessMessage"] = String.Format("Brand {0} Deleted Successfully", brand.Name);
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// This method loads the form to the user to enter the brand details
        /// </summary>
        /// <returns>This method returns the form which is present in Brand method </returns>
        public IActionResult Create()
        {
            Brand brand = new Brand();
            return View(brand);
        }
        /// <summary>
        /// This method checks if the description length is greater than 4 or not and also checks
        /// Brand is already exist or not
        /// </summary>
        /// <param name="brand"></param>
        /// <returns>If the condition is satisified and  data is saved in database then it will return the success message or else it will return the error message</returns>
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            bool read = false;
            string errMessage = "";

            try
            {
                if (brand.Description.Length < 4 ||brand.Description == null)

                    errMessage = String.Format(" description of {0} must be 4 charecters", brand.Name);
                //unit name exist or not
                if (_repo.IsBrandNameExist(brand.Name) == true)
                    errMessage =String.Format( " {0} Brand Name  {1} already exist",errMessage,brand.Name);
                if (errMessage == "")
                {
                    brand = _repo.Create(brand);
                    read = true;
                }

                // unit = _unitRepo.Create(unit);
            }
            catch (Exception ex)
            {
                errMessage = String.Format(" {0} {1}", errMessage, ex.Message);
            }
            if (read == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(brand);
            }
            else
            {
                TempData["SuccessMessage"] = String.Format("Brand {0} created Successfully",brand.Name);
                return RedirectToAction(nameof(Index));
            }

        }


    }
}
