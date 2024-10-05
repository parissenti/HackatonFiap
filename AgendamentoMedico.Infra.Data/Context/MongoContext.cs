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
        private readonly MongoClient _client;

        public MongoContext(IOptions<MongoConfiguration> config)
        {
            _client = new MongoClient(config.Value.ConnectionString);
            _db = _client.GetDatabase(config.Value.Database);

            CriaCollectionSeNaoExistir<Usuario>("Usuarios").Wait();
            CriaCollectionSeNaoExistir<PeriodoAtendimento>("PeriodoAtendimentos").Wait();
            CriaCollectionSeNaoExistir<ConsultaAgendamento>("ConsultaAgendamentos").Wait();
        }
        private async Task CriaCollectionSeNaoExistir<T>(string nomeCollection)
        {
            Console.WriteLine($"Verificando se a coleção '{nomeCollection}' existe...");
            var filter = new BsonDocument("name", nomeCollection);
            var collections = await _db.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });

            if (!await collections.AnyAsync())
            {
                Console.WriteLine($"Coleção '{nomeCollection}' não encontrada. Criando agora...");
                await _db.CreateCollectionAsync(nomeCollection);
                Console.WriteLine($"Coleção '{nomeCollection}' criada com sucesso.");
            }
            else
            {
                Console.WriteLine($"Coleção '{nomeCollection}' já existe. Nenhuma ação necessária.");
            }
        }

        public async Task<IClientSessionHandle> StartSessionAsync()
        {
            return await _client.StartSessionAsync();
        }

        public IMongoCollection<Usuario> Usuarios => _db.GetCollection<Usuario>("Usuarios");
        public IMongoCollection<PeriodoAtendimento> PeriodoAtendimentos => _db.GetCollection<PeriodoAtendimento>("PeriodoAtendimentos");
        public IMongoCollection<ConsultaAgendamento> ConsultaAgendamentos => _db.GetCollection<ConsultaAgendamento>("ConsultaAgendamentos");
    }
}
