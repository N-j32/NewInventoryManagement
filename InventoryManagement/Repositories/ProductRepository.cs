using InventoryManagement.Data;
using InventoryManagement.Interfaces;
using InventoryManagement.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repositories
{
    public class ProductRepository : IProduct
    {
        private readonly ApplicationContext _context;


        public ProductRepository(ApplicationContext context)
        {
            _context = context;
        }
        /// <summary>
        /// This method adds the entered data in the Product to the database
        /// </summary>
        /// <param name="product"></param>
        /// <returns>It returns data present in  Product table</returns>
        public Product Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }
        /// <summary>
        /// This method is used to removes the record from the Produt table
        /// </summary>
        /// <param name="product"></param>
        /// <returns>This method returns the updated Product table</returns>
        public Product Delete(Product product)
        {
            _context.Products.Remove(product);
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
            return product;
        }
        /// <summary>
        /// This method is used to update the record present in Product table
        /// </summary>
        /// <param name="product"></param>
        /// <returns>It returns the updated data</returns>
        public Product Edit(Product product)
        {
            _context.Products.Attach(product);
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return product;
        }
        /// <summary>
        /// This method is used to sort the table either in ascending or in descending order
        /// </summary>
        /// <param name="products"></param>
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <returns>it returns the sorted table</returns>
        public Product GetItem(int id)
        {
            Product product = _context.Products.Where(item => item.ProductId == id).Include(u => u.Units)
                .Include(u => u.Brands)
                .Include(u => u.Categories)
                .FirstOrDefault();
            return product;
        }
        /// <summary>
        /// This method is used to list the records
        /// </summary>
        /// <returns>It returns the records present in the Product table</returns>
        public List<Product> GetItems()
        {
            List<Product> products = _context.Products.Include(u => u.Units)
                .Include(u => u.Brands)
                .Include(u => u.Categories)
                .ToList();
            return products;
        }
        // <summary>
        /// This method is used to sort the table either in ascending or in descending order
        /// </summary>
        /// <param name="products"></param>
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <returns>it returns the sorted table</returns>
        private List<Product> DoSort(List<Product> products, string SortProperty, SortOrder sortOrder)
        {

            //sort based on name property
            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    products = products.OrderBy(n => n.Name).ToList();
                else
                    products = products.OrderByDescending(n => n.Name).ToList();

            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    products = products.OrderBy(d => d.Code).ToList();
                else
                    products = products.OrderByDescending(d => d.Code).ToList();

            }
            return (products);

        }
        /// <summary>
        /// This method is used to return the related record based on the users search
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <param name="SearchText"></param>
        /// <returns>This method returns the records user searched for else it returns the list of the records present in the unit table in sorted order.</returns>
        public List<Product> Getitems(string SortProperty, SortOrder sortOrder, string SearchText = "")
        {
           
            List<Product> products = _context.Products.Include(u => u.Units)
                .Include(u => u.Brands)
                .Include(u => u.Categories)
                .ToList();

            if (SearchText != "" && SearchText != null)
            {

                products = _context.Products.
                        Where(u => u.Name.Contains(SearchText) || u.Brands.Name.Contains(SearchText)
                        || u.Units.Name.Contains(SearchText) || u.Cost.ToString().Contains(SearchText)).ToList();

            }


            else
                products = _context.Products.ToList();


            products = DoSort(products, SortProperty, sortOrder);
            return products;

        }

        /// <summary>
        /// checks if there exist a same unit name in the Product table by  passing the records with the help of count functionality
        /// </summary>
        /// <param name="name"></param>
        /// <returns>if Product name doesnot exist it returns count as zero else it returns true</returns>
        public bool IsProductNameExist(string name)
        {
            int ct = _context.Products.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// used for Edit functionality and it checks if there exist a same Product name in the Product table with different id so we ignore this record and checks for any other record with same name
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Id"></param>
        /// <returns>if Product name doesnot exist it returns count as zero else it returns true</returns>
        public bool IsProductNameExist(string name, int Id)
        {
            int ct = _context.Products.Where(n => n.Name.ToLower() == name.ToLower() && n.ProductId != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// This method is used for multifield searching of record
        /// </summary>
        /// <param name="SortProperty"></param>
        /// <param name="sortOrder"></param>
        /// <param name="SearchText"></param>
        /// <param name="searchString"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns>It returns the searched record in sorting format</returns>
        public List<Product> Getitems(string SortProperty, SortOrder sortOrder, string SearchText = "", 
            string searchString = "", string searchUnit="",string searchBrand="",int minPrice=0,int maxPrice=0)
        {
        
            var products = _context.Products.Include(b => b.Units)
               .Include(b =>b.Brands)
               .Include(b => b.Categories)
               .ToList();

          
            searchString = searchString ?? string.Empty;
            searchBrand = searchBrand ?? string.Empty;
            searchUnit = searchUnit ?? string.Empty;
            
           products = products.Where(b => (b.Name==searchString || string.IsNullOrEmpty(searchString)) 
                 && (b.Brands.Name==searchBrand || string.IsNullOrEmpty(searchBrand))
                 &&(b.Units.Name==searchUnit || string.IsNullOrEmpty(searchUnit))
            &&(b.Cost >= minPrice|| minPrice==0)  && (b.Cost<=maxPrice || maxPrice == 0)).ToList();
            

            products = DoSort(products, SortProperty, sortOrder);
            return products;
        }
    }
}

