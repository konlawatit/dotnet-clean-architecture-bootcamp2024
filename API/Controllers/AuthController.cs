using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Queries.Login;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request) {
            var result = await mediator.Send(new RegisterCommand { Register = request });
            if (result is false) {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request) {
            var result = await mediator.Send(new LoginCommand { LoginRequest = request });
            if (request is null) {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
