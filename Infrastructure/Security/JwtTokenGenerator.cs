using Application.Interfaces.Security;
using Application.UseCases.LoginUser.Records;
using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly JwtSettings _settings;

        public JwtTokenGenerator(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }
        public LoginUserResult GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_settings.SecretKey));
            var expirationTime = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expirationTime,
                signingCredentials: credentials
            );
            LoginUserResult tokenResponse = new(new JwtSecurityTokenHandler().WriteToken(token), expirationTime, user.Id.ToString());

            return tokenResponse;
        }
    }
}
