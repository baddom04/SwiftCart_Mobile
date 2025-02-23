namespace ShoppingList.Persistor
{
    public static class PlatformServiceRegistry
    {
        private static readonly Dictionary<Type, Type> serviceTypes = [];

        public static void Register<T, T2>() where T2 : T
        {
            serviceTypes[typeof(T)] = typeof(T2);
        }

        public static Type Resolve<T>()
        {
            if (!serviceTypes.TryGetValue(typeof(T), out var implementationType))
            {
                throw new InvalidOperationException($"No platform-specific implementation registered for {typeof(T).Name}.");
            }
            return implementationType;
        }
    }
}
