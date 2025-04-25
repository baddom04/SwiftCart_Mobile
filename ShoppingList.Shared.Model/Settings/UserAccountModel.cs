﻿using ShoppingList.Core;
using ShoppingList.Persistor.Services.Interfaces;
using ShoppingList.Persistor;
using Microsoft.Extensions.DependencyInjection;

namespace ShoppingList.Shared.Model.Settings
{
    public class UserAccountModel
    {
        public User? User { get; private set; }

        private readonly IUserService _service = AppServiceProvider.Services.GetRequiredService<IUserService>();

        public async Task LoginAsync(string email, string password)
        {
            await _service.LoginAsync(email, password);
        }
        public async Task RegisterAsync(string username, string email, string password)
        {
            await _service.RegisterAsync(username, email, password);
        }
        public async Task<User> GetUserAsync(bool forceQuery = false)
        {
            if (!forceQuery)
                return User ??= await _service.GetUserAsync();
            else
                User = await _service.GetUserAsync();

            return User;
        }
        public async Task LogoutAsync()
        {
            await _service.LogoutAsync();
            User = null;
        }
        public async Task DeleteUserAsync()
        {
            if (User is null)
                throw new NullReferenceException(nameof(User));

            await _service.DeleteUserAsync(User.Id);

            User = null;
        }
        public async Task UpdateUser(string? username, string? email, string? password)
        {
            if (User is null)
                throw new NullReferenceException(nameof(User));

            await _service.UpdateUserAsync(User.Id, username, email, password);
        }
        public async Task UpdatePassword(string currentPassword, string newPassword)
        {
            if (User is null)
                throw new NullReferenceException(nameof(User));

            await _service.UpdatePasswordAsync(User.Id, currentPassword, newPassword);
        }
    }
}
