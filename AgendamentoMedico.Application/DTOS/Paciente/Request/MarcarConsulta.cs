namespace AgendamentoMedico.Application.DTOS.Medico.Request
{
    public class MarcarConsulta
    {
        public DateTime DataConsulta { get; set; }
        public Guid idMedico { get; set; }
        public Guid idPaciente { get; set; }
    }
}
