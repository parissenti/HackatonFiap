using AgendamentoMedico.Domain.Entitites;

namespace AgendamentoMedico.Tests.Fixture
{
    public class ConsultaAgendamentoFaker
    {
        public static List<ConsultaAgendamento> ConsultaAgendamentoFake { get; } = new List<ConsultaAgendamento>()
        {
            new ConsultaAgendamento { 
                Id = Guid.NewGuid(),
                DataConsulta = DateTime.Now,
                idMedico = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                idPaciente = Guid.NewGuid()
            },
            new ConsultaAgendamento { 
                Id = Guid.NewGuid(),
                DataConsulta = DateTime.Now.AddDays(1),
                idMedico = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                idPaciente = Guid.NewGuid()
            }
        };
    }
}
