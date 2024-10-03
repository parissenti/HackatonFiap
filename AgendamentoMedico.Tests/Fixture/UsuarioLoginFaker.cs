using AgendamentoMedico.Domain.Entitites;

namespace AgendamentoMedico.Tests.Fixture
{
    public class UsuarioLoginFaker
    {
        public static List<UsuarioLogin> UsuarioFake { get; } = new List<UsuarioLogin>()
        {
            new UsuarioLogin() { Email = "medico@email.com", Senha = "SENHA"}
        };
    }
}
