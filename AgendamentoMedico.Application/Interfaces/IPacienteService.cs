using AgendamentoMedico.Domain.Entitites;

namespace AgendamentoMedico.Application.Interfaces
{
    public interface IPacienteService
    {
        Task MarcarConsulta(DateTime dataConsulta, Guid idMedico, Guid idPaciente);
        Task<IEnumerable<ConsultaAgendamentoResponse>> ListarConsultas(Guid idPaciente);

    }
}
