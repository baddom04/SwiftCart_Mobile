using ShoppingList.Models;
using ShoppingList.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ShoppingList.Loaders
{
    internal static class ShoppingListLoader
    {
        private readonly static JsonSerializerOptions options = new() { WriteIndented = true };
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
                return [];
            }
        }

        public static void SaveShoppingList(List<ShoppingItem> shoppingList)
        {
            List<JsonShoppingItemModel> jsonModels = 
                [.. shoppingList.Select(item => new JsonShoppingItemModel() 
                {
                    Owner = item.Owner,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    Description = item.Description,
                })];

            string jsonContent = JsonSerializer.Serialize(jsonModels, options);

            IFileService fileService = ServiceProvider.Resolve<IFileService>();
            fileService.SaveFile(_shoppingListPath, jsonContent);
        }
    }
    internal class JsonShoppingItemModel
    {
        public User Owner { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Unit Unit { get; set; }
        public string? Description { get; set; }
    }
}
