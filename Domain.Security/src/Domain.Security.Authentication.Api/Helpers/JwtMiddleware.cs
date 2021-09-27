using Domain.Security.Authentication.Api.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Security.Authentication.Api.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;

        public JwtMiddleware(RequestDelegate next, ITokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                _tokenService.ValidateToken(context, token);

            await _next(context);
        }
    }
}
