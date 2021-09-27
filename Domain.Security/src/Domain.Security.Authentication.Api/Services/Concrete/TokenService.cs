using Domain.Security.Authentication.Api.Entities;
using Domain.Security.Authentication.Api.Helpers;
using Domain.Security.Authentication.Api.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Domain.Security.Authentication.Api.Services.Concrete
{
    public class TokenService : ITokenService
    {
        private readonly string _secret;
        private readonly IResourceService _resourceService;

        public TokenService(IOptions<AppSettings> appSettings, IResourceService resourceService)
        {
            _secret = appSettings.Value.Secret;
            _resourceService = resourceService;
        }

        public string GenerateToken(Resource resource)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", resource.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);


        }

        public void ValidateToken(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach user to context on successful jwt validation
                context.Items["Resource"] = _resourceService.GetById(userId);
            }
            catch
            {
                throw new InvalidOperationException("Failed when trying to validate current token. See logs for detail.");
            }
        }
    }
}
