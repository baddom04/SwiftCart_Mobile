using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Persistor.Services;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Headers;

namespace ShoppingList.Persistor
{
    public static class AppServiceProvider
    {
        public static IServiceProvider Services { get; private set; }
        static AppServiceProvider()
        {
            ServiceCollection services = new();

            services.AddTransient<AuthDelegatingHandler>();
            services.AddTransient<IFileService, FileService>();

            services.AddTransient(typeof(ITokenService), PlatformServiceRegistry.Resolve<ITokenService>());

            static void configureClient(HttpClient client)
            {
                client.BaseAddress = NetworkSettings.BaseAddress;
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }

            services.AddHttpClient<IUserService, UserService>(configureClient).AddHttpMessageHandler<AuthDelegatingHandler>();
            services.AddHttpClient<IHouseholdService, HouseholdService>(configureClient).AddHttpMessageHandler<AuthDelegatingHandler>();
            services.AddHttpClient<IApplicationService, ApplicationService>(configureClient).AddHttpMessageHandler<AuthDelegatingHandler>();
            services.AddHttpClient<IGroceryService, GroceryService>(configureClient).AddHttpMessageHandler<AuthDelegatingHandler>();
            services.AddHttpClient<ICommentService, CommentService>(configureClient).AddHttpMessageHandler<AuthDelegatingHandler>();

            Services = services.BuildServiceProvider();
        }
    }
}
