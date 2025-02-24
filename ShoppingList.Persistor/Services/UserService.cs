using ShoppingList.Core;
using ShoppingList.Persistor.ServerResponseHandling;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    public class UserService(HttpClient httpClient, ITokenService tokenService) : APIService(httpClient), IUserService
    {
        private readonly ITokenService _tokenService = tokenService;

        public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"users/{id}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException($"DeleteUserAsync failed: {response.StatusCode}, {errorContent}");
            }
        }

        public async Task<User?> GetUserAsync(CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "user");

            string? token = await _tokenService.GetTokenAsync(cancellationToken);
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException($"GetUserAsync failed: {response.StatusCode}, {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<User>(cancellationToken: cancellationToken);
        }

        public async Task LoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var credentials = new { email, password };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("login", credentials, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                ErrorResponse? errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken);
                if (errorResponse == null)
                    throw new NullReferenceException(nameof(errorResponse));

                throw new HttpRequestException($"{errorResponse.Error?.ToString()}");
            }

            UserToken? tokenResponse = await response.Content.ReadFromJsonAsync<UserToken>(cancellationToken: cancellationToken);
            if (tokenResponse == null)
                throw new NullReferenceException(nameof(tokenResponse));

            await _tokenService.SaveTokenAsync(tokenResponse.Token, cancellationToken);
        }

        public async Task LogoutAsync(CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("logout", new { }, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException($"LogoutAsync failed: {response.StatusCode}, {errorContent}");
            }
        }

        public async Task RegisterAsync(string username, string email, string password, CancellationToken cancellationToken = default)
        {
            var payload = new { name = username, email, password };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("register", payload, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                ErrorResponse? errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken);
                if (errorResponse == null)
                    throw new NullReferenceException(nameof(errorResponse));

                throw new HttpRequestException($"{errorResponse.Error?.ToString()}");
            }

            UserToken? tokenResponse = await response.Content.ReadFromJsonAsync<UserToken>(cancellationToken: cancellationToken);
            if (tokenResponse == null)
                throw new NullReferenceException(nameof(tokenResponse));

            await _tokenService.SaveTokenAsync(tokenResponse.Token, cancellationToken);
        }

        public async Task UpdateUserAsync(int id, string? username, string? email, string? password, CancellationToken cancellationToken = default)
        {
            var payload = new { username, email, password };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"users/{id}", payload, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException($"UpdateUserAsync failed: {response.StatusCode}, {errorContent}");
            }
        }
    }
}
