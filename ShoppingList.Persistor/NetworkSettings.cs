namespace ShoppingList.Persistor;

public static class NetworkSettings
{
    public static Uri BaseAddress { get; } = new("http://192.168.0.34:8000/api/");
    public static int HouseholdPerPage { get; } = 7;
}
