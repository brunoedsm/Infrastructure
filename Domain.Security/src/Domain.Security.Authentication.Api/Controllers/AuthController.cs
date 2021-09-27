using Domain.Security.Authentication.Api.Dtos;
using Domain.Security.Authentication.Api.Helpers;
using Domain.Security.Authentication.Api.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Domain.Security.Authentication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IResourceService _resourceService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IResourceService resourceService, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _resourceService = resourceService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Authenticate(ResourceInputDto dto)
        {
            var resource = _resourceService.GetByCodeAndPassword(dto.Code, dto.Password);

            if (resource == null)
                return NotFound(new { Message = "Resource Not Found" });

            var token = _tokenService.GenerateToken(resource);


            _logger.LogInformation($"Authentication token generated to {resource.Name} at {DateTime.Now:dd/MM/yyyy hh:mm:ss}.");


            return Ok(new ResourceOutputDto()
            {
                Id = resource.Id,
                Name = resource.Name,
                Token = token
            });
        }

        [AuthorizeFilter]
        [HttpGet]
        public IActionResult GetAll()
        {
            var response = (from r in _resourceService.GetAll()
                            select new ResourceOutputDto()
                            {
                                Id = r.Id,
                                Name = r.Name
                            }
                            ).ToList();

            return Ok(response);
        }
    }
}
