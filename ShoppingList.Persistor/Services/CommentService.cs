using ShoppingList.Core;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    public class CommentService(HttpClient client) : APIService(client), ICommentService
    {
        public async Task DeleteCommentAsync(int household_id, int grocery_id, int commment_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"households/{household_id}/groceries/{grocery_id}/comments/{commment_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync(int household_id, int grocery_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"households/{household_id}/groceries/{grocery_id}/comments", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<Comment>? groceries = await response.Content.ReadFromJsonAsync<IEnumerable<Comment>>(cancellationToken);

            return groceries ?? throw new NullReferenceException(nameof(groceries));
        }

        public async Task PostCommentAsync(int household_id, int grocery_id, string content, CancellationToken cancellationToken = default)
        {
            var payload = new { content };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"households/{household_id}/groceries/{grocery_id}", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }
    }
}
