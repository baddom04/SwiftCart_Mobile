namespace ShoppingList.Persistor;

public static class NetworkSettings
{
    public static Uri BaseAddress { get; }
    public static int HouseholdPerPage { get; } = 5;
    public static int StoresPerPage { get; } = 10;

    static NetworkSettings()
    {
        BaseAddress = new($"https://swiftcart-cfb8gcc8edhrb3h8.westeurope-01.azurewebsites.net/api/");
        //BaseAddress = new($"http://192.168.0.34:8000/api/");
    }
}
