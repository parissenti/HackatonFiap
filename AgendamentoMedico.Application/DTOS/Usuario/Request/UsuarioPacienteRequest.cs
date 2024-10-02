namespace AgendamentoMedico.Application.DTOs.Usuario.Request
{
    public class UsuarioPacienteRequest
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }        
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
