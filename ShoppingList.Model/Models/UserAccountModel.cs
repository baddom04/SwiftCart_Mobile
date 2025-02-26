using ShoppingList.Core;
using ShoppingList.Persistor.Services.Interfaces;
using ShoppingList.Persistor;
using Microsoft.Extensions.DependencyInjection;

namespace ShoppingList.Model.Models
{
    public class UserAccountModel
    {
        public User? User { get; private set; }

        public async Task LoginAsync(string email, string password)
        {
            IUserService userService = AppServiceProvider.Services.GetRequiredService<IUserService>();
            await userService.LoginAsync(email, password);
        }


    }
}
