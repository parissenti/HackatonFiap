using AgendamentoMedico.API;
using AgendamentoMedico.Application.DTOs.Usuario.Request;
using AgendamentoMedico.Domain.Entitites;
using Microsoft.AspNetCore.Mvc.Testing;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace EmprestimoLivros.Tests.IntegrationTests
{
    public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly HttpClient _httpClient;
        protected readonly WebApplicationFactory<Startup> _factory;

        public IntegrationTestBase(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        public string GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            foreach (var jwtClaim in jwtToken.Claims)
            {
                Console.WriteLine($"Claim Type: {jwtClaim.Type}, Claim Value: {jwtClaim.Value}");
            }

            var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
            return claim?.Value;
        }

        protected async Task CriarUsuarioAsync(UsuarioPacienteRequest usuarioDTO)
        {
            await _httpClient.PostAsJsonAsync("/Usuario/cadastrar-paciente", usuarioDTO);
        }

        protected async Task<string> ObterTokenAutenticacaoAsync(
            string email = "usuario_teste@email.com",
            string senha = "123@senha"
        )
        {
            var usuarioDTO = new UsuarioPacienteRequest
            {
                Nome = "Admin",
                Email = email,
                Senha = senha,
            };

            await CriarUsuarioAsync(usuarioDTO);

            var loginDto = new UsuarioLogin { Email = email, Senha = senha };
            var response = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<dynamic>();

            return result;
        }

        protected async Task DeletarUsuarioAsync(Guid? id = null)
        {
            var response = await _httpClient.DeleteAsync($"/Usuario/{id.Value}");
            if (!response.IsSuccessStatusCode)
            {
                var deleteError = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao deletar usuário {id}: {deleteError}");
            }
            
        }
        protected void DefinirAutenticacaoHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        protected async Task DeletarAdminAsync(string token = null, string email = null)
        {
            DefinirAutenticacaoHeader(token);
            var listarResponse = await _httpClient.GetAsync("/Usuario/listar");
            listarResponse.EnsureSuccessStatusCode();

            var usuariosJson = await listarResponse.Content.ReadAsStringAsync();

            var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(usuariosJson);

            var usuarioEncontrado = usuarios?.FirstOrDefault(u => u.Email == email);
            await DeletarUsuarioAsync(usuarioEncontrado?.Id);
        }
    }
}
