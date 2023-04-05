using InventoryManagement.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace InventoryManagement.Interfaces
{
    public interface IUnit
    {
        List<Unit> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "");
        Unit GetUnit(int id);
        Unit Create(Unit unit);
        Unit Edit(Unit unit);
        Unit Delete(Unit unit);
        public bool IsUnitNameExist(string name);
        public bool IsUnitNameExist(string name,int Id);
        List<Unit> GetItems();
    }
}
