namespace ShoppingList.Persistor;

public static class NetworkSettings
{
    public static Uri BaseAddress { get; }
    public static int HouseholdPerPage { get; } = 5;
    public static int StoresPerPage { get; } = 10;

    static NetworkSettings()
    {
        //IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
        //IPAddress? ipv4Address = localIPs.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

        //BaseAddress = ipv4Address is not null ? new($"http://{ipv4Address}:8000/api/") : new($"http://192.168.0.34:8000/api/");
         BaseAddress = new($"http://10.42.223.94:8000/api/");
    }
}
