using AgendamentoMedico.Application.DTOs.Usuario.Request;
using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoMedico.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("autenticar")]
        public async Task<IActionResult> AutenticarUsuario([FromBody] AutenticarUsuarioRequest autenticarUsuario)
        {
            try
            {
                var token = await _usuarioService.AutenticarUsuario(autenticarUsuario.Email, autenticarUsuario.Senha);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("cadastrar-medico")]
        public async Task<IActionResult> CadastrarMedico([FromBody] UsuarioMedicoRequest usuario)
        {
            try
            {
                Usuario usuarioNew = new()
                {
                    Nome = usuario.Nome,
                    Cpf = usuario.Cpf,
                    Crm = usuario.Crm,
                    TempoDeConsulta = usuario.TempoDeConsulta,
                    Tipo = "M",
                    Email = usuario.Email,
                    Senha = usuario.Senha
                };

                await _usuarioService.CadastrarUsuario(usuarioNew);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("cadastrar-paciente")]
        public async Task<IActionResult> CadastrarPaciente([FromBody] UsuarioPacienteRequest usuario)
        {
            try
            {
                Usuario usuarioNew = new()
                {
                    Nome = usuario.Nome,
                    Cpf = usuario.Cpf,
                    Tipo = "P",
                    Email = usuario.Email,
                    Senha = usuario.Senha
                };

                await _usuarioService.CadastrarUsuario(usuarioNew);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("listar")]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.ListarUsuario();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
