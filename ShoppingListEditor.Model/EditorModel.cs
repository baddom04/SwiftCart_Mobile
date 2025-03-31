using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingListEditor.Model
{
    public class EditorModel
    {
        public Store? Store { get; private set; }
        private readonly IStoreService _storeService = AppServiceProvider.Services.GetRequiredService<IStoreService>();

        public async Task CreateStoreAsync(string name)
        {
            Store = await _storeService.CreateStoreAsync(name);
        }
    }
}
