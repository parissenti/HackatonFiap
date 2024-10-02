using MongoDB.Bson.Serialization.Attributes;

namespace AgendamentoMedico.Domain.Entitites
{
    public class Agenda : EntityBase
    {
        public Guid IdMedico { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateOnly Data { get; set; }
        public List<ConsultaAgendamento> Horarios { get; set; } = new List<ConsultaAgendamento>();

    }
}