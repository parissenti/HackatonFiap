using AgendamentoMedico.Domain.Entitites;

namespace AgendamentoMedico.Application.Interfaces
{
    public interface IPacienteService
    {
        Task MarcarConsulta(Guid idConsultaAgendamento, Guid idPaciente);
        Task<IEnumerable<ConsultaAgendamentoResponse>> ListarConsultas(Guid idPaciente);

    } 
}
