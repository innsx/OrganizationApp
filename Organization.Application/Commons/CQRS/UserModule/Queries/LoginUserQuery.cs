using ErrorOr;
using MediatR;

namespace Organization.Application.Commons.CQRS.UserModule.Queries
{
    public record LoginUserQuery(string Email, string Password) : IRequest<ErrorOr<string>>;
}
