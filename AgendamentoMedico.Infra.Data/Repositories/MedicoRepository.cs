using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Context;
using AgendamentoMedico.Infra.Data.Interfaces;
using MongoDB.Driver;

namespace AgendamentoMedico.Infra.Data.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly IMongoContext _context;

        public MedicoRepository(IMongoContext context)
        {
            _context = context;
        }

        public async Task CadastrarHorariosDisponiveis(PeriodoAtendimento periodoAtendimento)
        {
            try
            {
                await _context.PeriodoAtendimentos.InsertOneAsync(periodoAtendimento);
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }

        public async Task LiberarAgenda(Guid idMedico, DateTime dataLiberar)
        {
            var periodoAtendimentos = await _context.PeriodoAtendimentos.Find(e => true &&
                                                                        e.IdMedico.Equals(idMedico) &&
                                                                        e.Inicio >= (dataLiberar) && e.Inicio < dataLiberar.AddDays(1)).ToListAsync();
            var medico = await _context.Usuarios.Find(e => e.Id.Equals(idMedico)).FirstOrDefaultAsync();

            foreach (var periodo in periodoAtendimentos)
            {
                List<ConsultaAgendamento> horarios = GerarAgenda(periodo.Inicio, periodo.Fim, TimeSpan.FromMinutes(medico.TempoDeConsulta));
                Agenda agendaMedico = new Agenda();
                agendaMedico.Data = DateOnly.FromDateTime(periodo.Inicio);
                agendaMedico.Horarios.AddRange(horarios);
                medico.AgendaDiaria.Add(agendaMedico);
                foreach (ConsultaAgendamento consultaAgendamento in horarios)
                {
                    consultaAgendamento.idMedico = idMedico;
                    await _context.ConsultaAgendamentos.InsertOneAsync(consultaAgendamento);
                }
            }
        }

        private List<ConsultaAgendamento> GerarAgenda(DateTime inicio, DateTime fim, TimeSpan intervalo)
        {
            List<ConsultaAgendamento> horarios = new List<ConsultaAgendamento>();

            for (DateTime hora = inicio; hora < fim; hora = hora.Add(intervalo))
            {
                ConsultaAgendamento consulta = new ConsultaAgendamento();
                consulta.DataConsulta = hora;
                consulta.Disponivel = true;

                Random random = new Random();
                // consulta.Disponivel = random.Next(0, 2).Equals(1);
                horarios.Add(consulta);
            }

            return horarios;
        }

        public async Task<IEnumerable<ConsultaAgendamento>> ListarAgenda(Guid idMedico)
        {
            try
            {
                return await _context.ConsultaAgendamentos.Find(e => true && e.idMedico.Equals(idMedico)).ToListAsync();
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }
        public async Task<IEnumerable<PeriodoAtendimento>> ListarPeriodoAtendimento(Guid idMedico)
        {
            try
            {
                return await _context.PeriodoAtendimentos.Find(e => true && e.IdMedico.Equals(idMedico)).ToListAsync();
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }

        public async Task<IEnumerable<PeriodoAtendimento>> ListarHorariosDisponiveis(Guid idMedico)
        {
            try
            {
                return await _context.PeriodoAtendimentos.Find(e => true && e.IdMedico.Equals(idMedico)).ToListAsync();
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }

        public async Task<IEnumerable<Usuario>> ListarMedicos()
        {
            try
            {
                return await _context.Usuarios.Find(e => true && e.Tipo.Equals("M")).ToListAsync();
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }

        public async Task<IEnumerable<ConsultaAgendamento>> BuscarConsultaPorDataeHorarioConsulta(DateTime dataConsulta, Guid idMedico)
        {
            try
            {
                return await _context.ConsultaAgendamentos.Find(e => true && e.DataConsulta.Equals(dataConsulta) && e.idMedico.Equals(idMedico) && e.Disponivel.Equals(true)).ToListAsync();
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }
    }
}
