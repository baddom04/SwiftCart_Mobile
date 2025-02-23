namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface IUserService
    {
        Task LoginAsync(string email, string password);
        Task RegisterAsync(string username, string email, string password);
        Task LogoutAsnyc();
        Task GetUserAsync();
        Task DeleteUserAsync();
        Task UpdateUserAsync();
    }
}
