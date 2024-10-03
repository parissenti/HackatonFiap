using AgendamentoMedico.Domain.Entitites;
using AgendamentoMedico.Infra.Data.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgendamentoMedico.Infra.Data.Repositories
{
    public class JwtToken : IJwtToken
    {
        private readonly AppSettings _settings;

        public JwtToken(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<string> GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            List<Claim> claims = new()
            {
                new Claim("Nome", usuario.Nome.ToString()),
                new Claim(ClaimTypes.Role, "ADMIN"),
                new Claim("Tipo", usuario.Tipo.ToString()),
                new Claim("Email", usuario.Email.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = "FiapTechChallenge",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString =  tokenHandler.WriteToken(token);

            return await Task.FromResult(tokenString);
        }
    }
}
