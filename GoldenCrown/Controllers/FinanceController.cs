using FluentValidation;
using GoldenCrown.Attributes;
using GoldenCrown.DTOs.Finance;
using GoldenCrown.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoldenCrown.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [MyAuthorize]
    public class FinanceController : Controller
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalanceAsync()
        {
            var userId = HttpContext.Items["UserId"] as int?;
            var balanceResult = await _financeService.GetBalanceAsync(userId!.Value);

            if (balanceResult.IsSuccess)
            {
                return Ok(new BalanceResponce
                {
                    Balance = balanceResult.Value
                });
            }
            
            return BadRequest(new { Message = balanceResult.ErrorMessage });
            
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> DepositAsync([FromBody] DepositRequest request, IValidator<DepositRequest> validator)
        {

            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            return Ok();            
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferAsync([FromBody] TransferRequest request, IValidator<TransferRequest> validator) 
        {

            var validatorResult = await validator.ValidateAsync(request);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.ToDictionary());
            }

            var userId = HttpContext.Items["UserId"] as int?;
            var transferResult = await _financeService.TransferAsync(userId!.Value, request.ReceiverLogin, request.Amount);
            if (transferResult.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(new {Message = transferResult.ErrorMessage});
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetTransactionHistoryAcync([FromQuery]TransactionHistoryRequest request, IValidator<TransactionHistoryRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var userId = HttpContext.Items["UserId"] as int?;
            var historyResult = await _financeService.GetTransactionHistoryAsync(userId!.Value, request.From, request.To, request.Offset, request.Limit);
            if (historyResult.IsSuccess)
            {
                return Ok(historyResult.Value);
            }
            return BadRequest(new {Message = historyResult.ErrorMessage});
        }
    }
}
