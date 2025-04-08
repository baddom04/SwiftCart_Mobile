using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface ILocationService
    {
        Task<Location> CreateLocationAsync(int store_id, string country, string zip_code, string city, string street, string? detail, CancellationToken cancellationToken = default);
        Task<Location> GetLocationAsync(int store_id, CancellationToken cancellationToken = default);
        Task<Location> UpdateLocationAsync(int store_id, string country, string zip_code, string city, string street, string? detail, CancellationToken cancellationToken = default);
        Task DeleteLocationAsync(int store_id, CancellationToken cancellationToken = default);
        //Task<IEnumerable<string>> GetPossibleCountriesAsync(CancellationToken cancellationToken = default);
        //Task<IEnumerable<string>> GetPossibleCitiesAsync(string country, CancellationToken cancellationToken = default);
        //Task<IEnumerable<string>> GetPossibleStreetsAsync(string country, string city, CancellationToken cancellationToken = default);
        //Task<IEnumerable<string>> GetPossibleDetailsAsync(string country, string city, string street, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetPossiblesAsync(CancellationToken cancellationToken = default, params string[] parameters);
    }
}
