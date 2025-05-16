using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WalletAPI.Application.DTOs.User.Request;
using WalletAPI.Application.DTOs.User.Response;
using WalletAPI.Application.Interfaces;

namespace WalletAPI.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Criação de usuário")]
        [SwaggerResponse(201, "Usuário criado com sucesso", typeof(UserResponseDto))]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(401, "Sem permissão para realizar essa ação")]
        [SwaggerResponse(404, "Não encontrado")]
        [SwaggerResponse(500, "Oops!!! Erro interno")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequestDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _userAppService.CreateUserAsync(dto);
            return StatusCode(201, result);
        }
    }
}
