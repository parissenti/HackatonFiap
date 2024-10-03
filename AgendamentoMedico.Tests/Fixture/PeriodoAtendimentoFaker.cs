using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Domain.Enums;

namespace AgendamentoMedico.Tests.Fixture
{
    public class PeriodoAtendimentoFaker
    {
        public static List<PeriodoAtendimento> PeriodoAtendimentoFake { get; } = new List<PeriodoAtendimento>()
        {
            new PeriodoAtendimento
            {
                IdMedico = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                DiaDaSemana = DiasDaSemana.Segunda,
                Inicio = DateTime.Now,
                Fim = DateTime.Now.AddHours(2)
            },
            new PeriodoAtendimento
            {
                IdMedico = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                DiaDaSemana = DiasDaSemana.Terça,
                Inicio = DateTime.Now.AddHours(3),
                Fim = DateTime.Now.AddHours(5)
            },
            new PeriodoAtendimento
            {
                IdMedico = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                DiaDaSemana = DiasDaSemana.Quarta,
                Inicio = DateTime.Now.AddHours(3),
                Fim = DateTime.Now.AddHours(5)
            },
        };
    }
}
