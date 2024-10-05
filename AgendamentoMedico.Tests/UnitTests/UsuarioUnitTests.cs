using AgendamentoMedico.Application.Services;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Interfaces;
using AgendamentoMedico.Tests.Fixture;
using AutoMapper;
using MongoDB.Driver;
using Moq;


namespace AgendamentoMedico.Tests.UnitTests
{
    public class UsuarioUnitTests
    {
        private readonly UsuarioService _usuarioService;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository = new Mock<IUsuarioRepository>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly Mock<IJwtToken> _tokenJwtMock;

        public UsuarioUnitTests()
        {
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _tokenJwtMock = new Mock<IJwtToken>();
            _usuarioService = new UsuarioService(
                _mockUsuarioRepository.Object,
                _tokenJwtMock.Object
            );
        }

        [Fact]
        public async Task Autenticar_DeveLancarExcecao_QuandoEmailNulo()
        {
            var dadosLogin = new UsuarioLogin { Senha = "SENHA" };

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await _usuarioService.AutenticarUsuario(dadosLogin)
            );

            _mockUsuarioRepository.Verify(repo => repo.AutenticarUsuario(
                It.IsAny<UsuarioLogin>()),
                Times.Never
            );

        }

        [Fact]
        public async Task Autenticar_DeveLancarExcecao_QuandoSenhaNulo()
        {
            var dadosLogin = new UsuarioLogin { Email = "usuario@email.com"};

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await _usuarioService.AutenticarUsuario(dadosLogin)
            );

            _mockUsuarioRepository.Verify(repo => repo.AutenticarUsuario(
                It.IsAny<UsuarioLogin>()),
                Times.Never
            );

        }

        [Fact]
        public async Task Autenticar_DeveLancarExcecao_QuandoCamposNulos()
        {
            var dadosLogin = new UsuarioLogin { };

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await _usuarioService.AutenticarUsuario(dadosLogin)
            );

            _mockUsuarioRepository.Verify(repo => repo.AutenticarUsuario(
                It.IsAny<UsuarioLogin>()),
                Times.Never
            );

        }

        [Fact]
        public async Task Autenticar_DeveLancarExcecao_QuandoUsuarioNaoExiste()
        {
            var dadosLogin = new UsuarioLogin {
                Email = "usuarioInesistente@email.com",
                Senha = "SENHA"
            };

            Func<Task> act = async () => await _usuarioService.AutenticarUsuario(dadosLogin);

            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task Autenticar_DeveRetornarToken_LoginFeitoComSucesso()
        {
            var dadosLogin = UsuarioLoginFaker.UsuarioFake[0];

            var usuario = new Usuario();

            _mockUsuarioRepository.Setup(repo => repo.AutenticarUsuario(It.IsAny<UsuarioLogin>()))
                .ReturnsAsync(usuario);

            _tokenJwtMock.Setup(token => token.GenerateToken(usuario))
                .ReturnsAsync("tokenGerado");

            var token = await _usuarioService.AutenticarUsuario(dadosLogin);

            Assert.NotNull(token);
            Assert.Equal("tokenGerado", token);
        }

        [Fact]
        public async Task Autenticar_DeveRetornarNull_QuandoIdDoUsuarioNaoExiste()
        {
            var id = Guid.NewGuid();

            _mockUsuarioRepository.Setup(repo => repo.BuscarUsuarioPorId(id))
                .ReturnsAsync((Usuario)null);

            var resultado = await _usuarioService.BuscarUsuario(id);

            Assert.Null(resultado);

            _mockUsuarioRepository.Verify(repo => repo.BuscarUsuarioPorId(id), Times.Once);
        }

        [Fact]
        public async Task Autenticar_DeveRetornarUsuario_QuandoIdDoUsuarioExiste()
        {
            var id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");

            var usuario = new Usuario();

            _mockUsuarioRepository.Setup(repo => repo.BuscarUsuarioPorId(It.IsAny<Guid>()))
                .ReturnsAsync(usuario);

            _tokenJwtMock.Setup(token => token.GenerateToken(usuario))
                .ReturnsAsync("tokenGerado");

            var token = await _usuarioService.BuscarUsuario(id);

            Assert.NotNull(token);
        }

        [Fact]
        public async Task CadastrarUsuario_DeveLancarExcecao_QuandoEmailJaEstaEmUso()
        {
            var usuario = UsuarioFaker.UsuarioFake[0];

            _mockUsuarioRepository.Setup(repo => repo.BuscarUsuarioPorEmail(usuario.Email))
                .ReturnsAsync(usuario);

            var exception = await Assert.ThrowsAsync<Exception>(() => _usuarioService.CadastrarUsuario(usuario));
            
            Assert.Equal("E-mail já está em uso.", exception.Message);
        }

        [Fact]
        public async Task CadastrarUsuario_DeveCadastrarUsuario_QuandoEmailNaoEstaEmUso()
        {
            var usuario = UsuarioFaker.UsuarioFake[0];

            _mockUsuarioRepository.Setup(repo => repo.BuscarUsuarioPorEmail(usuario.Email))
                .ReturnsAsync((Usuario)null);

            await _usuarioService.CadastrarUsuario(usuario);

            _mockUsuarioRepository.Verify(repo => repo.CadastrarUsuario(It.Is<Usuario>(u => u.Email == usuario.Email)), Times.Once);
            Assert.NotNull(usuario.Senha); 
            Assert.True(BCrypt.Net.BCrypt.Verify("SenhaForte123!", usuario.Senha));
        }

        [Fact]
        public async Task CadastrarUsuario_SenhaDeveSerHash()
        {
            var usuario = new Usuario
            {
                Email = "joao.silva@email.com",
                Senha = "SenhaForte123!"
            };

            _mockUsuarioRepository.Setup(repo => repo.BuscarUsuarioPorEmail(usuario.Email))
                .ReturnsAsync((Usuario)null);

            await _usuarioService.CadastrarUsuario(usuario);

            Assert.NotNull(usuario.Senha);
            Assert.True(BCrypt.Net.BCrypt.Verify("SenhaForte123!", usuario.Senha));
        }

        [Fact]
        public async Task ListarUsuario_DeveRetornarListaDeUsuarios()
        {
            var usuariosEsperados = UsuarioFaker.UsuarioFake;

            _mockUsuarioRepository.Setup(repo => repo.ListarUsuarios())
                .ReturnsAsync(usuariosEsperados);

            var resultado = await _usuarioService.ListarUsuario();

            Assert.NotNull(resultado);
            Assert.Equal(usuariosEsperados.Count, resultado.Count());
            Assert.Equal(usuariosEsperados, resultado); 
            _mockUsuarioRepository.Verify(repo => repo.ListarUsuarios(), Times.Once);
        }
    }
}
