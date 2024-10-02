using MongoDB.Bson.Serialization.Attributes;

namespace AgendamentoMedico.Domain.Entitites
{
    public class ConsultaAgendamentoResponse : EntityBase
    {
        public Guid idMedico { get; set; }
        public Usuario Medico { get; set; }
        public Guid idPaciente { get; set; }
        public Usuario Paciente { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataConsulta { get; set; }        
    }
}