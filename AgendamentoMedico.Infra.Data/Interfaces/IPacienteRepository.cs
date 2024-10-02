using AgendamentoMedico.Domain.Entitites;

namespace AgendamentoMedico.Infra.Data.Interfaces
{
    public interface IPacienteRepository
    {        
        Task MarcarConsulta(Guid idConsultaAgendamento, Guid idPaciente);
        Task<IEnumerable<ConsultaAgendamento>> ListarConsultas(Guid idPaciente);
     
    }
}
