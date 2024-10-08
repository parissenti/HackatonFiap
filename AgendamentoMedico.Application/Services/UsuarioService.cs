﻿using AgendamentoMedico.Application.Interfaces;
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

        public async Task<string> AutenticarUsuario(UsuarioLogin usuarioLogin)
        {
            if (usuarioLogin == null)
                throw new ArgumentNullException(nameof(usuarioLogin));

            if (string.IsNullOrWhiteSpace(usuarioLogin.Email))
                throw new ArgumentException("E-mail é obrigatório.", nameof(usuarioLogin.Email));

            if (string.IsNullOrWhiteSpace(usuarioLogin.Senha))
                throw new ArgumentException("Senha é obrigatório.", nameof(usuarioLogin.Senha));

            var login = new UsuarioLogin { Email = usuarioLogin.Email, Senha = usuarioLogin.Senha };

            var usuario = await _usuarioRepository.AutenticarUsuario(login);

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

        public async Task <bool>DeletarUsuarioPorId(Guid id)
        {
            return await _usuarioRepository.DeletarUsuarioPorId(id);
        }
    }
}
