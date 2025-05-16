using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WalletAPI.Application.DTOs.Transfer.Request;
using WalletAPI.Application.Interfaces;

namespace WalletAPI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionAppService _transactionAppService;

        public TransactionController(ITransactionAppService transactionAppService)
        {
            _transactionAppService = transactionAppService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Criar transferência", Description = "Transfere saldo da carteira do usuário autenticado para outro usuário.")]
        [SwaggerResponse(204, "Transferência realizada com sucesso")]
        [SwaggerResponse(400, "Erro de validação")]
        [SwaggerResponse(401, "Não autorizado")]
        [SwaggerResponse(500, "Erro interno")]
        public async Task<IActionResult> Transfer([FromBody] TransactionCreateRequestDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            await _transactionAppService.CreateTransferAsync(userId, dto);
            return NoContent();
        }
    }
}
