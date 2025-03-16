using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    internal interface ICommentService
    {
        Task PostCommentAsync(int household_id, int grocery_id, string content, CancellationToken cancellationToken = default);
        Task<IEnumerable<Comment>> GetCommentsAsync(int household_id, int grocery_id, CancellationToken cancellationToken = default);
        Task DeleteCommentAsync(int household_id, int grocery_id, int commment_id, CancellationToken cancellationToken = default);
    }
}
