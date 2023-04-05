using InventoryManagement.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace InventoryManagement.Interfaces
{
    public interface IProductGroup
    {
        List<ProductGroup> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "");
        ProductGroup GetItem(int id);
        ProductGroup Create(ProductGroup productGroup);
        ProductGroup Edit(ProductGroup productGroup);
        ProductGroup Delete(ProductGroup productGroup);
        public bool IsProductGroupNameExist(string name);
        public bool IsProductGroupNameExist(string name, int Id);
        List<ProductGroup> GetItems();
    }
}
