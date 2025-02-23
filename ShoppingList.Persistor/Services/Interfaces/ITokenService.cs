namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface ITokenService
    {
        Task SaveTokenAsync(string token);
        Task<string?> GetTokenAsync();
        Task ClearTokenAsync();
    }
}
