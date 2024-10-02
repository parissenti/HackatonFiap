using AgendamentoMedico.Domain.Entitites;

namespace AgendamentoMedico.Application.Interfaces
{
    public interface IMedicoService
    {
        Task<IEnumerable<Usuario>> ListarMedicos();
        Task CadastrarHorariosDisponiveis(PeriodoAtendimento periodoAtendimento);
        Task LiberarAgenda(Guid idMedico, DateTime dataLiberar);

        Task<IEnumerable<PeriodoAtendimento>> ListarHorariosDisponiveis(Guid idMedico);
        Task<IEnumerable<ConsultaAgendamento>> ListarAgenda(Guid idMedico);

    } 
}
