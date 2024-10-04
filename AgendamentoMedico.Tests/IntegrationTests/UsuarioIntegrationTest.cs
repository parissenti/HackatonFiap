using AgendamentoMedico.API;
using AgendamentoMedico.Application.DTOs.Usuario.Request;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Tests.Fixture;
using EmprestimoLivros.Tests.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;


namespace AgendamentoMedico.Tests.IntegrationTests
{
    public class UsuarioControllerTests : IntegrationTestBase
    {
        public UsuarioControllerTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task AutenticarUsuario_Valido_Retorna_Ok()
        {
            var usuario = UsuarioFaker.UsuarioFake[0];

            var usuarioDTO = new UsuarioPacienteRequest
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Cpf = usuario.Cpf
            };
            await CriarUsuarioAsync(usuarioDTO);

            var loginDto = new UsuarioLogin
            {
                Email = usuarioDTO.Email,
                Senha = usuarioDTO.Senha
            };

            var response = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();
            await DeletarAdminAsync(token, usuarioDTO.Email);
        }

        [Fact]
        public async Task AutenticarUsuario_Email_Incorreto_Retorna_BadRequest()
        {
            var loginDto = new UsuarioLogin
            {
                Email = "email_incorreto@email.com",
                Senha = "123@senha"
            };

            var response = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AutenticarUsuario_Senha_Incorreta_Retorna_BadRequest()
        {
            var usuarioDTO = new UsuarioPacienteRequest
            {
                Nome = "Usuario Teste",
                Email = "usuario_teste@email.com",
                Senha = "123@senha",
                Cpf = "12345678901"
            };
            await CriarUsuarioAsync(usuarioDTO);

            var loginDto = new UsuarioLogin
            {
                Email = usuarioDTO.Email,
                Senha = "senha_incorreta"
            };

            var response = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CadastrarMedico_Valido_Retorna_Ok()
        {
            var usuario = UsuarioFaker.UsuarioFake[0];
            var usuarioMedico = new UsuarioMedicoRequest
            {
                Nome = usuario.Nome,
                Cpf = usuario.Cpf,
                Crm = usuario.Crm,
                TempoDeConsulta = 30,
                Email = usuario.Email,
                Senha = usuario.Senha
            };

            var responseCreate = await _httpClient.PostAsJsonAsync("/Usuario/cadastrar-medico", usuarioMedico);
            responseCreate.EnsureSuccessStatusCode();

            var loginDto = new UsuarioLogin
            {
                Email = usuarioMedico.Email,
                Senha = usuarioMedico.Senha
            };

            var response = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();
            await DeletarAdminAsync(token, usuarioMedico.Email);
        }

        [Fact]
        public async Task CadastrarMedico_Email_Existente_Retorna_BadRequest()
        {
            var usuario = UsuarioFaker.UsuarioFake[2];
            var usuarioMedico = new UsuarioMedicoRequest
            {
                Nome = usuario.Nome,
                Cpf = usuario.Cpf,
                Crm = usuario.Crm,
                TempoDeConsulta = 30,
                Email = usuario.Email,
                Senha = usuario.Senha
            };

            var responseCadastro = await _httpClient.PostAsJsonAsync("/Usuario/cadastrar-medico", usuarioMedico);
            responseCadastro.EnsureSuccessStatusCode();

            var response = await _httpClient.PostAsJsonAsync("/Usuario/cadastrar-medico", usuarioMedico);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

            var loginDto = new UsuarioLogin
            {
                Email = usuario.Email,
                Senha = usuario.Senha
            };

            var response_login = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            response_login.EnsureSuccessStatusCode();

            var token = await response_login.Content.ReadAsStringAsync();
            await DeletarAdminAsync(token, usuarioMedico.Email);
        }

        [Fact]
        public async Task CadastrarPaciente_Valido_Retorna_Ok()
        {
            var usuario = UsuarioFaker.UsuarioFake[2];

            var usuarioDTO = new UsuarioPacienteRequest
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Cpf = usuario.Cpf
            };

            var response = await _httpClient.PostAsJsonAsync("/Usuario/cadastrar-paciente", usuarioDTO);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao cadastrar paciente: {errorMessage}");
            }

            response.EnsureSuccessStatusCode();

            var loginDto = new UsuarioLogin
            {
                Email = usuario.Email,
                Senha = usuario.Senha
            };

            var response_login = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            var token = await response_login.Content.ReadAsStringAsync();
            await DeletarAdminAsync(token, usuario.Email);
        }

        [Fact]
        public async Task CadastrarPaciente_Email_Existente_Retorna_BadRequest()
        {
            var usuario = UsuarioFaker.UsuarioFake[0];

            var usuarioDTO = new UsuarioPacienteRequest
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Cpf = usuario.Cpf
            };

            await _httpClient.PostAsJsonAsync("/Usuario/cadastrar-paciente", usuarioDTO);

            var response = await _httpClient.PostAsJsonAsync("/Usuario/cadastrar-paciente", usuarioDTO);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            
            var loginDto = new UsuarioLogin
            {
                Email = usuario.Email,
                Senha = usuario.Senha
            };

            var response_login = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            response_login.EnsureSuccessStatusCode();

            var token = await response_login.Content.ReadAsStringAsync();
            await DeletarAdminAsync(token, usuario.Email);
        }

        [Fact]
        public async Task ListarUsuarios_UsuarioAutenticado_Retorna_Ok()
        {
            var usuario = UsuarioFaker.UsuarioFake[0];

            var usuarioDTO = new UsuarioPacienteRequest
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Cpf = usuario.Cpf
            };
            await CriarUsuarioAsync(usuarioDTO);

            var loginDto = new UsuarioLogin
            {
                Email = usuarioDTO.Email,
                Senha = usuarioDTO.Senha
            };

            var response_auth = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            var token = await response_auth.Content.ReadAsStringAsync();
            DefinirAutenticacaoHeader(token);
            var response = await _httpClient.GetAsync("/Usuario/listar");

            response.EnsureSuccessStatusCode();
            await DeletarAdminAsync(token, usuario.Email);
        }

        [Fact]
        public async Task ListarUsuarios_UsuarioNaoAutenticado_Retorna_Unauthorized()
        {
            var response = await _httpClient.GetAsync("/Usuario/listar");

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DeletarUsuario_UsuarioNaoAutenticado_Retorna_Unauthorized()
        {
            var response = await _httpClient.DeleteAsync("/Usuario/some-guid");

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
