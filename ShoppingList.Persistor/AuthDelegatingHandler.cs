using System.Net.Http.Headers;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Persistor
{
    public class AuthDelegatingHandler(ITokenService tokenService) : DelegatingHandler
    {
        private readonly ITokenService _tokenService = tokenService;
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? token = await _tokenService.GetTokenAsync(cancellationToken);
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
