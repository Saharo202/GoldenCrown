using FluentValidation;
using GoldenCrown.Attributes;
using GoldenCrown.DTOs.Finance;
using GoldenCrown.Features.Finance.Deposit;
using GoldenCrown.Features.Finance.GetBalance;
using GoldenCrown.Features.Finance.GetTransactionHistory;
using GoldenCrown.Features.Finance.Transfer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using GoldenCrown.Features.Finance.OpenCurrencyAccount;
using GoldenCrown.Features.Finance.ConvertCurrency;

namespace GoldenCrown.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [MyAuthorize]
    public class FinanceController : Controller
    {
        private readonly IMediator _mediator;

        public FinanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalanceAsync()
        {
            var balanceResult = await _mediator.Send(new GetBalanceQuery(GetUserid()));

            if (balanceResult.IsSuccess)
            {
                return Ok(new BalanceResponce
                {
                    Accounts = balanceResult.Value
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
            var result = await _mediator.Send(new DepositCommand(
                GetUserid(),
                request.Currency,
                request.Amount));
            if (result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferAsync([FromBody] TransferRequest request, IValidator<TransferRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var transferResult = await _mediator.Send(new TransferCommand(
                GetUserid(),
                request.ReceiverLogin,
                request.Amount,
                request.Currency));
            if (transferResult.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(new { Message = transferResult.ErrorMessage });
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetTransactionHistoryAcync([FromQuery] TransactionHistoryRequest request, IValidator<TransactionHistoryRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var historyResult = await _mediator.Send(new GetTransactionHistoryQuery(GetUserid(), request.From, request.To, request.Offset, request.Limit));
            if (historyResult.IsSuccess)
            {
                return Ok(historyResult.Value);
            }
            return BadRequest(new { Message = historyResult.ErrorMessage });
        }

        [HttpPost("createdAccount")]
        public async Task<IActionResult> OpenCurrencyAccountAsync(
        [FromBody] OpenCurrencyAccountRequest request)
        {
            var result = await _mediator.Send(
                new OpenCurrencyAccountCommand(
                    GetUserid(),
                    request.Currency));

            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(new
            {
                Message = result.ErrorMessage
            });
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertAsync([FromBody] ConvertCurrencyRequest request)
        {
            var result = await _mediator.Send(new ConvertCurrencyCommand(
                        GetUserid(),
                        request.FromCurrency,
                        request.ToCurrency,
                        request.Amount));

            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(
                new
                {
                    Message = result.ErrorMessage
                });
        }

        internal int GetUserid()
        {
            var userId = HttpContext.Items[Constants.UserIdContextParameter] as int?;

            return userId!.Value;
        }
    }
}

