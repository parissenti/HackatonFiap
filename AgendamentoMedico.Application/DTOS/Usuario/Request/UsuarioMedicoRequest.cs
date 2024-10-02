namespace AgendamentoMedico.Application.DTOs.Usuario.Request
{
    public class UsuarioMedicoRequest
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Crm { get; set; }
        public int TempoDeConsulta { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
