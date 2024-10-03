using AgendamentoMedico.Domain.Entitites;

namespace AgendamentoMedico.Infra.Data.Interfaces
{
    public interface IMedicoRepository
    {
        Task CadastrarHorariosDisponiveis(PeriodoAtendimento periodoAtendimento);
        Task LiberarAgenda(Guid idMedico, DateTime dataLiberar);
        Task<IEnumerable<Usuario>> ListarMedicos();
        Task<IEnumerable<PeriodoAtendimento>> ListarPeriodoAtendimento(Guid idMedico);
        Task<IEnumerable<ConsultaAgendamento>> ListarAgenda(Guid idMedico);
        Task<IEnumerable<ConsultaAgendamento>> BuscarConsultaPorDataeHorarioConsulta(DateTime dataConsulta, Guid idMedico);

    }
}
