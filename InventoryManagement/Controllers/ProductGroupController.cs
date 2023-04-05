using InventoryManagement.Interfaces;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericRepositoryDemo.Controllers
{
    public class ProductGroupController : Controller
    {
        //private readonly ApplicationContext _context;
        private readonly IProductGroup _repo;
        private readonly ILogger<ProductGroupController> _logger;
        public ProductGroupController(IProductGroup repo, ILogger<ProductGroupController> logger)
        {
            // _context = context;
            _repo = repo;
            _logger =logger;
        }

        /// <summary>
        /// This method returns the list of the ProductGroup table with sorting , searchbox and pagination functionality
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



            List<ProductGroup> productGroups = _repo.GetItems(sortProperty, sortOrder, SearchText);
            var pager = new PagerModel(productGroups.Count, pg, pageSize);
            this.ViewBag.Pager = pager;
            //refill the unitlist with the records of selected page
            productGroups = productGroups.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
            TempData["CurrentPage"] = pg;
            return View(productGroups);
        }

        /// <summary>
        /// This method is used to display the particular record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the ProductGroup object attached to it.</returns>

        public IActionResult Details(int id)
        {
            ProductGroup productGroup = _repo.GetItem(id);
            return View(productGroup);
        }
        /// <summary>
        /// This method is used to fetch the particular record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the ProductGroup object attached to it</returns>

        public IActionResult Edit(int id)
        {
            ProductGroup productGroup = _repo.GetItem(id);
            return View(productGroup);
        }
        /// <summary>
        /// This method checks if the description length is greater than 4 or not and also checks
        /// unit is already exist or not
        /// </summary>
        /// <param name="productGroup"></param>
        /// <returns>If the condition is satisified and  data is updated in database then it will return the success message or else it will return the error message</returns>
        [HttpPost]
        public IActionResult Edit(ProductGroup productGroup)
        {
            bool read = false;
            string errMessage = "";
            try
            {
                //for error message
                if (productGroup.Description.Length < 4 || productGroup.Description == null)

                    errMessage = String.Format(" description of {0} must be 4 charecters",productGroup.Name);
                if (_repo.IsProductGroupNameExist(productGroup.Name, productGroup.ProductGroupId) == true)
                    //errMessage = errMessage + " Unit Name" + productGroup.Name + "Already Exist";
                    errMessage = String.Format( "{0} Unit Name {1} Already Exist",errMessage,productGroup.Name);

                if (errMessage == "")
                {
                    productGroup = _repo.Edit(productGroup);
                    TempData["SuccessMessage"] = String.Format( " {0} Unit Saved Successfully",productGroup.Name);
                    read = true;
                }

            }
            catch (Exception ex)
            {
                errMessage = String.Format(" {0} {1}", errMessage, ex.Message);

            }
            TempData["SuccessMessage"] = String.Format("Product group {0} Saved Successfully",productGroup.Name);
            if (read == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(productGroup);
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
        /// <returns>It returns the ProductGroup object attached to it</returns>
        public IActionResult Delete(int id)
        {
            ProductGroup productGroup = _repo.GetItem(id);
            return View(productGroup);
        }
        /// <summary>
        /// Deletes the record and update the database and then it will return to the index page
        /// </summary>
        /// <param name="productGroup"></param>
        /// <returns>if the delete is successfull it returns the success message else it will return the error message</returns>
        [HttpPost]
        public IActionResult Delete(ProductGroup productGroup)
        {
            string errMessage = "";
            try
            {
                productGroup = _repo.Delete(productGroup);
            }
            catch (Exception ex)
            {
                errMessage = String.Format(" {0} {1}", errMessage, ex.Message);
            }
            
            TempData["SuccessMessage"] = String.Format("Unit {0} Deleted Successfully", productGroup.Name);
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// This method loads the form to the user to enter the ProductGroup details
        /// </summary>
        /// <returns>This method returns the form which is present in ProductGroup method </returns>
        public IActionResult Create()
        {
            ProductGroup productGroup = new ProductGroup();
            return View(productGroup);
        }
        /// <summary>
        /// This method checks if the description length is greater than 4 or not and also checks
        /// ProductGroup is already exist or not
        /// </summary>
        /// <param name="productGroup"></param>
        /// <returns>If the condition is satisified and  data is saved in database then it will return the success message or else it will return the error message</returns>
        [HttpPost]
        public IActionResult Create(ProductGroup productGroup)
        {
            bool read = false;
            string errMessage = "";

            try
            {
                if (productGroup.Description.Length < 4 || productGroup.Description == null)

                    errMessage = String.Format(" description of {0} must be 4 charecters",productGroup.Name);
                //unit name exist or not
                if (_repo.IsProductGroupNameExist(productGroup.Name) == true)
                    errMessage = String.Format("{0} ProductGroup Name {1} Already Exist", errMessage, productGroup.Name);
                if (errMessage == "")
                {
                    productGroup = _repo.Create(productGroup);
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
                return View(productGroup);
            }
            else
            {
                TempData["SuccessMessage"] = String.Format("ProductGroup {0} created Successfully",productGroup.Name);
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
