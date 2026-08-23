namespace Organization.Application.Commons.DTOs
{
    public record ValidUserResponseDto(string Id, string Email, string UserName, string PasswordHash);
}
