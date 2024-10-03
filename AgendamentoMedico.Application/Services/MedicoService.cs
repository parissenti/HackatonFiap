using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Interfaces;

namespace AgendamentoMedico.Application.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _medicoRepository;

        public MedicoService(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        public async Task CadastrarHorariosDisponiveis(PeriodoAtendimento periodoAtendimento)
        {
            await _medicoRepository.CadastrarHorariosDisponiveis(periodoAtendimento);
        }

        public async Task LiberarAgenda(Guid idMedico, DateTime dataLiberar)
        {
            await _medicoRepository.LiberarAgenda(idMedico, dataLiberar);
        }

        public async Task<IEnumerable<PeriodoAtendimento>> ListarHorariosDisponiveis(Guid idMedico)
        {
            return await _medicoRepository.ListarPeriodoAtendimento(idMedico);
        }

        public async Task<IEnumerable<ConsultaAgendamento>> ListarAgenda(Guid idMedico)
        {
            return await _medicoRepository.ListarAgenda(idMedico);
        }

        public async Task<IEnumerable<Usuario>> ListarMedicos()
        {
            return await _medicoRepository.ListarMedicos();
        }

        public async Task<IEnumerable<ConsultaAgendamento>> BuscarConsultaPorDataeHorarioConsulta(DateTime dataConsulta, Guid idMedico)
        {
            return await _medicoRepository.BuscarConsultaPorDataeHorarioConsulta(dataConsulta, idMedico);
        }

    }
}
