using AgendamentoMedico.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace AgendamentoMedico.Domain.Entitites
{
    public class PeriodoAtendimento : EntityBase
    {
        public Guid IdMedico { get; set; }
        public DiasDaSemana DiaDaSemana { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Inicio { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Fim { get; set; }
    }
}
