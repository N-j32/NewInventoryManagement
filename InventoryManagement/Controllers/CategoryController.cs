using InventoryManagement.Interfaces;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Controllers
{
    public class CategoryController : Controller
    {
        //private readonly ApplicationContext _context;
        private readonly ICategory _repo;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategory repo, ILogger<CategoryController> logger)
        {
            // _context = context;
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// This method returns the list of the Category table with sorting , searchbox and pagination functionality
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="SearchText"></param>
        /// <param name="pg"></param>
        /// <param name="pageSize"></param>
        /// <returns>this method returns the list of records in sorted order</returns>
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            ViewData["SortParamName"] = "name";
            ViewData["SortParamDesc"] = "description";
            SortOrder sortOrder;
            string sortProperty;
            //ViewData["SortIconName"] = "";
            // ViewData["SortIconDesc"] = "";

            switch (sortExpression.ToLower())
            {
                case "name_desc":
                    sortOrder = SortOrder.Descending;
                    sortProperty = "name";
                    ViewData["SortParamName"] = "name";
                    //use this in js
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



            List<Category> categories = _repo.GetItems(sortProperty, sortOrder, SearchText);
            var pager = new PagerModel(categories.Count, pg, pageSize);
            this.ViewBag.Pager = pager;
            //refill the unitlist with the records of selected page
            categories = categories.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
            TempData["CurrentPage"] = pg;
            return View(categories);




        }

        /// <summary>
        /// This method is used to display the particular record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the Category object attached to it.</returns>

        public IActionResult Details(int id)
        {
            Category category = _repo.GetItem(id);
            return View(category);
        }
        /// <summary>
        /// This method is used to fetch the particular record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the Category object attached to it</returns>

        public IActionResult Edit(int id)
        {
            Category category = _repo.GetItem(id);
            return View(category);
        }
        /// <summary>
        /// This method checks if the description length is greater than 4 or not and also checks
        /// Category is already exist or not
        /// </summary>
        /// <param name="category"></param>
        /// <returns>If the condition is satisified and  data is updated in database then it will return the success message or else it will return the error message</returns>
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            bool read = false;
            string errMessage = "";
            try
            {
                //for error message
                if (category.Description.Length < 4 || category.Description == null)

                    errMessage = String.Format(" description of {0} must be 4 charecters",category.Name);
                if (_repo.IsCategoryNameExist(category.Name, category.CategoryId) == true)
                    errMessage =String.Format(" {0} Category Name {1} Already Exist",errMessage,category.Name);

                if (errMessage == "")
                {
                    category = _repo.Edit(category);
                    TempData["SuccessMessage"] = String.Format("{0} Category Saved Successfully", category.Name);
                    read = true;
                }

            }
            catch (Exception ex)
            {
                errMessage = String.Format(" {0} {1}", errMessage, ex.Message);

            }
            TempData["SuccessMessage"] =String.Format( "Category {0} Saved Successfully",category.Name);
            if (read == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(category);
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
        /// <returns>It returns the Category object attached to it</returns>


        public IActionResult Delete(int id)
        {
            Category category = _repo.GetItem(id);
            return View(category);
        }
        /// <summary>
        /// Deletes the record and update the database and then it will return to the index page
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>if the delete is successfull it returns the success message else it will return the error message</returns>
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            string errMessage = "";
            try
            {
                category = _repo.Delete(category);
            }
            catch (Exception ex)
            {
                errMessage = String.Format(" {0} {1}", errMessage, ex.Message);
            }

            TempData["SuccessMessage"] = String.Format("Category {0} Deleted Successfully", category.Name);
            return RedirectToAction(nameof(Index));
        }
        // <summary>
        /// This method loads the form to the user to enter the Category details
        /// </summary>
        /// <returns>This method returns the form which is present in Category method </returns>
        public IActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }
        /// <summary>
        /// This method checks if the description length is greater than 4 or not and also checks
        /// unit is already exist or not
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>If the condition is satisified and  data is saved in database then it will return the success message or else it will return the error message</returns>
        [HttpPost]
        public IActionResult Create(Category category)
        {
            bool read = false;
            string errMessage = "";

            try
            {
                if (category.Description.Length < 4 || category.Description == null)

                    errMessage = String.Format(" description of {0} must be 4 charecters", category.Name);
                //unit name exist or not
                if (_repo.IsCategoryNameExist(category.Name) == true)
                    errMessage = String.Format("{0} Category Name {1} already exist",errMessage,category.Name);
                if (errMessage == "")
                {
                    category = _repo.Create(category);
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
                return View(category);
            }
            else
            {
                TempData["SuccessMessage"] = String.Format("Unit {0} created Successfully",category.Name);
                return RedirectToAction(nameof(Index));
            }

        }


    }
}
