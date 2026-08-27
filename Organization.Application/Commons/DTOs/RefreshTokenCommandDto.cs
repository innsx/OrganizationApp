using ErrorOr;
using MediatR;

namespace Organization.Application.Commons.DTOs
{
    public record RefreshTokenCommandDto(string Email) : IRequest<ErrorOr<string>>;
}
