using InventoryManagement.Interfaces;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Controllers
{
    public class ProductController : Controller
    {
        //private readonly ApplicationContext _context;
        private readonly IProduct _repo;
        private readonly IUnit _unitRepo;
        private readonly IBrand _brandRepo;
        private readonly ICategory _categoryRepo;
        private readonly IProductGroup _productGroupRepo;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProduct repo, IUnit unitRepo, IBrand brandRepo, ICategory categoryRepo, IProductGroup productGroupRepo, ILogger<ProductController> logger)
        {
            // _context = context;
            _repo = repo;
            _unitRepo = unitRepo;
            _brandRepo = brandRepo;
            _categoryRepo = categoryRepo;
            _productGroupRepo = productGroupRepo;
            _logger = logger;
        }

        /// <summary>
        /// This method returns the list of the Product table with sorting , searchbox and pagination functionality
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="SearchText"></param>
        /// <param name="pg"></param>
        /// <param name="pageSize"></param>
        /// <returns>this method returns the list of records in sorted order </returns>
        public IActionResult Index(string sortExpression = "", string SearchText = "",int pg = 1, 
            int pageSize = 5, string searchString = "", string searchUnit="",
            string searchBrand="",int minPrice=0,int maxPrice=0)
        {
            ViewData["SortParamName"] = "name";
            ViewData["SortParamDesc"] = "code";
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
                    
                    ViewData["SortIconName"] = "fa fa-arrow-down";
                    break;

                case "code":
                    sortOrder = SortOrder.Ascending;
                    sortProperty = "code";
                    ViewData["SortIconDesc"] = "fa fa-arrow-up";
                    ViewData["SortParamDesc"] = "code_desc";

                    break;

                case "code_desc":
                    sortOrder = SortOrder.Descending;
                    sortProperty = "code";
                    ViewData["SortIconDesc"] = "fa fa-arrow-down";
                    ViewData["SortParamDesc"] = "code";
                    break;

                default:
                    sortOrder = SortOrder.Ascending;
                    sortProperty = "name";
                    ViewData["SortIconName"] = "fa fa-arrow-up";
                    ViewData["SortParamName"] = "name_desc";
                    break;
            }



            List<Product> products = _repo.Getitems(sortProperty, sortOrder, SearchText,  searchString,  searchUnit,searchBrand ,minPrice,maxPrice);
            Product product = new Product();
            ViewBag.Units = GetUnits();
            ViewBag.Brands = GetBrands();
            ViewBag.Categories = GetCategories();
            ViewBag.ProductGroups = GetProductGroups();
            var pager = new PagerModel(products.Count, pg, pageSize);
            this.ViewBag.Pager = pager;
            //refill the unitlist with the records of selected page
            products = products.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
            TempData["CurrentPage"] = pg;
            return View(products);




        }

        /// <summary>
        /// This method is used to display the particular record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the Product object attached to it.</returns>

        public IActionResult Details(int id)
        {
            Product product = _repo.GetItem(id);
            return View(product);
        }
        /// <summary>
        /// This method is used to fetch the particular record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the Product object attached to it</returns>

        public IActionResult Edit(int id)
        {
            Product product = _repo.GetItem(id);
            ViewBag.Units = GetUnits();
            ViewBag.Brands = GetBrands();
            ViewBag.Categories = GetCategories();
            ViewBag.ProductGroups = GetProductGroups();
            //TempData.Keep();
            return View(product);
        }
        /// <summary>
        /// This method checks if the description length is greater than 4 or not and also checks
        /// unit is already exist or not
        /// </summary>
        /// <param name="product"></param>
        /// <returns>If the condition is satisified and  data is updated in database then it will return the success message or else it will return the error message</returns>
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            bool read = false;
            string errMessage = "";
            try
            {
                //for error message
                if (product.Description.Length < 4 || product.Description == null)

                    errMessage = String.Format(" description of {0} must be 4 charecters", product.Name);
                if (_repo.IsProductNameExist(product.Name, product.UnitId) == true)
                    errMessage = String.Format(" {0} Product Name {1} Already Exist",errMessage,product.Name);

                if (errMessage == "")
                {
                    product = _repo.Edit(product);
                    TempData["SuccessMessage"] = String.Format( "{0} Unit Saved Successfully",product.Name);
                    read = true;
                }

            }
            catch (Exception ex)
            {
                errMessage = String.Format( "{0} {1} ",errMessage,ex.Message );

            }
            TempData["SuccessMessage"] = String.Format("Unit {0} Saved Successfully",product.Name);
            if (read == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(product);
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
        /// <returns>It returns the Product object attached to it</returns>


        public IActionResult Delete(int id)
        {
            Product product = _repo.GetItem(id);
            return View(product);
        }

        /// <summary>
        /// Deletes the record and update the database and then it will return to the index page
        /// </summary>
        /// <param name="product"></param>
        /// <returns>if the delete is successfull it returns the success message else it will return the error message</returns>
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            string errMessage="";
            try
            {
                product = _repo.Delete(product);
            }
            catch(Exception ex)
            {
                errMessage = String.Format(" {0} {1}", errMessage, ex.Message);
            }
            TempData["SuccessMessage"] = String.Format("Product {0} Deleted Successfully", product.Name);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// This method loads the form to the user to enter the Product details
        /// </summary>
        /// <returns>This method returns the form which is present in Product method </returns>
        public IActionResult Create()
        {
            Product product = new Product();
            ViewBag.Units = GetUnits();
            ViewBag.Brands = GetBrands();
            ViewBag.Categories = GetCategories();
            ViewBag.ProductGroups = GetProductGroups();

            return View(product);
        }
        /// <summary>
        /// This method checks if the description length is greater than 4 or not and also checks
        /// Brand is already exist or not
        /// </summary>
        /// <param name="product"></param>
        /// <returns>If the condition is satisified and  data is saved in database then it will return the success message or else it will return the error message</returns>
        [HttpPost]
        public IActionResult Create(Product product)
        {
            bool read = false;
            string errMessage = "";

            try
            {
                if (product.Description.Length < 4 || product.Description == null)

                    errMessage = String.Format(" description of {0} must be 4 charecters", product.Name);
                //unit name exist or not

                if (errMessage == "")
                {
                    product = _repo.Create(product);
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
                return View(product);
            }
            else
            {
                TempData["SuccessMessage"] = String.Format("Product {0} created Successfully",product.Name);
                return RedirectToAction(nameof(Index));
            }

        }
        /// <summary>
        /// This method is used to bind the Unit list in the product form
        /// </summary>
        /// <returns>this method returns the list of the unit name</returns>
        private List<SelectListItem> GetUnits()
        {
            var listUnits = new List<SelectListItem>();
            List<Unit> units = _unitRepo.GetItems();
            listUnits = units.Select(u => new SelectListItem()
            {
                Value = u.UnitId.ToString(),
                Text = u.Name
            }).ToList();

            var item = new SelectListItem()
            {
                Value = "",
                Text = "------Select Unit------"
            };
            listUnits.Insert(0, item);
            return listUnits;

        }

        /// <summary>
        /// This method is used to bind the Brand list in the product form
        /// </summary>
        /// <returns>this method returns the list of the Brand name</returns>
        private List<SelectListItem> GetBrands()
        {
            var listBrands = new List<SelectListItem>();
            List<Brand> brands = _brandRepo.GetItems();
            listBrands = brands.Select(u => new SelectListItem()
            {
                Value = u.BrandId.ToString(),
                Text = u.Name
            }).ToList();

            var item = new SelectListItem()
            {
                Value = "",
                Text = "------Select Brand------"
            };
            listBrands.Insert(0, item);
            return listBrands;

        }
        /// <summary>
        /// This method is used to bind the Category list in the product form
        /// </summary>
        /// <returns>this method returns the list of the Category name</returns>
        private List<SelectListItem> GetCategories()
        {
            var listCategories = new List<SelectListItem>();
            List<Category> categories = _categoryRepo.GetItems();
            listCategories = categories.Select(u => new SelectListItem()
            {
                Value = u.CategoryId.ToString(),
                Text = u.Name
            }).ToList();

            var item = new SelectListItem()
            {
                Value = "",
                Text = "------Select Category------"
            };
            listCategories.Insert(0, item);
            return listCategories;

        }
        /// <summary>
        /// This method is used to bind the ProductGroup list in the product form
        /// </summary>
        /// <returns>this method returns the list of the ProductGroup name</returns>
        private List<SelectListItem> GetProductGroups()
        {
            var listProductGroups = new List<SelectListItem>();
            List<ProductGroup> productGroups = _productGroupRepo.GetItems();
            listProductGroups = productGroups.Select(u => new SelectListItem()
            {
                Value = u.ProductGroupId.ToString(),
                Text = u.Name
            }).ToList();

            var item = new SelectListItem()
            {
                Value = "",
                Text = "------Select ProductGroup------"
            };
            listProductGroups.Insert(0, item);
            return listProductGroups;

        }

    }
}
