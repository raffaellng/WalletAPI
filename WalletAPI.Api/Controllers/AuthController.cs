using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WalletAPI.Application.DTOs.Auth;
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
        [SwaggerResponse(401, "Credenciais inválidas")]
        [SwaggerResponse(500, "Erro interno")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authAppService.LoginAsync(request);
            return Ok(result);
        }
    }
}
