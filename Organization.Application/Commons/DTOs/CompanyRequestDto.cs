namespace Organization.Application.Commons.DTOs
{
    //change CompanyRequestDto to a Record
    //public class CompanyRequestDto
    //{
    //    public string? Name { get; set; }
    //    public string? Address { get; set; }
    //    public string? Country { get; set; }
    //}

    public record CompanyRequestDto(string Name, string Address, string Country);
}
