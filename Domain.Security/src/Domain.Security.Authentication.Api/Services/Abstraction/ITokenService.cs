using Domain.Security.Authentication.Api.Entities;
using Microsoft.AspNetCore.Http;

namespace Domain.Security.Authentication.Api.Services.Abstraction
{
    public interface ITokenService
    {
        string GenerateToken(Resource resource);

        void ValidateToken(HttpContext context,string token);
    }
}
