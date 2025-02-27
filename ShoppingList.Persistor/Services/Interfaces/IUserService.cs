using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface IUserService
    {
        Task LoginAsync(string email, string password, CancellationToken cancellationToken = default);
        Task RegisterAsync(string username, string email, string password, CancellationToken cancellationToken = default);
        Task LogoutAsync(CancellationToken cancellationToken = default);
        Task<User> GetUserAsync(CancellationToken cancellationToken = default);
        Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(int id, string? username, string? email, string? password, CancellationToken cancellationToken = default);
        Task UpdatePasswordAsync(int id, string currentPassword, string newPassword, CancellationToken cancellationToken = default);    }
}
