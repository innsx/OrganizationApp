using ErrorOr;
using MapsterMapper;
using MediatR;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Authentications;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Commons.Errors;

namespace Organization.Application.Commons.CQRS.UserModule.Queries
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, ErrorOr<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<string>> Handle(LoginUserQuery loginUserRequest, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(loginUserRequest.Email);

            //Since response "user" properties matched ValidaUserResponse Properties, 
            //we DO NOT have to create a MAPPINGS
            //from SOURCE "user" to DESTINATION ValidUserResponse
            var mappedUser = _mapper.Map<ValidUserResponseDto>(user);

            bool isUserPasswordValid = BCrypt.Net.BCrypt.Verify(loginUserRequest.Password, user.PasswordHash);

            //we check if Login User's Password MATCHES
            //passwordHash stored in our OrganizationDb3 Database tblUserDetails table
            if (mappedUser is not null && isUserPasswordValid == true)
            {
                //if user is valid, we generate a JWT Token for the user & return it to the client
                var Token = _jwtTokenGenerator.GenerateToken(mappedUser);
                return Token;
            }
            else
            {
                //if user is invalid, we return an error message to the client
                return Errors.User.IncorrectEmailOrPassword("Email or Password is incorrected.");
            }

        }
    }
}
