using InventoryManagement.Data;
using InventoryManagement.Interfaces;
using InventoryManagement.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repositories
{
    public class ProductGroupRepository : IProductGroup
    {
        private readonly ApplicationContext _context;

        public ProductGroupRepository(ApplicationContext context)
        {
            _context = context;
        }
        /// <summary>
        /// This method adds the entered data in the ProductGroup to the database
        /// </summary>
        /// <param name="productGroup"></param>
        /// <returns>It returns data present in ProductGroup table</returns>
        public ProductGroup Create(ProductGroup productGroup)
        {
            _context.ProductGroups.Add(productGroup);
            _context.SaveChanges();
            return productGroup;
        }

        /// <summary>
        /// This method is used to removes the record from the ProductGroup table
        /// </summary>
        /// <param name="productGroup"></param>
        /// <returns>This method returns the updated ProductGroup table</returns>

        public ProductGroup Delete(ProductGroup productGroup)
        {
            _context.ProductGroups.Remove(productGroup);
            _context.Entry(productGroup).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
            return productGroup;
        }
        /// <summary>
        /// This method is used to update the record present in ProductGroup table
        /// </summary>
        /// <param name="productGroup"></param>
        /// <returns>It returns the updated data</returns>


        public ProductGroup Edit(ProductGroup productGroup)
        {
            _context.ProductGroups.Attach(productGroup);
            _context.Entry(productGroup).State = EntityState.Modified;
            _context.SaveChanges();
            return productGroup;
        }

        /// <summary>
        /// This method is used to fetch the particular record from the ProductGroup table based on ProductGroupId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns the particular data from the ProductGroup table</returns>

        public ProductGroup GetItem(int id)
        {
            ProductGroup productGroup = _context.ProductGroups.Where(item => item.ProductGroupId == id).FirstOrDefault();
            return productGroup;
        }
        /// <summary>
        /// This method is used to sort the table either in ascending or in descending order
        /// </summary>
        /// <param name="productGroups"></param>
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <returns>it returns the sorted table</returns>
        private List<ProductGroup> DoSort(List<ProductGroup> productGroups, string SortProperty, SortOrder sortOrder)
        {

            //sort based on name property
            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    productGroups = productGroups.OrderBy(n => n.Name).ToList();
                else
                    productGroups = productGroups.OrderByDescending(n => n.Name).ToList();

            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    productGroups = productGroups.OrderBy(d => d.Description).ToList();
                else
                    productGroups = productGroups.OrderByDescending(d => d.Description).ToList();

            }
            return (productGroups);

        }
        /// <summary>
        /// This method is used to return the related record based on the users search
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <param name="SearchText"></param>
        /// <returns>This method returns the records user searched for else it returns the list of the records present in the category table in sorted order.</returns>

        public List<ProductGroup> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "")
        {
            List<ProductGroup> productGroups = _context.ProductGroups.ToList();

            if (SearchText != "" && SearchText != null)
            {
                productGroups = _context.ProductGroups.
                        Where(u => u.Name.Contains(SearchText)).ToList();
            }
            else
                productGroups = _context.ProductGroups.ToList();


            productGroups = DoSort(productGroups, SortProperty, sortOrder);
            return productGroups;
        }
        /// <summary>
        /// checks if there exist a same unit name in the ProductGroup table by  passing the records with the help of count functionality
        /// </summary>
        /// <param name="name"></param>
        /// <returns>if ProductGroup name doesnot exist it returns count as zero else it returns true</returns>
        public bool IsProductGroupNameExist(string name)
        {
            int ct = _context.ProductGroups.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// used for Edit functionality and it checks if there exist a same ProductGroup name in the ProductGroup table with different id so we ignore this record and checks for any other record with same name
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Id"></param>
        /// <returns>if ProductGroup name doesnot exist it returns count as zero else it returns true</returns>
        public bool IsProductGroupNameExist(string name, int id)
        {
            int ct = _context.ProductGroups.Where(n => n.Name.ToLower() == name.ToLower() && n.ProductGroupId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        // <summary>
        /// This method is used to list the data from ProductGroup table
        /// </summary>
        /// <returns>It returns the records present in ProductGroup model</returns>
        public List<ProductGroup> GetItems()
        {
            List<ProductGroup> productGroups = _context.ProductGroups.ToList();
            return productGroups;
        }
    }
}

