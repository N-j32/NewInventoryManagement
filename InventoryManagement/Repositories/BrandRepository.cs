using InventoryManagement.Data;
using InventoryManagement.Interfaces;
using InventoryManagement.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repositories
{
    public class BrandRepository : IBrand
    {
        private readonly ApplicationContext _context;

        public BrandRepository(ApplicationContext context)
        {
            _context = context;
        }
        /// <summary>
        /// This method adds the entered data in the Brand to the database
        /// </summary>
        /// <param name="brand"></param>
        /// <returns>It returns data present in Brand table</returns>
        public Brand Create(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return brand;
        }
        /// <summary>
        /// This method is used to removes the record from the Brand table
        /// </summary>
        /// <param name="brand"></param>
        /// <returns>This method returns the updated Brand table</returns>
        public Brand Delete(Brand brand)
        {
            _context.Brands.Remove(brand);
            _context.Entry(brand).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
            return brand;
        }
        /// <summary>
        /// This method is used to update the record present in Brand table
        /// </summary>
        /// <param name="brand"></param>
        /// <returns>It returns the updated data</returns>
        public Brand Edit(Brand brand)
        {
            _context.Brands.Attach(brand);
            _context.Entry(brand).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return brand;
        }
        /// <summary>
        /// This method is used to fetch the particular record from the Brand table based on BrandId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the particular data from the Brand table</returns>
        public Brand GetItem(int id)
        {
            Brand brand = _context.Brands.Where(item => item.BrandId == id).FirstOrDefault();
            return brand;
        }
        /// <summary>
        /// This method is used to sort the table either in ascending or in descending order
        /// </summary>
        /// <param name="brands"></param>
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <returns>it returns the sorted table</returns>
        private List<Brand> DoSort(List<Brand> brands, string SortProperty, SortOrder sortOrder)
        {

            //sort based on name property
            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    brands = brands.OrderBy(n => n.Name).ToList();
                else
                    brands = brands.OrderByDescending(n => n.Name).ToList();

            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    brands = brands.OrderBy(d => d.Description).ToList();
                else
                    brands = brands.OrderByDescending(d => d.Description).ToList();

            }
            return (brands);

        }
        /// <summary>
        /// This method is used to return the related record based on the users search
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <param name="SearchText"></param>
        /// <returns>This method returns the records user searched for else it returns the list of the records present in the unit table in sorted order.</returns>
        public List<Brand> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "")
        {
            List<Brand> brands = _context.Brands.ToList();

            if (SearchText != "" && SearchText != null)
            {
                brands = _context.Brands.
                        Where(u => u.Name.Contains(SearchText)).ToList();
            }
            else
                brands = _context.Brands.ToList();


            brands = DoSort(brands, SortProperty, sortOrder);
            return brands;
        }
        /// <summary>
        /// checks if there exist a same unit name in the brand table by  passing the records with the help of count functionality
        /// </summary>
        /// <param name="name"></param>
        /// <returns>ifbrand name doesnot exist it returns count as zero else it returns true</returns>
        public bool IsBrandNameExist(string name)
        {
            int ct = _context.Brands.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// used for Edit functionality and it checks if there exist a same brand name in the brand table with different id so we ignore this record and checks for any other record with same name
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Id"></param>
        /// <returns>if brand name doesnot exist it returns count as zero else it returns true</returns>
        public bool IsBrandNameExist(string name, int Id)
        {
            int ct = _context.Brands.Where(n => n.Name.ToLower() == name.ToLower() && n.BrandId != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        // <summary>
        /// This method is used to list the data from brand table
        /// </summary>
        /// <returns>It returns the records present in brand model</returns>
        public List<Brand> GetItems()
        {
            List<Brand>Brands = _context.Brands.ToList();
            return Brands;
        }
    }
}

