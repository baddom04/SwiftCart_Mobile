using ShoppingList.Persistor.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ShoppingList.Android
{
    internal class AndroidTokenService : ITokenService
    {
        private readonly string _tokenKey = "auth_token";
        public Task ClearTokenAsync()
        {
            SecureStorage.Remove(_tokenKey);
            return Task.CompletedTask;
        }

        public async Task<string?> GetTokenAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await SecureStorage.GetAsync(_tokenKey);
            }
            catch
            {
                return null;
            }
        }

        public async Task SaveTokenAsync(string token, CancellationToken cancellationToken)
        {
            await SecureStorage.SetAsync(_tokenKey, token);
        }
    }
}
