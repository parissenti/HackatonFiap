using AgendamentoMedico.API;
using AgendamentoMedico.Application.DTOs.Usuario.Request;
using AgendamentoMedico.Application.DTOS.Medico.Request;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Tests.Fixture;
using EmprestimoLivros.Tests.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace AgendamentoMedico.Tests.IntegrationTests
{
    public class PacienteControllerTests : IntegrationTestBase
    {
        public PacienteControllerTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task MarcarConsulta_Valido_Retorna_Ok()
        {
            var marcarConsulta = new MarcarConsulta
            {
                idAgendamentoMedico = Guid.NewGuid(),
                idPaciente = Guid.NewGuid()
            };

            var response = await _httpClient.PostAsJsonAsync("/Paciente/marcar-consulta", marcarConsulta);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ListarConsultasAgendadas_UsuarioAutenticado_Retorna_Ok()
        {
            var usuario = UsuarioFaker.UsuarioFake[3];
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

            var responseAuth = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            responseAuth.EnsureSuccessStatusCode();
            var token = await responseAuth.Content.ReadAsStringAsync();

            DefinirAutenticacaoHeader(token);

            var pacienteId = usuario.Id; 
            var marcarConsulta = new MarcarConsulta
            {
                idAgendamentoMedico = Guid.NewGuid(),
                idPaciente = pacienteId
            };
            var responseMarcarConsulta = await _httpClient.PostAsJsonAsync("/Paciente/marcar-consulta", marcarConsulta);
            responseMarcarConsulta.EnsureSuccessStatusCode(); 

            var response = await _httpClient.GetAsync($"/Paciente/listar-consultas-agendadas/{pacienteId}");
            await DeletarAdminAsync(token, usuarioDTO.Email);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ListarConsultasAgendadas_UsuarioNaoAutenticado_Retorna_Unauthorized()
        {
            var pacienteId = Guid.NewGuid();

            var response = await _httpClient.GetAsync($"/Paciente/listar-consultas-agendadas/{pacienteId}");
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
