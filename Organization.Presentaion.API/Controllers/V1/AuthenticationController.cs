using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Commons.CQRS.UserModule.Commands.RegisterUser;
using Organization.Application.Commons.CQRS.UserModule.Queries;
using Organization.Application.Commons.DTOs;

namespace Organization.Presentaion.API.Controllers.V1
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]  //specified version
    [ApiController]
    [AllowAnonymous]  //allow anonymous access to this controller
    public sealed class AuthenticationController : BaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly ISender _sender;

        public AuthenticationController(IMapper mapper, ISender sender)
        {
            _mapper = mapper;
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequestDto registerUserRequestDto)
        {
            var registerUserCommand = _mapper.Map<RegisterUserCommand>(registerUserRequestDto);

            var result = await _sender.Send(registerUserCommand);

            return result.Match(
                result => Ok(result),
                errors => GetProblemFromErrorsCollection(errors)
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequestDto loginUserRequestDto)
        {
            // Map the LoginUserRequestDto to a LoginUserQuery using Mapster
            var loginUserQuery = _mapper.Map<LoginUserQuery>(loginUserRequestDto);

            // Send the LoginUserQuery to the MediatR pipeline
            var result = await _sender.Send(loginUserQuery);

            // Use the Match method to handle the result and return the appropriate IActionResult
            return result.Match(
                r => Ok(r),
                errors => GetProblemFromErrorsCollection(errors) // Return a ProblemDetails response with the errors
            );
        }

    }
}
