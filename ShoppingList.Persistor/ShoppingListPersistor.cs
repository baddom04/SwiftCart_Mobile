using ShoppingList.Utils;
using System.Text.Json;

namespace ShoppingList.Persistor
{
    public static class ShoppingListPersistor
    {
        private readonly static JsonSerializerOptions options = new() { WriteIndented = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
        private readonly static string _shoppingListPath = "shopping_list.data";
        //public static IEnumerable<ShoppingItem> LoadShoppingList()
        //{
        //    try
        //    {
        //        IFileService fileService = ServiceProvider.Resolve<IFileService>();
        //        string? jsonString = fileService.ReadFile(_shoppingListPath);
        //        if (jsonString is null) return [];

        //        IEnumerable<ShoppingItem>? shoppingList = JsonSerializer.Deserialize<List<ShoppingItem>>(jsonString);
        //        return shoppingList ?? [];
        //    }
        //    catch
        //    {
        //        throw;
        //        //return [];
        //    }
        //}

        //public static void SaveShoppingList(IEnumerable<ShoppingItem> shoppingList)
        //{
        //    string jsonContent = JsonSerializer.Serialize(shoppingList, options);

        //    IFileService fileService = ServiceProvider.Resolve<IFileService>();
        //    fileService.SaveFile(_shoppingListPath, jsonContent);
        //}
    }
}
