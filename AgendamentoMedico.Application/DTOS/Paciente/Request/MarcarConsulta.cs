namespace AgendamentoMedico.Application.DTOS.Medico.Request
{
    public class MarcarConsulta
    {
        public Guid idAgendamentoMedico { get; set; }
        public Guid idPaciente { get; set; }
    }
}
