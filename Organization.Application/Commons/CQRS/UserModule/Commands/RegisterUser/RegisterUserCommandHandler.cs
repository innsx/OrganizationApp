using ErrorOr;
using MediatR;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Users.Models;

namespace Organization.Application.Commons.CQRS.UserModule.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Unit>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.registerUserRequestDto.Password);

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            var userId = await _unitOfWork.Users.AddAsnyc(
                new User
                {
                    Email = request.registerUserRequestDto.Email,
                    UserName = request.registerUserRequestDto.UserName,
                    PasswordHash = passwordHash
                }
            );

            if (userId == null)
            {
                _unitOfWork.RollbackDbTransactionAndDispose();
                return Error.Failure("User registration failed.");
            }

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();
            return Unit.Value;
        }
    }

}
