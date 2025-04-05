using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    internal class MapSegmentService(HttpClient client) : APIService(client), IMapSegmentService
    {
        public async Task<MapSegment> CreateMapSegmentAsync(int map_id, int x, int y, SegmentType type, int? section_id, CancellationToken cancellationToken = default)
        {
            var payload = new { x, y, type = type.ToString().ToLower(), section_id };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"maps/{map_id}/segments", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            MapSegment? mapSegments = await response.Content.ReadFromJsonAsync<MapSegment>(cancellationToken);

            return mapSegments ?? throw new NullReferenceException();
        }

        public async Task DeleteMapSegmentAsync(int map_id, int segment_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"maps/{map_id}/segments/{segment_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<IEnumerable<MapSegment>> GetMapSegmentAsync(int map_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"maps/{map_id}/segments", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<MapSegment>? mapSegments = await response.Content.ReadFromJsonAsync<IEnumerable<MapSegment>>(cancellationToken);

            return mapSegments ?? throw new NullReferenceException();
        }

        public async Task<MapSegment> UpdateMapSegmentAsync(int map_id, int segment_id, int x, int y, SegmentType type, int? section_id, CancellationToken cancellationToken = default)
        {
            var payload = new { x, y, type = type.ToString().ToLower(), section_id };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"maps/{map_id}/segments/{segment_id}", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            MapSegment? mapSegments = await response.Content.ReadFromJsonAsync<MapSegment>(cancellationToken);

            return mapSegments ?? throw new NullReferenceException();
        }
    }
}
