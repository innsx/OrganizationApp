using Organization.Application.Commons.DTOs;
using Organization.Domain.Users.Models;

namespace Organization.Application.Commons.Interfaces.Authentications
{
    public interface IJwtTokenGenerator
    {
        Task<string> DoTokenCreationAsync(User user);
    }
}
