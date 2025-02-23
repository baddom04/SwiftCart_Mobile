namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface ITokenService
    {
        Task SaveTokenAsync(string token, CancellationToken cancellationToken);
        Task<string?> GetTokenAsync(CancellationToken cancellationToken);
        Task ClearTokenAsync();
    }
}
