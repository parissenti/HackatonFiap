﻿using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Context;
using AgendamentoMedico.Infra.Data.Interfaces;
using MongoDB.Driver;

namespace AgendamentoMedico.Infra.Data.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly IMongoContext _context;

        public PacienteRepository(IMongoContext context)
        {
            _context = context;
        }

        public async Task MarcarConsulta(Guid idConsultaAgendamento, Guid idPaciente)
        {
            var filter = Builders<ConsultaAgendamento>.Filter.Eq(e => e.Id, idConsultaAgendamento);

            var updateLock = Builders<ConsultaAgendamento>.Update.Set(e => e.IsLocked, true);
            var result = await _context.ConsultaAgendamentos.UpdateOneAsync(filter, updateLock);

            if (result.ModifiedCount == 0)
            {
                throw new InvalidOperationException("Consulta já está sendo processada.");
            }

            try
            {
                var update = Builders<ConsultaAgendamento>.Update.Set(e => e.idPaciente, idPaciente)
                                                                 .Set(e => e.Disponivel, false);
                await _context.ConsultaAgendamentos.UpdateOneAsync(filter, update);
            }
            finally
            {
                await _context.ConsultaAgendamentos.UpdateOneAsync(filter, Builders<ConsultaAgendamento>.Update.Set(e => e.IsLocked, false));
            }

        }

        public async Task<IEnumerable<ConsultaAgendamento>> ListarConsultas(Guid idPaciente)
        {
            try
            {
                return await _context.ConsultaAgendamentos.Find(e => true && e.idPaciente.Equals(idPaciente) &&
                                                                e.DataConsulta >= DateTime.Now.Date).ToListAsync();
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }
    }
}
