using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Interfaces;

namespace AgendamentoMedico.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public PacienteService(IPacienteRepository pacienteRepository, IUsuarioRepository usuarioRepository)
        {
            _pacienteRepository = pacienteRepository;
            _usuarioRepository = usuarioRepository;
        }
        public async Task MarcarConsulta(Guid idConsultaAgendamento, Guid idPaciente)
        {
            await _pacienteRepository.MarcarConsulta(idConsultaAgendamento, idPaciente);
        }

        public async Task<IEnumerable<ConsultaAgendamentoResponse>> ListarConsultas(Guid idPaciente)
        {
            List<ConsultaAgendamentoResponse> consultas = new List<ConsultaAgendamentoResponse>();

            foreach (var consulta in await _pacienteRepository.ListarConsultas(idPaciente))
            {
                consultas.Add(new ConsultaAgendamentoResponse
                {
                    Id = consulta.Id,
                    DataConsulta = consulta.DataConsulta,
                    idMedico = consulta.idMedico,
                    idPaciente = consulta.idPaciente,
                    Medico = await _usuarioRepository.BuscarUsuarioPorId(consulta.idMedico),
                    Paciente = await _usuarioRepository.BuscarUsuarioPorId(consulta.idPaciente)
                });
            }

            return consultas;
        }
    }
}
