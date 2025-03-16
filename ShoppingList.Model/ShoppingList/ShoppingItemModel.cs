using ShoppingList.Persistor.Services.Interfaces;
using ShoppingList.Persistor;
using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core.Enums;
using ShoppingList.Core;

namespace ShoppingList.Model.ShoppingList
{
    public class ShoppingItemModel(Grocery grocery, int householdId)
    {
        private readonly IGroceryService _groceryService = AppServiceProvider.Services.GetRequiredService<IGroceryService>();
        private readonly ICommentService _commentService = AppServiceProvider.Services.GetRequiredService<ICommentService>();
        public Grocery Grocery { get; } = grocery;
        private IEnumerable<Comment> _comments = [];
        private readonly int _householdId = householdId;

        public async Task DeleteGroceryAsync()
        {
            await _groceryService.DeleteGroceryAsync(_householdId, Grocery.Id);
        }
        public async Task UpdateGroceryAsync(string? name = null, int? quantity = null, UnitType? unit = null, string? description = null)
        {
            await _groceryService.UpdateGroceryAsync(_householdId, Grocery.Id, name, quantity, unit, description);
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            _comments = await _commentService.GetCommentsAsync(_householdId, Grocery.Id);
            return _comments;
        }

        public async Task CreateCommentAsync(string content)
        {
            await _commentService.PostCommentAsync(_householdId, Grocery.Id, content);
        }

        public async Task DeleteCommentAsync(int comment_id)
        {
            await _commentService.DeleteCommentAsync(_householdId, Grocery.Id, comment_id);
        }
    }
}
