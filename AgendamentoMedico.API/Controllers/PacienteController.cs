using AgendamentoMedico.Application.DTOS.Medico.Request;
using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoMedico.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacienteController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("marcar-consulta")]
        public async Task<IActionResult> MarcarConsulta([FromBody] MarcarConsulta marcarConsulta)
        {
            try
            {
                await _pacienteService.MarcarConsulta(marcarConsulta.idAgendamentoMedico, marcarConsulta.idPaciente);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize]
        [Route("listar-consultas-agendadas/{id}")]
        public async Task<IActionResult> ListarPeriodoAtendimentos(Guid id)
        {
            try
            {
                var consultasAgendadas = await _pacienteService.ListarConsultas(id);
                return Ok(consultasAgendadas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
