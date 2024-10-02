using AgendamentoMedico.Application.DTOS.Medico.Request;
using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoMedico.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicoController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("cadastrar-periodo-atendimento")]
        public async Task<IActionResult> CadastrarHorariosDisponiveis([FromBody] PeriodoAtendimento periodoAtendimento)
        {
            try
            {
                await _medicoService.CadastrarHorariosDisponiveis(periodoAtendimento);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("listar-periodo-atendimento/{id}")]
        public async Task<IActionResult> ListarPeriodoAtendimentos(Guid id)
        {
            try
            {
                var usuarios = await _medicoService.ListarHorariosDisponiveis(id);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("liberar-agenda")]
        public async Task<IActionResult> LiberarAgenda([FromBody] LIberarAgenda liberarAgenda)
        {
            try
            {

                await _medicoService.LiberarAgenda(liberarAgenda.idMedico, liberarAgenda.dataLiberar);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize]
        [Route("listar-agenda/{id}")]
        public async Task<IActionResult> ListarAgenda(Guid id)
        {
            try
            {
                var usuarios = await _medicoService.ListarAgenda(id);
                return Ok(usuarios);
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
                var usuarios = await _medicoService.ListarMedicos();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
