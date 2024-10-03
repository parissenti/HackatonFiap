using AgendamentoMedico.Application.Services;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Domain.Enums;
using AgendamentoMedico.Infra.Data.Interfaces;
using AgendamentoMedico.Tests.Fixture;
using Moq;
using Xunit;

namespace AgendamentoMedico.Tests.UnitTests
{
    public class MedicoUnitTests
    {
        private readonly MedicoService _medicoService;
        private readonly Mock<IMedicoRepository> _mockMedicoRepository;

        public MedicoUnitTests()
        {
            _mockMedicoRepository = new Mock<IMedicoRepository>();
            _medicoService = new MedicoService(_mockMedicoRepository.Object);
        }

        [Fact]
        public async Task CadastrarHorariosDisponiveis_DeveChamarCadastrarHorariosDisponiveisDoRepositorio()
        {
            var periodoAtendimento = PeriodoAtendimentoFaker.PeriodoAtendimentoFake[0];

            await _medicoService.CadastrarHorariosDisponiveis(periodoAtendimento);

            _mockMedicoRepository.Verify(repo => repo.CadastrarHorariosDisponiveis(periodoAtendimento), Times.Once);
        }

        [Fact]
        public async Task LiberarAgenda_DeveChamarLiberarAgendaDoRepositorio()
        {
            var idMedico = Guid.NewGuid();
            var dataLiberar = DateTime.Now;

            await _medicoService.LiberarAgenda(idMedico, dataLiberar);

            _mockMedicoRepository.Verify(repo => repo.LiberarAgenda(idMedico, dataLiberar), Times.Once);
        }

        [Fact]
        public async Task ListarHorariosDisponiveis_DeveRetornarListaDePeriodos()
        {
            var idMedico = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
            var periodosFicticios = PeriodoAtendimentoFaker.PeriodoAtendimentoFake;

            _mockMedicoRepository.Setup(repo => repo.ListarPeriodoAtendimento(idMedico)).ReturnsAsync(periodosFicticios);

            var resultado = await _medicoService.ListarHorariosDisponiveis(idMedico);

            Assert.NotNull(resultado);
            Assert.Equal(periodosFicticios.Count, resultado.Count());
            _mockMedicoRepository.Verify(repo => repo.ListarPeriodoAtendimento(idMedico), Times.Once);
        }

        [Fact]
        public async Task ListarAgenda_DeveRetornarListaDeConsultas()
        {
            var idMedico = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
            var consultasFicticias = ConsultaAgendamentoFaker.ConsultaAgendamentoFake;

            _mockMedicoRepository.Setup(repo => repo.ListarAgenda(idMedico)).ReturnsAsync(consultasFicticias);

            var resultado = await _medicoService.ListarAgenda(idMedico);

            Assert.NotNull(resultado);
            Assert.Equal(consultasFicticias.Count, resultado.Count());
            _mockMedicoRepository.Verify(repo => repo.ListarAgenda(idMedico), Times.Once);
        }

        [Fact]
        public async Task ListarMedicos_DeveRetornarListaDeMedicos()
        {
            var medicosFicticios = new List<Usuario>
            {
                new Usuario { Id = Guid.NewGuid(), Nome = "Dr. Pedro", Tipo = "Médico" },
                new Usuario { Id = Guid.NewGuid(), Nome = "Dra. Ana", Tipo = "Médica" }
            };

            _mockMedicoRepository.Setup(repo => repo.ListarMedicos()).ReturnsAsync(medicosFicticios);

            var resultado = await _medicoService.ListarMedicos();

            Assert.NotNull(resultado);
            Assert.Equal(medicosFicticios.Count, resultado.Count());
            _mockMedicoRepository.Verify(repo => repo.ListarMedicos(), Times.Once);
        }
    }
}
