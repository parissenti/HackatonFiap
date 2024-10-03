using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace AgendamentoMedico.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMedicoRepository _medicoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly EnvioEmail _envioEmail;
        public PacienteService(IPacienteRepository pacienteRepository, IUsuarioRepository usuarioRepository, IMedicoRepository medicoRepository, IOptions<EnvioEmail> envioEmail)
        {
            _pacienteRepository = pacienteRepository;
            _usuarioRepository = usuarioRepository;
            _medicoRepository = medicoRepository;
            _envioEmail = envioEmail.Value;
        }

        public async Task<IEnumerable<ConsultaAgendamentoResponse>> ListarConsultas(Guid idPaciente)
        {
            List<ConsultaAgendamentoResponse> consultas = new List<ConsultaAgendamentoResponse>();

            foreach (var consulta in await _pacienteRepository.ListarConsultas(idPaciente))
            {
                consultas.Add(new ConsultaAgendamentoResponse
                {
                    Id = consulta.Id,
                    DataConsulta = consulta.DataConsulta,
                    idMedico = consulta.idMedico,
                    idPaciente = consulta.idPaciente,
                    Medico = await _usuarioRepository.BuscarUsuarioPorId(consulta.idMedico),
                    Paciente = await _usuarioRepository.BuscarUsuarioPorId(consulta.idPaciente)
                });
            }

            return consultas;
        }

        public async Task MarcarConsulta(DateTime dataConsulta, Guid idMedico, Guid idPaciente)
        {
            var consulta = await _medicoRepository.BuscarConsultaPorDataeHorarioConsulta(dataConsulta, idMedico);

            if (consulta == null || !consulta.Any())
                throw new Exception("Data/Horário não disponível para agendamento");

            var medico = await _usuarioRepository.BuscarUsuarioPorId(idMedico);

            if (medico == null)
                throw new Exception("Não foi possível localizar o médico informado !");

            var paciente = await _usuarioRepository.BuscarUsuarioPorId(idPaciente);

            if (paciente == null)
                throw new Exception("Não foi possível localizar o médico informado !");

            await _pacienteRepository.MarcarConsulta(consulta.First().Id, idPaciente);

            await EnviarEmail(medico, paciente, consulta.First());

        }

        private async Task EnviarEmail(Usuario medico, Usuario paciente, ConsultaAgendamento consultaAgendamento)
        {

            var smtpUserNameLocal = _envioEmail.SmtpUserName;
            var GoogleAppPassword = _envioEmail.GoogleAppPassword;

            var fromAddress = smtpUserNameLocal;
            var toAddress = medico.Email;
            var subject = $"Health&Med - Nova consulta agendada";
            var body = new StringBuilder();


            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromAddress, GoogleAppPassword),
                EnableSsl = true,
            };


            body.AppendLine($"<div>ˮOlá, Dr. {medico.Nome}! </div>" +
                            $"<br><div>Você tem uma nova consulta marcada! Paciente: {paciente.Nome}:</div><br>" +
                            $"<br><div>Data e horário: {consultaAgendamento.DataConsulta}:</div><br>");

            var mailMessage = new MailMessage(fromAddress, toAddress, subject, body.ToString());
            mailMessage.IsBodyHtml = true;


            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao enviar e-mail:", ex);
            }
        }
    }
}
