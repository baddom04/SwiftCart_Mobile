using ShoppingList.Core;
using ShoppingList.Persistor.ServerResponseHandling;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    internal class UserService(HttpClient httpClient, ITokenService tokenService) : APIService(httpClient), IUserService
    {
        private readonly ITokenService _tokenService = tokenService;

        private async Task ValidateUser(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            await ValidateResponse(response, cancellationToken);

            UserToken tokenResponse = await response.Content.ReadFromJsonAsync<UserToken>(cancellationToken) 
                ?? throw new NullReferenceException(nameof(tokenResponse)); ;

            await _tokenService.SaveTokenAsync(tokenResponse.Token, cancellationToken);
        }

        public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"users/{id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<User> GetUserAsync(CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync("user", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            User? user = await response.Content.ReadFromJsonAsync<User>(cancellationToken: cancellationToken);

            return user ?? throw new NullReferenceException(nameof(user));
        }

        public async Task LoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var credentials = new { email, password };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("login", credentials, cancellationToken);

            await ValidateUser(response, cancellationToken);
        }

        public async Task LogoutAsync(CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("logout", new { }, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task RegisterAsync(string username, string email, string password, CancellationToken cancellationToken = default)
        {
            var payload = new { name = username, email, password };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("register", payload, cancellationToken);

            await ValidateUser(response, cancellationToken);
        }

        public async Task UpdateUserAsync(int id, string? username, string? email, string? password, CancellationToken cancellationToken = default)
        {
            var payload = new { name = username, email, password };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"users/{id}", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task UpdatePasswordAsync(int id, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            var payload = new { new_password = newPassword, current_password = currentPassword };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"users/{id}/password", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }
    }
}
