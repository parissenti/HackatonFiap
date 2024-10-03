using AgendamentoMedico.Application.Services;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Interfaces;
using Moq;

namespace AgendamentoMedico.Tests.UnitTests
{
    public class PacienteUnitTests
    {

        private readonly PacienteService _pacienteService;
        private readonly Mock<IPacienteRepository> _mockPacienteRepository;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;

        public PacienteUnitTests()
        {
            _mockPacienteRepository = new Mock<IPacienteRepository>();
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _pacienteService = new PacienteService(_mockPacienteRepository.Object, _mockUsuarioRepository.Object);
        }

        [Fact]
        public async Task MarcarConsulta_DeveMarcarConsulta()
        {
            var idConsultaAgendamento = Guid.NewGuid();
            var idPaciente = Guid.NewGuid();

            await _pacienteService.MarcarConsulta(idConsultaAgendamento, idPaciente);

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
