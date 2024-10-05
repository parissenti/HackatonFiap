using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Context;
using AgendamentoMedico.Infra.Data.Interfaces;
using MongoDB.Driver;

namespace AgendamentoMedico.Infra.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IMongoContext _context;

        public UsuarioRepository(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Usuario> AutenticarUsuario(UsuarioLogin usuarioLogin)
        {
            try
            {
                var usuario = await _context.Usuarios.Find(
                    u => u.Email == usuarioLogin.Email
                ).FirstOrDefaultAsync();

                if (usuario == null)
                {
                    throw new Exception("Usuário ou senha inválidos.");
                }

                bool senhaValida = BCrypt.Net.BCrypt.Verify(usuarioLogin.Senha, usuario.Senha);

                if (!senhaValida)
                {
                    throw new Exception("Usuário ou senha inválidos.");
                }

                usuario.Senha = null;
                return usuario;
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string email)
        {
            try
            {
                return await _context.Usuarios.Find(u => u.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }

        public async Task<Usuario> BuscarUsuarioPorId(Guid id)
        {
            try
            {
                return await _context.Usuarios.Find(u => u.Id == id).FirstOrDefaultAsync();
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }

        public async Task CadastrarUsuario(Usuario usuario)
        {
            try
            {
                await _context.Usuarios.InsertOneAsync(usuario);
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }

        public Task DesabilitarUsuario(int idUsuario)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Usuario>> ListarUsuarios()
        {
            try
            {
                return await _context.Usuarios.Find(e => true).ToListAsync();
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }

        public async Task<bool> DeletarUsuarioPorId(Guid id)
        {
            try
            {
                var resultado = await _context.Usuarios.DeleteOneAsync(u => u.Id == id);
                return resultado.DeletedCount > 0;
            }
            catch (MongoException ex)
            {
                throw new MongoException(ex.Message);
            }
        }
    }
}
