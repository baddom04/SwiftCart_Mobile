using ShoppingList.Core;
using ShoppingList.Persistor.Services.Interfaces;
using ShoppingList.Persistor;
using Microsoft.Extensions.DependencyInjection;

namespace ShoppingList.Model.Models
{
    public class UserAccountModel
    {
        private readonly IUserService _service;
        public User? User { get; private set; }

        public UserAccountModel()
        {
            _service = AppServiceProvider.Services.GetRequiredService<IUserService>();
        }
        public async Task LoginAsync(string email, string password)
        {
            await _service.LoginAsync(email, password);
        }

        public async Task RegisterAsync(string username, string email, string password)
        {
            await _service.RegisterAsync(username, email, password);
        }
    }
}
