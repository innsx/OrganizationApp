using Organization.Application.Commons.DTOs;

namespace Organization.Application.Commons.Interfaces.Authentications
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ValidUserResponseDto validUserResponseDto);
    }
}
