using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WalletAPI.Application.DTOs.Auth.Request;
using WalletAPI.Application.DTOs.Auth.Response;
using WalletAPI.Application.Interfaces;

namespace WalletAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthAppService _authAppService;

        public AuthController(IAuthAppService authAppService)
        {
            _authAppService = authAppService;
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login do usuário", Description = "Autenticação via JWT")]
        [SwaggerResponse(200, "Login realizado com sucesso", typeof(LoginResponseDto))]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(401, "Usuário ou senha inválidos.")]
        [SwaggerResponse(404, "Não encontrado")]
        [SwaggerResponse(500, "Oops!!! Erro interno")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authAppService.LoginAsync(request);
            return Ok(result);
        }
    }
}
