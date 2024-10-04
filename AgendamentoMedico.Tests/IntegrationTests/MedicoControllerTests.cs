using AgendamentoMedico.API;
using AgendamentoMedico.Application.DTOs.Usuario.Request;
using AgendamentoMedico.Application.DTOS.Medico.Request;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Domain.Enums;
using AgendamentoMedico.Tests.Fixture;
using EmprestimoLivros.Tests.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;

namespace AgendamentoMedico.Tests.IntegrationTests
{
    public class MedicoControllerTests : IntegrationTestBase
    {
        public MedicoControllerTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task CadastrarHorariosDisponiveis_Valido_Retorna_Ok()
        {
            var periodoAtendimento = new PeriodoAtendimento
            {
                IdMedico = Guid.NewGuid(),
                DiaDaSemana = DiasDaSemana.Segunda,
                Inicio = DateTime.Now,
                Fim = DateTime.Now.AddHours(8)
            };

            var response = await _httpClient.PostAsJsonAsync("/Medico/cadastrar-periodo-atendimento", periodoAtendimento);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ListarPeriodoAtendimentos_UsuarioAutenticado_Retorna_Ok()
        {
            var usuario = UsuarioFaker.UsuarioFake[3];
            var usuarioDTO = new UsuarioPacienteRequest
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Cpf = usuario.Cpf,
            };
            await CriarUsuarioAsync(usuarioDTO);

            var loginDto = new UsuarioLogin
            {
                Email = usuarioDTO.Email,
                Senha = usuarioDTO.Senha
            };

            var responseAuth = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            responseAuth.EnsureSuccessStatusCode();
            var token = await responseAuth.Content.ReadAsStringAsync();
            DefinirAutenticacaoHeader(token);

            var periodoAtendimento = new PeriodoAtendimento
            {
                IdMedico = Guid.NewGuid(), 
                DiaDaSemana = DiasDaSemana.Segunda,
                Inicio = DateTime.Now,
                Fim = DateTime.Now.AddHours(8)
            };
            await _httpClient.PostAsJsonAsync("/Medico/cadastrar-periodo-atendimento", periodoAtendimento);

            var response = await _httpClient.GetAsync($"/Medico/listar-periodo-atendimento/{periodoAtendimento.IdMedico}");
            response.EnsureSuccessStatusCode();
            await DeletarAdminAsync(token, usuarioDTO.Email);
        }

        [Fact]
        public async Task ListarPeriodoAtendimentos_UsuarioNaoAutenticado_Retorna_Unauthorized()
        {
            var medicoId = Guid.NewGuid();

            var response = await _httpClient.GetAsync($"/Medico/listar-periodo-atendimento/{medicoId}");
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task LiberarAgenda_Valido_Retorna_Ok()
        {
            var liberarAgenda = new LiberarAgenda
            {
                idMedico = Guid.NewGuid(),
                dataLiberar = DateTime.Now
            };

            var response = await _httpClient.PostAsJsonAsync("/Medico/liberar-agenda", liberarAgenda);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ListarAgenda_UsuarioAutenticado_Retorna_Ok()
        {
            var usuario = UsuarioFaker.UsuarioFake[3];
            var usuarioDTO = new UsuarioPacienteRequest
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Cpf = usuario.Cpf,
            };
            await CriarUsuarioAsync(usuarioDTO);

            var loginDto = new UsuarioLogin
            {
                Email = usuarioDTO.Email,
                Senha = usuarioDTO.Senha
            };

            var responseAuth = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            responseAuth.EnsureSuccessStatusCode();
            var token = await responseAuth.Content.ReadAsStringAsync();
            DefinirAutenticacaoHeader(token);

            var response = await _httpClient.GetAsync($"/Medico/listar-agenda/{usuario.Id}");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ListarAgenda_UsuarioNaoAutenticado_Retorna_Unauthorized()
        {
            var medicoId = Guid.NewGuid();

            var response = await _httpClient.GetAsync($"/Medico/listar-agenda/{medicoId}");
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
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
                Cpf = usuario.Cpf,
            };
            await CriarUsuarioAsync(usuarioDTO);

            var loginDto = new UsuarioLogin
            {
                Email = usuarioDTO.Email,
                Senha = usuarioDTO.Senha
            };

            var responseAuth = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            var token = await responseAuth.Content.ReadAsStringAsync();
            DefinirAutenticacaoHeader(token);

            var response = await _httpClient.GetAsync("/Medico/listar");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ListarUsuarios_UsuarioNaoAutenticado_Retorna_Unauthorized()
        {
            var response = await _httpClient.GetAsync("/Medico/listar");
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
