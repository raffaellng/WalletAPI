using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using WalletAPI.Application.DTOs.Wallet.Request;
using WalletAPI.Application.Interfaces;

namespace WalletAPI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletAppService _walletAppService;

        public WalletController(IWalletAppService walletAppService)
        {
            _walletAppService = walletAppService;
        }

        [HttpGet("saldo")]
        [SwaggerOperation(Summary = "Consultar saldo", Description = "Retorna o saldo atual da carteira do usuário autenticado.")]
        [SwaggerResponse(200, "Saldo retornado com sucesso", typeof(object))] // ou um DTO se desejar
        [SwaggerResponse(401, "Token inválido ou ausente")]
        [SwaggerResponse(500, "Erro interno")]
        public async Task<IActionResult> GetBalance()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var saldo = await _walletAppService.GetBalanceAsync(userId);
            return Ok(new { saldo });
        }

        [HttpPost("depositar")]
        [SwaggerOperation(Summary = "Depositar saldo", Description = "Adiciona um valor à carteira do usuário autenticado.")]
        [SwaggerResponse(204, "Depósito realizado com sucesso")]
        [SwaggerResponse(400, "Valor inválido para depósito")]
        [SwaggerResponse(401, "Token inválido ou ausente")]
        [SwaggerResponse(500, "Erro interno")]
        public async Task<IActionResult> Deposit([FromBody] WalletDepositRequestDto dto)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            await _walletAppService.AddBalanceAsync(userId, dto.Amount);
            return NoContent();
        }
    }
}
