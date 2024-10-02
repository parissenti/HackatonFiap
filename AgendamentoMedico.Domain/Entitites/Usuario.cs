
namespace AgendamentoMedico.Domain.Entitites
{
    public class Usuario : EntityBase
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Crm { get; set; }
        public int TempoDeConsulta { get; set; }
        public string Tipo { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public List<PeriodoAtendimento> PeriodoAtendimento { get; set; } = new List<PeriodoAtendimento>();
        public List<Agenda> AgendaDiaria { get; set; } = new List<Agenda>();
    }
}
