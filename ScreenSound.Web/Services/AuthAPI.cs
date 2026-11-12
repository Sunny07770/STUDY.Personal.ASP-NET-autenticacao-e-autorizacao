using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Services
{
    public class AuthAPI
    {
        private readonly HttpClient _httpClient;

            public AuthAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<AuthResponse> LoginAsync(string email, string senha)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", new
            {
                email,
                password = senha
            });

            if (response.IsSuccessStatusCode)
            {
                return new AuthResponse { Sucesso = true };
            }
            else
            {
                return new AuthResponse { Sucesso = false, Erros = ["login/senha inválidos"] };
            }
        }
    }
}
