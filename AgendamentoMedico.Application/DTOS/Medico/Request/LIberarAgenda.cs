namespace AgendamentoMedico.Application.DTOS.Medico.Request
{
    public class LIberarAgenda
    {
        public Guid idMedico { get; set; }
        public DateTime dataLiberar { get; set; }
    }
}
