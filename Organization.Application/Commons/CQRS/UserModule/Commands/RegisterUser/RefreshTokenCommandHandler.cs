using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Authentications;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Commons.Errors;

namespace Organization.Application.Commons.CQRS.UserModule.Commands.RegisterUser
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommandDto, ErrorOr<string>>
    {
        //Injecting the IUnitOfWork dependencies into the constructor so we can access Entity objects
        private readonly IUnitOfWork _unitOfWork;

        //Injecting the IHttpContextAccessor to access the HTTP context and retrieve the refresh token from the request cookies
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Injecting the IJwtTokenGenerator to generate new access tokens and refresh tokens for the user
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<string>> Handle(RefreshTokenCommandDto refreshTokenCommandDto, CancellationToken cancellationToken)
        {
            //RETRIEVE the refresh TOKEN from COOKIE using the const variable RefreshTokenCookieKey
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies[GlobalConstants.RefreshTokenCookieKey];

            //Get the User base of the RefreshTokenCommand object email property
            var user = await _unitOfWork.Users.GetUserByEmail(refreshTokenCommandDto.Email);

            //Check if the user is null or the refresh token is null
            //or does not match the user's refresh token
            if (user.RefreshToken is null || !user.RefreshToken.Equals(refreshToken))
            {
                //Return an error indicating that the refresh token is invalid
                return Errors.User.InvalidRefreshToken("Refresh TOKEN is Invalid.");
            }
            else if (user.RefreshTokenExpiryDate < DateTime.UtcNow)
            {
                //Return an error indicating that the refresh token has expired
                return Errors.User.RefreshTokenExpired("Refresh Token Expire Date has Expired/Invalid.");
            }

            //If the refresh token is valid and not expired, generate a new access token for the user
            var accessToken = await _jwtTokenGenerator.DoTokenCreationAsync(user);

            //Return the new access token to the client
            return accessToken;
        }
    }
}
