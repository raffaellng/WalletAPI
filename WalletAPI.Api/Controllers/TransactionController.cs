using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WalletAPI.Application.DTOs.Transfer.Request;
using WalletAPI.Application.DTOs.Transfer.Response;
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

        [HttpPost("manual-transfer")]
        [SwaggerOperation(Summary = "Transferência administrativa", Description = "Permite transferir saldo de qualquer usuário para qualquer outro.")]
        [SwaggerResponse(204, "Transferência realizada com sucesso")]
        [SwaggerResponse(400, "Erro de validação")]
        [SwaggerResponse(401, "Não autorizado")]
        [SwaggerResponse(500, "Erro interno")]
        public async Task<IActionResult> AdminTransfer([FromBody] TransactionManualCreateRequestDto dto)
        {
            await _transactionAppService.CreateManualTransferAsync(dto);
            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar transferências realizadas", Description = "Retorna todas as transferências feitas pelo usuário autenticado, com filtros opcionais por data.")]
        [SwaggerResponse(200, "Lista retornada com sucesso", typeof(IEnumerable<TransactionResponseDto>))]
        [SwaggerResponse(401, "Não autorizado")]
        [SwaggerResponse(500, "Erro interno")]
        public async Task<IActionResult> List(
            [FromQuery] string? email,
            [FromQuery] DateTime? dataInicio,
            [FromQuery] DateTime? dataFim)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _transactionAppService.ListUserTransactionsAsync(userId, email, dataInicio, dataFim);
            return Ok(result);
        }
    }
}
