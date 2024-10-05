using AgendamentoMedico.Application.DTOs.Usuario.Request;

namespace AgendamentoMedico.Application.Validator
{
    public class UsuarioValidator : BaseValidator
    {
        public async Task validaCamposDoUsuarioDoTipoMedico(UsuarioMedicoRequest requestDTO)
        {
            await Task.Run(() =>
            {
                if (requestDTO == null)
                    throw new ArgumentNullException("Dados para cadastro do Médico estão inválidos " + nameof(requestDTO));

                if (string.IsNullOrEmpty(requestDTO.Cpf))
                    throw new ArgumentException("O CPF do médico é obrigatório");

                if (string.IsNullOrEmpty(requestDTO.Nome))
                    throw new ArgumentException("O nome do médico é obrigatório");

                if (string.IsNullOrEmpty(requestDTO.Crm))
                    throw new ArgumentException("O CRM do médico é obrigatório");

                if (requestDTO.TempoDeConsulta == null || requestDTO.TempoDeConsulta == 0)
                    throw new ArgumentException("O tempo para a consulta do médico é obrigatório");

                if (string.IsNullOrEmpty(requestDTO.Email))
                    throw new ArgumentException("O E-mail é obrigatório");

                if (string.IsNullOrEmpty(requestDTO.Senha))
                    throw new ArgumentException("A senha é obrigatório");

            });
        }
    }
}
