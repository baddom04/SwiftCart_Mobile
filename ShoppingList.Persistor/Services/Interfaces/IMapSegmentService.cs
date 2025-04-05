using ShoppingList.Core;
using ShoppingList.Core.Enums;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface IMapSegmentService
    {
        Task<MapSegment> CreateMapSegmentAsync(int map_id, int x, int y, SegmentType type, int? section_id, CancellationToken cancellationToken = default);
        Task<IEnumerable<MapSegment>> GetMapSegmentAsync(int map_id, CancellationToken cancellationToken = default);
        Task<MapSegment> UpdateMapSegmentAsync(int map_id, int segment_id, int x, int y, SegmentType type, int? section_id, CancellationToken cancellationToken = default);
        Task DeleteMapSegmentAsync(int map_id, int segment_id, CancellationToken cancellationToken = default);
    }
}
