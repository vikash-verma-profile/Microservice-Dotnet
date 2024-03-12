using Order.Interface;
using Order.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Common.Models.Models;


namespace Order.Repositories
{
    public class JwtManagerRepository:IJWTManagerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly EshoppingContext _context;
        public JwtManagerRepository(IConfiguration configuration, EshoppingContext context) {
            _configuration = configuration;
            _context = context;
        }

        public string Authenticate(LoginViewModel login)
        {
            if (!(_context.TblLogins.Any(x=>x.Username==login.UserName && x.Password==login.Password)))
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
