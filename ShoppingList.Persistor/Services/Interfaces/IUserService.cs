using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface IUserService
    {
        Task LoginAsync(string email, string password, CancellationToken cancellationToken);
        Task RegisterAsync(string username, string email, string password, CancellationToken cancellationToken);
        Task LogoutAsync(CancellationToken cancellationToken);
        Task<User?> GetUserAsync(CancellationToken cancellationToken);
        Task DeleteUserAsync(int id, CancellationToken cancellationToken);
        Task UpdateUserAsync(int id, string? username, string? email, string? password, CancellationToken cancellationToken);
    }
}
