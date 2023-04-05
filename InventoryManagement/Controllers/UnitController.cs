using InventoryManagement.Data;
using InventoryManagement.Interfaces;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static InventoryManagement.Models.Unit;

namespace InventoryManagement.Controllers
{
    public class UnitController : Controller
    {
        private readonly ILogger<UnitController> _logger;
        private readonly ApplicationContext _context;
        private readonly IUnit _unitRepo;
        public UnitController(ApplicationContext context, IUnit unitRepo, ILogger<UnitController> logger)
        {
            _context = context;
            _unitRepo = unitRepo;
            _logger = logger;
        }
        /// <summary>
        /// This method returns the list of the unit table with sorting , searchbox and pagination functionality
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="SearchText"></param>
        /// <param name="pg"></param>
        /// <param name="pageSize"></param>
        /// <returns>this method returns the list of records in sorted order </returns>
        public IActionResult Index(string sortExpression="",string SearchText="",int pg=1,int pageSize=5)
        {
           
            ViewData["SortParamName"]="name";
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



            List<Unit> units = _unitRepo.GetItems(sortProperty, sortOrder, SearchText);
            var pager = new PagerModel(units.Count, pg, pageSize);
            this.ViewBag.Pager = pager;
            //refill the unitlist with the records of selected page
            units = units.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
            TempData["CurrentPage"] = pg;
            return View(units);


         

        }
        /// <summary>
        /// This method loads the form to the user to enter the unit details
        /// </summary>
        /// <returns>This method returns the form which is present in unit method </returns>
        public IActionResult Create()
        {
            Unit unit = new Unit();
            return View(unit);
        }
        
        /// <summary>
        /// This method checks if the description length is greater than 4 or not and also checks
        /// unit is already exist or not . if all  the conditions saticifies, then it will create a record.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>If all the conditions are  satisified and  data is saved in database then it will return the success message or else it will return the error message</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Unit unit)
        {
            bool read = false;
            string errMessage = "";

            try
            {
                if (unit.Description.Length < 4 || unit.Description == null)

                    errMessage = String.Format("{0} description must be 4 charecters", unit.Name);
                //unit name exist or not
                if (_unitRepo.IsUnitNameExist(unit.Name) == true)
                   
                //errMessage = errMessage + " " + " Unit Name"+unit.Name+ "already exist";
                errMessage = String.Format("{0} Unit Name {1} already Exist", errMessage, unit.Name);
                if (errMessage == "")
                {
                    unit = _unitRepo.Create(unit);
                    read = true;
                }

               // unit = _unitRepo.Create(unit);
            }
            catch(Exception ex)
            {
                errMessage =String.Format("{0} {1} " ,errMessage, ex.Message);
            }
            if (read == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(unit);
            }
            else
            {
               
                TempData["SuccessMessage"] = String.Format("Unit {0} Created Successfully",unit.Name);
                return RedirectToAction(nameof(Index));
            }
            
        }

       /// <summary>
       /// This method is used to display the particular record.
       /// </summary>
       /// <param name="id"></param>
       /// <returns>It returns the unit object attached to it.</returns>

        public IActionResult Details(int id)
        {
            Unit unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
        /// <summary>
        /// This method is used to fetch the particular record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the unit object attached to it</returns>
        public IActionResult Edit(int id)
        {
            Unit unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
        /// <summary>
        /// This method checks if the description length is greater than 4 or not and also checks
        /// unit is already exist or not . if all conditions are satisified, then allow for updation 
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>If the condition is satisified and  data is updated in database then it will return the success message or else it will return the error message</returns>
        [HttpPost]
        public IActionResult Edit(Unit unit)
        {
            bool read = false;
            string errMessage = "";
            try
            {
                //for error message
                if(unit.Description.Length<4 || unit.Description==null)
                
                     errMessage = String.Format("{0} description must be 4 charecters",unit.Name);
                if (_unitRepo.IsUnitNameExist(unit.Name, unit.UnitId) == true)
                   
                    errMessage = String.Format("{0} Unit Name {1} already Exist", errMessage, unit.Name);

                if (errMessage == "")
                {
                    unit = _unitRepo.Edit(unit);
                    TempData["SuccessMessage"] = String.Format( " {0}  Unit Saved Successfully", unit.Name );
                    read = true;
                }

            }
            catch (Exception ex)
            {
               errMessage = String.Format(" {0} {1}",errMessage,ex.Message);
                
            }
            
            TempData["SuccessMessage"] = String.Format("Unit {0} Created Successfully", unit.Name);
            if (read == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(unit);
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
        /// <returns>It returns the unit object attached to it</returns>
        public IActionResult Delete(int id)
        {
            Unit unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
        /// <summary>
        /// Deletes the record and update the database and then it will return to the index page
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>if the delete is successfull it returns the success message else it will return the error message</returns>
        [HttpPost]
        public IActionResult Delete (Unit unit)
        {
            string errMessage = "";
            try
            {
                unit = _unitRepo.Delete(unit);
            }
            catch(Exception ex)
            {
                errMessage = String.Format(" {0} {1}", errMessage, ex.Message);
            }
            //TempData["SuccessMessage"] = "Unit " + unit.Name + " Deletd Successfully";
            TempData["SuccessMessage"] = String.Format("Unit {0} Deleted Successfully", unit.Name);
            return RedirectToAction(nameof(Index));
        }

        

    }
}
