using Microsoft.Extensions.DependencyInjection;

namespace AgendamentoMedico.Infra.IoC
{
    public static class IoCConfiguration
    {
        public static void ConfigureRepository(IServiceCollection services)
        {
            //services.AddScoped<IMongoContext, MongoContext>();
            //services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            //services.AddScoped<IJwtToken, JwtToken>();
        }

        public static void ConfigureService(IServiceCollection services)
        {
            //services.AddScoped<IUsuarioService, UsuarioService>();
            //services.AddScoped<IAtivoService, AtivoService>();
            //services.AddScoped<IPortifolioService, PortifolioService>();
            //services.AddScoped<ITransacaoService, TransacaoService>();
        }

        public static void ConfigureAutoMapper(IServiceCollection services)
        {
            //services.AddAutoMapper(typeof(AutoMapperConfig));
        }
    }
}
