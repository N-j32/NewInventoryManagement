using InventoryManagement.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace InventoryManagement.Interfaces
{
    public interface IBrand
    {
        List<Brand> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "");
        Brand GetItem(int id);
        Brand Create(Brand brand);
        Brand Edit(Brand brand);
        Brand Delete(Brand brand);
        public bool IsBrandNameExist(string name);
        public bool IsBrandNameExist(string name, int Id);
        List<Brand> GetItems();
    }
}
