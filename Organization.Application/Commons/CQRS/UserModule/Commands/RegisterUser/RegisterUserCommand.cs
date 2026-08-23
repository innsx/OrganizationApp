using ErrorOr;
using MediatR;
using Organization.Application.Commons.DTOs;

namespace Organization.Application.Commons.CQRS.UserModule.Commands.RegisterUser
{
    public record RegisterUserCommand(RegisterUserRequestDto registerUserRequestDto) : IRequest<ErrorOr<Unit>>;
}
