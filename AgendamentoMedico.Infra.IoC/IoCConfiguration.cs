using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Application.Services;
using AgendamentoMedico.Infra.Data.Context;
using AgendamentoMedico.Infra.Data.Interfaces;
using AgendamentoMedico.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AgendamentoMedico.Infra.IoC
{
    public static class IoCConfiguration
    {
        public static void ConfigureRepository(IServiceCollection services)
        {
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IJwtToken, JwtToken>();
            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
        }

        public static void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IMedicoService, MedicoService>();
            services.AddScoped<IPacienteService, PacienteService>();            
        }

        public static void ConfigureAutoMapper(IServiceCollection services)
        {
            //services.AddAutoMapper(typeof(AutoMapperConfig));
        }
    }
}
