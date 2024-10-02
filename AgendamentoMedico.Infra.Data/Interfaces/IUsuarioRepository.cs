using AgendamentoMedico.Domain.Entitites;

namespace AgendamentoMedico.Infra.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        Task CadastrarUsuario(Usuario usuario);
        Task<Usuario> AutenticarUsuario(string email, string senha);
        Task<Usuario> BuscarUsuarioPorEmail(string email);
        Task<Usuario> BuscarUsuarioPorId(Guid id);
        Task DesabilitarUsuario(int idUsuario);
        Task<IEnumerable<Usuario>> ListarUsuarios();
    }
}
