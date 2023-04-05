using InventoryManagement.Data;
using InventoryManagement.Interfaces;
using InventoryManagement.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static InventoryManagement.Models.Unit;

namespace InventoryManagement.Repositories
{
    public class UnitRepository : IUnit
    {
        private readonly ApplicationContext _context;

        public UnitRepository(ApplicationContext context)
        {
            _context = context;
        }
        /// <summary>
        /// This method adds the entered data in the Unit to the database
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>It returns data present in unit table</returns>
        public Unit Create(Unit unit)
        {
            _context.Units.Add(unit);
            _context.SaveChanges();
            return unit;
        }
        /// <summary>
        /// This method is used to removes the record from the unit table
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>This method returns the updated unit table</returns>
        public Unit Delete(Unit unit)
        {
            _context.Units.Remove(unit);
            _context.Entry(unit).State = EntityState.Deleted;
            _context.SaveChanges();
            return unit;
        }

        /// <summary>
        /// This method is used to update the record present in Unit table
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>It returns the updated data</returns>
        public Unit Edit(Unit unit)
        {
            _context.Units.Attach(unit);
            _context.Entry(unit).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return unit;
        }
        /// <summary>
        /// This method is used to sort the table either in ascending or in descending order
        /// </summary>
        /// <param name="units"></param>
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <returns>it returns the sorted table</returns>
        private List<Unit>DoSort(List<Unit>units,string SortProperty, SortOrder sortOrder)
        {
           
            //sort based on name property
            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    units = units.OrderBy(n => n.Name).ToList();
                else
                    units = units.OrderByDescending(n => n.Name).ToList();

            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    units = units.OrderBy(d => d.Description).ToList();
                else
                    units = units.OrderByDescending(d => d.Description).ToList();

            }
            return (units);

        }
        /// <summary>
        /// This method is used to return the related record based on the users search
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <param name="SearchText"></param>
        /// <returns>This method returns the records user searched for else it returns the list of the records present in the unit table in sorted order.</returns>
        public List<Unit> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "")
        {
            List<Unit> units = _context.Units.ToList();

            if (SearchText != "" && SearchText != null)
            {
                units = _context.Units.
                        Where(u => u.Name.Contains(SearchText)).ToList();
            }
            else
                units = _context.Units.ToList();


            units = DoSort(units, SortProperty, sortOrder);
            return units;
        }

        /// <summary>
        /// This method is used to fetch the particular record from the unit table based on unitId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the particular data from Unit table</returns>

        public Unit GetUnit(int id)
        {
           
                Unit unit = _context.Units.Where(u => u.UnitId == id).FirstOrDefault();
                return unit;
            
        }




        /// <summary>
        /// checks if there exist a same unit name in the unit table by  passing the records with the help of count functionality
        /// </summary>
        /// <param name="name"></param>
        /// <returns>if unit name doesnot exist it returns count as zero else it returns true</returns>
        public bool IsUnitNameExist(string name)
        {
            int ct = _context.Units.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// used for Edit functionality and it checks if there exist a same unit name in the unit table with different id so we ignore this record and checks for any other record with same name
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Id"></param>
        /// <returns>if unit name doesnot exist it returns count as zero else it returns true</returns>
        public bool IsUnitNameExist(string name, int Id)
        {
            int ct = _context.Units.Where(n => n.Name.ToLower() == name.ToLower() && n.UnitId != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// This method is used to list the data from unit table
        /// </summary>
        /// <returns>It returns the records present in Unit model</returns>
        public List<Unit> GetItems()
        {
            List<Unit> units = _context.Units.ToList();
            return units;
        }
    }
}
