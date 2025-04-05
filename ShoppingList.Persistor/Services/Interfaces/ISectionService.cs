using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface ISectionService
    {
        Task<Section> CreateSectionAsync(int map_id, string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Section>> GetSectionAsync(int map_id, CancellationToken cancellationToken = default);
        Task<Section> UpdateSectionAsync(int map_id, int section_id, string name, CancellationToken cancellationToken = default);
        Task DeleteSectionAsync(int map_id, int section_id, CancellationToken cancellationToken = default);
    }
}
