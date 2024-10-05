using AgendamentoMedico.Domain.Entitites;

namespace AgendamentoMedico.Tests.Fixture
{
    public  class UsuarioFaker
    {
        public static List<Usuario> UsuarioFake { get; } = new List<Usuario>()
        {
            new Usuario() {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                Nome = "João Silva",
                Cpf = "123.456.789-00",
                Crm = "C123456",
                TempoDeConsulta = 30,
                Tipo = "M",
                Email = "joao.silva@email.com",
                Senha = "SenhaForte123!"
            },
            new Usuario() {
                Id = Guid.Parse("b1b2c3d4-e5f6-7890-abcd-ef1234567891"),
                Nome = "Maria Souza",
                Cpf = "234.567.890-01",
                Crm = null,
                TempoDeConsulta = 0,
                Tipo = "P", 
                Email = "maria.souza@email.com",
                Senha = "SenhaForte456!"
            },
            new Usuario() {
                Id = Guid.Parse("c1b2c3d4-e5f6-7890-abcd-ef1234567892"),
                Nome = "Carlos Oliveira",
                Cpf = "345.678.901-02",
                Crm = "C987654",
                TempoDeConsulta = 20,
                Tipo = "M",
                Email = "carlos.oliveira@email.com",
                Senha = "SenhaForte789!"
            },
            new Usuario() {
                Id = Guid.Parse("b1b2c3f5-e5f6-7890-abcd-ef1234567891"),
                Nome = "Maria Luiza",
                Cpf = "234.567.890-01",
                Crm = null,
                TempoDeConsulta = 0,
                Tipo = "P",
                Email = "maria.luiza@email.com",
                Senha = "SenhaForte456!"
            },
        };
    }
}
