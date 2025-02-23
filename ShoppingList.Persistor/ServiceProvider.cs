using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Persistor.Services;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Headers;

namespace ShoppingList.Persistor
{
    public static class ServiceProvider
    {
        public static IServiceProvider Services { get; private set; }
        static ServiceProvider()
        {
            ServiceCollection services = new();

            services.AddTransient<AuthDelegatingHandler>();
            services.AddTransient<IFileService, FileService>();

            services.AddTransient(typeof(ITokenService), PlatformServiceRegistry.Resolve<ITokenService>());

            services.AddHttpClient<IUserService, UserService>(client =>
            {
                client.BaseAddress = NetworkSettings.BaseAddress;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddHttpMessageHandler<AuthDelegatingHandler>();

            Services = services.BuildServiceProvider();
        }
    }
}
