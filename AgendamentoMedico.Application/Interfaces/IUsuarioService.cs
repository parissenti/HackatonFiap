using AgendamentoMedico.Application.DTOs.Usuario.Request;
using AgendamentoMedico.Domain.Entitites;

namespace AgendamentoMedico.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<string> AutenticarUsuario(UsuarioLogin usuarioLogin);
        Task CadastrarUsuario(Usuario usuario);
        Task<Usuario> BuscarUsuario(Guid id);
        Task<IEnumerable<Usuario>> ListarUsuario();
    } 
}
