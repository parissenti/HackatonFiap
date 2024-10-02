using MongoDB.Bson.Serialization.Attributes;

namespace AgendamentoMedico.Domain.Entitites
{
    public class EntityBase
    {
        [BsonId]
        public Guid Id { get; set; }
        public EntityBase()
        {
            Id = new Guid();
        }
    }
}
