using AggregatorService.Api.Extensions;
using AggregatorService.Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AggregatorService.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateAndGetToken([FromBody] AuthenticateAndGetTokenRequest request)
        {
            var query = request.ToAuthenticateAndGetTokenQuery();

            var token = await _mediator.Send(query);
            
            if (token == null) return Unauthorized();

            return Ok(token);
        }
    }
}
