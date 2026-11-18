using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging.Abstractions;
using ScreenSound.Web.Response;
using System.Net.Http.Json;
using System.Security.Claims;

namespace ScreenSound.Web.Services
{
    public class AuthAPI : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var pessoa = new ClaimsPrincipal();

            var info = await _httpClient.GetFromJsonAsync<InfoPessoaResponse>("auth/manage/info");

            if (info is not null)
            {
                Claim[] dados =
                    [
                        new Claim(ClaimTypes.Name, info.Email),
                        new Claim(ClaimTypes.Email, info.Email)
                    ];

                var identity = new ClaimsIdentity(dados, "Cookies");
                pessoa = new ClaimsPrincipal(identity);
            }

            return new AuthenticationState(pessoa);
        }

            public AuthAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }


        public async Task<AuthResponse> LoginAsync(string email, string senha)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login?useCookies=true", new
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
