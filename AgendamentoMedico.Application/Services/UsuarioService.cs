using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Interfaces;

namespace AgendamentoMedico.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtToken _jwtToken;

        public UsuarioService(IUsuarioRepository usuarioRepository, IJwtToken jwtToken)
        {
            _usuarioRepository = usuarioRepository;
            _jwtToken = jwtToken;
        }

        public async Task<string> AutenticarUsuario(string email, string senha)
        {
            var usuario = await _usuarioRepository.AutenticarUsuario(email, senha);

            if (usuario == null)
                throw new Exception("Falha ao autenticar usuário.");

            return await _jwtToken.GenerateToken(usuario);
        }

        public async Task<Usuario> BuscarUsuario(Guid id)
        {
            return await _usuarioRepository.BuscarUsuarioPorId(id);
        }

        public async Task CadastrarUsuario(Usuario usuario)
        {

            var usuarioExistente = await _usuarioRepository.BuscarUsuarioPorEmail(usuario.Email);

            if (usuarioExistente != null)
                throw new Exception("E-mail já está em uso.");

            string senhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            usuario.Senha = senhaHash;

            await _usuarioRepository.CadastrarUsuario(usuario);
        }

        public async Task<IEnumerable<Usuario>> ListarUsuario()
        {
            return await _usuarioRepository.ListarUsuarios();
        }
    }
}
