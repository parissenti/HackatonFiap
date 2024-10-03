namespace AgendamentoMedico.Application.Validator
{
    public class BaseValidator
    {
        public async Task ValidaId(int id)
        {

            await Task.Run(() =>
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Id deve ser um número válido", nameof(id));
                }
            });

        }
    }
}
