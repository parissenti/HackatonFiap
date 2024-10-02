using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AgendamentoMedico.Infra.Data.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _db;

        public MongoContext(IOptions<MongoConfiguration> config)
        {
            var client = new MongoClient(config.Value.ConnectionString);
            _db = client.GetDatabase(config.Value.Database);

            CriaCollectionSeNaoExistir<Usuario>("Usuarios").Wait();
            CriaCollectionSeNaoExistir<PeriodoAtendimento>("PeriodoAtendimentos").Wait();
            CriaCollectionSeNaoExistir<ConsultaAgendamento>("ConsultaAgendamentos").Wait();
        }

        private async Task CriaCollectionSeNaoExistir<T>(string nomeCollection)
        {
            await Task.Run(() =>
            {
                var filter = new BsonDocument("name", nomeCollection);
                var collections = _db.ListCollections(new ListCollectionsOptions { Filter = filter });

                if (!collections.Any())
                    _db.CreateCollection(nomeCollection);
            });
        }

        public IMongoCollection<Usuario> Usuarios => _db.GetCollection<Usuario>("Usuarios");
        public IMongoCollection<PeriodoAtendimento> PeriodoAtendimentos => _db.GetCollection<PeriodoAtendimento>("PeriodoAtendimentos");
        public IMongoCollection<ConsultaAgendamento> ConsultaAgendamentos => _db.GetCollection<ConsultaAgendamento>("ConsultaAgendamentos");
    }
}
