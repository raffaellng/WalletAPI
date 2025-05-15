using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WalletAPI.Application.DTOs.User.Request;
using WalletAPI.Application.DTOs.User.Response;
using WalletAPI.Application.Interfaces;

namespace WalletAPI.Api.Controllers
{
    [ApiController]
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
        [SwaggerResponse(400, "Erro de validação")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequestDto dto)
        {
            var result = await _userAppService.CreateUserAsync(dto);
            return StatusCode(201, result);
        }
    }
}
