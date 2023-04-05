using InventoryManagement.Data;
using InventoryManagement.Interfaces;
using InventoryManagement.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repositories
{
    public class CategoryRepository : ICategory
    {
        private readonly ApplicationContext _context;

        public CategoryRepository(ApplicationContext context)
        {
            _context = context;
        }
        /// <summary>
        /// This method adds the entered data in the Category to the database
        /// </summary>
        /// <param name="category"></param>
        /// <returns>It returns data present in Category table</returns>
        public Category Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }
        /// <summary>
        /// This method is used to removes the record from the Category table
        /// </summary>
        /// <param name="category"></param>
        /// <returns>This method returns the updated Category table</returns>
        public Category Delete(Category category)
        {
            _context.Categories.Attach(category);
            _context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
            return category;
        }
        /// <summary>
        /// This method is used to update the record present in Category table
        /// </summary>
        /// <param name="category"></param>
        /// <returns>It returns the updated data</returns>
        public Category Edit(Category category)
        {
            _context.Categories.Remove(category);
            _context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return category;
        }
        /// <summary>
        /// This method is used to fetch the particular record from the Category table based on CategoryId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the particular data from the Category table</returns>
        public Category GetItem(int id)
        {
            Category category = _context.Categories.Where(item => item.CategoryId == id).FirstOrDefault();
            return category;
        }
        /// <summary>
        /// This method is used to sort the table either in ascending or in descending order
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <returns>it returns the sorted table</returns>
        private List<Category> DoSort(List<Category> categories, string SortProperty, SortOrder sortOrder)
        {

            //sort based on name property
            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    categories = categories.OrderBy(n => n.Name).ToList();
                else
                    categories = categories.OrderByDescending(n => n.Name).ToList();

            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    categories = categories.OrderBy(d => d.Description).ToList();
                else
                    categories = categories.OrderByDescending(d => d.Description).ToList();

            }
            return (categories);

        }
        /// <summary>
        /// This method is used to return the related record based on the users search
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <param name="SearchText"></param>
        /// <returns>This method returns the records user searched for else it returns the list of the records present in the category table in sorted order.</returns>
        public List<Category> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "")
        {
            List<Category> categories = _context.Categories.ToList();

            if (SearchText != "" && SearchText != null)
            {
                categories = _context.Categories.
                        Where(u => u.Name.Contains(SearchText)).ToList();
            }
            else
                categories = _context.Categories.ToList();


            categories = DoSort(categories, SortProperty, sortOrder);
            return categories;
        }
        /// <summary>
        /// checks if there exist a same unit name in the Category table by  passing the records with the help of count functionality
        /// </summary>
        /// <param name="name"></param>
        /// <returns>if Category name doesnot exist it returns count as zero else it returns true</returns>
        public bool IsCategoryNameExist(string name)
        {
            int ct = _context.Categories.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// used for Edit functionality and it checks if there exist a same Category name in the Category table with different id so we ignore this record and checks for any other record with same name
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Id"></param>
        /// <returns>if category name doesnot exist it returns count as zero else it returns true</returns>
        public bool IsCategoryNameExist(string name, int Id)
        {
            int ct = _context.Categories.Where(n => n.Name.ToLower() == name.ToLower() && n.CategoryId != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        // <summary>
        /// This method is used to list the data from Category table
        /// </summary>
        /// <returns>It returns the records present in Category model</returns>
        public List<Category> GetItems()
        {
            List<Category> category = _context.Categories.ToList();
            return category;
        }
    }
}

