using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ShoppingList.Loaders
{
    internal static class ShoppingListLoader
    {
        private readonly static string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "shoppinglist.json");
        private readonly static JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
        static ShoppingListLoader()
        {
            if (!File.Exists(dataPath)) using (File.Create(dataPath)) { }
        }

        public static List<ShoppingItem> LoadShoppingList()
        {
            try
            {
                string jsonString = File.ReadAllText(dataPath);
                List<ShoppingItem>? shoppingList = JsonSerializer.Deserialize<List<ShoppingItem>>(jsonString);
                return shoppingList ?? [];
            }
            catch
            {
                return [];
            }
        }

        public static async void SaveShoppingList(List<ShoppingItem> shoppingList)
        {
            string jsonString = JsonSerializer.Serialize(shoppingList, options);
            await File.WriteAllTextAsync(dataPath, jsonString);
        }
    }
}
