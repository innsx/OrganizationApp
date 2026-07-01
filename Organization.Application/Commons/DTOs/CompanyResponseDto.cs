namespace Organization.Application.Commons.DTOs
{
    //public class CompanyResponseDto
    //{
    //    public string? Name { get; set; }
    //    public string? Address { get; set; }
    //    public string? Country { get; set; }
    //}

    //using a RECORD instead a CLASS
    public record CompanyResponseDto(string Name, string Address, string Country);
}
