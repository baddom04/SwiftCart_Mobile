using ShoppingList.Models;
using ShoppingList.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace ShoppingList.Loaders
{
    internal static class ShoppingListLoader
    {
        private readonly static JsonSerializerOptions options = new() { WriteIndented = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
        private readonly static string _shoppingListPath = "shopping_list.json";
        public static List<ShoppingItem> LoadShoppingList()
        {
            try
            {
                IFileService fileService = ServiceProvider.Resolve<IFileService>();
                string? jsonString = fileService.ReadFile(_shoppingListPath);
                if (jsonString is null) return [];

                List<ShoppingItem>? shoppingList = JsonSerializer.Deserialize<List<ShoppingItem>>(jsonString);
                return shoppingList ?? [];
            }
            catch
            {
                throw;
                //return [];
            }
        }

        public static void SaveShoppingList(ObservableCollection<ShoppingItemDisplay> shoppingList)
        {
            List<ShoppingItem> list = shoppingList.Select(display => display.Item).ToList();
            string jsonContent = JsonSerializer.Serialize(list, options);

            IFileService fileService = ServiceProvider.Resolve<IFileService>();
            fileService.SaveFile(_shoppingListPath, jsonContent);
        }
    }
}
