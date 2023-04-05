using InventoryManagement.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace InventoryManagement.Interfaces
{
    public interface ICategory
    {
        List<Category> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "");
        Category GetItem(int id);
        Category Create(Category category);
        Category Edit(Category category);
        Category Delete(Category category);
        public bool IsCategoryNameExist(string name);
        public bool IsCategoryNameExist(string name, int Id);
        List<Category> GetItems();
    }
}
