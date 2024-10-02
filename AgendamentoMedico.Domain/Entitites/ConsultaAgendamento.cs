using MongoDB.Bson.Serialization.Attributes;

namespace AgendamentoMedico.Domain.Entitites
{
    public class ConsultaAgendamento : EntityBase
    {
        public Guid idMedico { get; set; }
        public Guid idPaciente { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataConsulta { get; set; }
        public bool Disponivel { get; set; }
    }
}