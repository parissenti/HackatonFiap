using AgendamentoMedico.Application.Services;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Interfaces;
using Microsoft.Extensions.Options;
using Moq;

namespace AgendamentoMedico.Tests.UnitTests
{
    public class PacienteUnitTests
    {

        private readonly PacienteService _pacienteService;
        private readonly Mock<IPacienteRepository> _mockPacienteRepository;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly Mock<IMedicoRepository> _mockMedicoRepository;
        private readonly Mock<IOptions<EnvioEmail>> _mockEnvioEmailOptions;

        public PacienteUnitTests()
        {
            _mockPacienteRepository = new Mock<IPacienteRepository>();
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _mockMedicoRepository = new Mock<IMedicoRepository>();

            _mockEnvioEmailOptions = new Mock<IOptions<EnvioEmail>>();
            _mockEnvioEmailOptions.Setup(opt => opt.Value).Returns(new EnvioEmail
            {
                SmtpUserName = "fakeemail@example.com",
                GoogleAppPassword = "fakepassword"
            });

            _pacienteService = new PacienteService(
                _mockPacienteRepository.Object,
                _mockUsuarioRepository.Object,
                _mockMedicoRepository.Object,
                _mockEnvioEmailOptions.Object
            );
        }

        //[Fact]
        public async Task MarcarConsulta_DeveMarcarConsulta()
        {
            // Arrange
            var dateTime = DateTime.Parse("2024-10-06 13:00:00");
            var idConsultaAgendamento = Guid.NewGuid();
            var idPaciente = Guid.NewGuid();
            var consultaMock = new List<ConsultaAgendamento>
            {
                new ConsultaAgendamento { Id = idConsultaAgendamento, DataConsulta = dateTime }
            };

            var medicoMock = new Usuario { Id = idConsultaAgendamento, Email = "medico@email.com" };
            var pacienteMock = new Usuario { Id = idPaciente, Email = "paciente@email.com" };

            _mockMedicoRepository
                .Setup(repo => repo.BuscarConsultaPorDataeHorarioConsulta(dateTime, idConsultaAgendamento))
                .ReturnsAsync(consultaMock);

            _mockUsuarioRepository
                .Setup(repo => repo.BuscarUsuarioPorId(idConsultaAgendamento))
                .ReturnsAsync(medicoMock);

            _mockUsuarioRepository
                .Setup(repo => repo.BuscarUsuarioPorId(idPaciente))
                .ReturnsAsync(pacienteMock);

            _mockPacienteRepository
                .Setup(repo => repo.MarcarConsulta(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            await _pacienteService.MarcarConsulta(dateTime, idConsultaAgendamento, idPaciente);

            _mockPacienteRepository.Verify(repo => repo.MarcarConsulta(idConsultaAgendamento, idPaciente), Times.Once);
        }

        [Fact]
        public async Task ListarConsultas_DeveRetornarListaDeConsultas()
        {
            var idPaciente = Guid.NewGuid();
            var consultasFicticias = new List<ConsultaAgendamento>
        {
            new ConsultaAgendamento { Id = Guid.NewGuid(), DataConsulta = DateTime.Now, idMedico = Guid.NewGuid(), idPaciente = idPaciente },
            new ConsultaAgendamento { Id = Guid.NewGuid(), DataConsulta = DateTime.Now.AddDays(1), idMedico = Guid.NewGuid(), idPaciente = idPaciente }
        };

            _mockPacienteRepository.Setup(repo => repo.ListarConsultas(idPaciente)).ReturnsAsync(consultasFicticias);

            _mockUsuarioRepository.Setup(repo => repo.BuscarUsuarioPorId(It.IsAny<Guid>())).ReturnsAsync(new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva"
            });

            var resultado = await _pacienteService.ListarConsultas(idPaciente);

            Assert.NotNull(resultado);
            Assert.Equal(consultasFicticias.Count, resultado.Count());
            _mockPacienteRepository.Verify(repo => repo.ListarConsultas(idPaciente), Times.Once);
            _mockUsuarioRepository.Verify(repo => repo.BuscarUsuarioPorId(It.IsAny<Guid>()), Times.Exactly(consultasFicticias.Count * 2)); // Para médico e paciente
        }
        
    }
}
