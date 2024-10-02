using AgendamentoMedico.Domain.Entitites;

namespace AgendamentoMedico.Infra.Data.Interfaces
{
    public interface IJwtToken
    {
        public Task<string> GenerateToken(Usuario usuario);
    }
}
