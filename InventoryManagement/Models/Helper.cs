namespace InventoryManagement.Models
{
    public static class Helper
    {
        public static string GetTypeName(string fulltypeName)
        {
            string resulttString = "";
            
            try
            {
                int lastIndex = fulltypeName.LastIndexOf('.') + 1;
                resulttString = fulltypeName.Substring(lastIndex, fulltypeName.Length - lastIndex);
            }
            catch
            {
                resulttString = fulltypeName;

            }
            resulttString = resulttString.Replace("]", "");
            return resulttString;
        }
    }
}
