using Microsoft.IdentityModel.Tokens;
using NewEraCashCarry.Helpers;
using NewEraCashCarry.Interfaces;
using PDP.University.Examine.Project.Web.API.Models;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewEraCashCarry.Services
{
    public class TokenService : ITokenService
    {
        private readonly AuthSettings _authSettings;
        public TokenService(AuthSettings authSettings)
        {
            _authSettings = authSettings;
        }

        /// <summary>
        /// Generates a JWT token for the authenticated user.
        /// </summary>
        /// <param name="user">The user for whom the token will be generated.</param>
        /// <returns>The generated JWT token as a string.</returns>
        public string GenerateJwtToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_authSettings.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),

                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                Log.Information($"Token generated successfully.");

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to generate Token");
                throw;
            }
        }
    }
}
