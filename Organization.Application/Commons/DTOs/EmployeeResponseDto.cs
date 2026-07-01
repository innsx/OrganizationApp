namespace Organization.Application.Commons.DTOs
{
    //public class EmployeeResponseDto
    //{
    //    public string? Name { get; set; }
    //    public int Age { get; set; }
    //    public string? CompanyName { get; set; }
    //    public string? Position { get; set; }
    //    public Decimal Salary { get; set; }
    //    public DateTime CreatedOn { get; set; }
    //    public DateTime ModifiedOn { get; set; }
    //}

    public record EmployeeResponseDto(
        string Name,
        int Age, 
        string Position, 
        decimal Salary, 
        DateTime CreatedOn, 
        DateTime ModifiedOn, 
        string CompanyId); 
}
