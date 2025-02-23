namespace ShoppingList.Persistor.Services
{
    public abstract class APIService(HttpClient httpClient)
    {
        protected readonly HttpClient _httpClient = httpClient;
    }
}