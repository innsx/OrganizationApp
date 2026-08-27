namespace Organization.Application.Commons.DTOs
{
    public class RefreshTokenDto
    {
        public string TokenValue { get; set; } = string.Empty;
        public DateTime Expires { get; set; }

    }
}
