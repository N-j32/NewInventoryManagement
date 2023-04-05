using InventoryManagement.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace InventoryManagement.Interfaces
{
    public interface IProduct
    {
       List<Product> GetItems();
        List<Product> Getitems(string SortProperty, SortOrder sortOrder, string SearchText = "",  
            string searchString = "",string searchUnit="", string searchBrand = "",int minPrice=0 ,int maxPrice=0);
        Product GetItem(int id);
        Product Create(Product product);
        Product Edit(Product product);
        Product Delete(Product product);
        List<Product> Getitems(string SortProperty, SortOrder sortOrder, string SearchText = "");
        public bool IsProductNameExist(string name);
        public bool IsProductNameExist(string name, int Id);

    }
}
