using FluentValidation;
using GoldenCrown.DTOs.Users;
using GoldenCrown.Features.User.UserLogin;
using GoldenCrown.Features.User.UserRegister;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoldenCrown.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, [FromServices] IValidator<RegisterRequest> validator)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var command = new UserRegisterCommand(request.Login, request.Name, request.Password);
            var result = await _mediator.Send(command); if (result)
            {
                return Ok();
                
            }
            return BadRequest(new { Message = "User registration failde" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequest request, [FromServices] IValidator<LoginRequest> validator)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var command = new UserLoginCommand(request.Login, request.Password);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(new { Token = result.Value });
            }
            return NotFound();
        } 
    }
}
