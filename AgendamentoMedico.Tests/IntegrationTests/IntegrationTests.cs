using AgendamentoMedico.API;
using AgendamentoMedico.Application.DTOs.Usuario.Request;
using AgendamentoMedico.Application.DTOS.Medico.Request;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Domain.Enums;
using AgendamentoMedico.Tests.Fixture;
using EmprestimoLivros.Tests.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace AgendamentoMedico.Tests.IntegrationTests
{
    public class IntegrationTests : IntegrationTestBase
    {
        public IntegrationTests(WebApplicationFactory<Startup> factory) : base(factory)
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
            responseAuth.EnsureSuccessStatusCode();
            var responseBody = await responseAuth.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenEntity>(responseBody);
            var token = tokenResponse.Token;
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
            var responseBody = await responseAuth.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenEntity>(responseBody);
            var token = tokenResponse.Token;

            responseAuth.EnsureSuccessStatusCode();
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

        //[Fact]
        public async Task MarcarConsulta_Valido_Retorna_Ok()
        {
            var marcarConsulta = new MarcarConsulta
            {
                DataConsulta = DateTime.Parse("2024-10-06 13:00:00"),
                idMedico = Guid.NewGuid(),
                idPaciente = Guid.NewGuid()
            };

            var response = await _httpClient.PostAsJsonAsync("/Paciente/marcar-consulta", marcarConsulta);
            response.EnsureSuccessStatusCode();
        }

        //[Fact]
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
                DataConsulta = DateTime.Now,
                idMedico = Guid.NewGuid(),
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

            var responseAuth = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            responseAuth.EnsureSuccessStatusCode();
            var responseBody = await responseAuth.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenEntity>(responseBody);
            var token = tokenResponse.Token;

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

            var responseAuth = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            responseAuth.EnsureSuccessStatusCode();
            var responseBody = await responseAuth.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenEntity>(responseBody);
            var token = tokenResponse.Token;

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

            var responseAuth = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            responseAuth.EnsureSuccessStatusCode();
            var responseBody = await responseAuth.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenEntity>(responseBody);
            var token = tokenResponse.Token;
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

            var responseAuth = await _httpClient.PostAsJsonAsync("/Usuario/autenticar", loginDto);
            responseAuth.EnsureSuccessStatusCode();
            var responseBody = await responseAuth.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenEntity>(responseBody);
            var token = tokenResponse.Token;
            await DeletarAdminAsync(token, usuario.Email);
        }

        //[Fact]
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

        //[Fact]
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

        //[Fact]
        public async Task ListarUsuarios_UsuarioNaoAutenticado_Retorna_Unauthorized()
        {
            var response = await _httpClient.GetAsync("/Usuario/listar");

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        //[Fact]
        public async Task DeletarUsuario_UsuarioNaoAutenticado_Retorna_Unauthorized()
        {
            var response = await _httpClient.DeleteAsync("/Usuario/some-guid");

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
