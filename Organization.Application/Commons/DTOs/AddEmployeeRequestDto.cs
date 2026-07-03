namespace Organization.Application.Commons.DTOs
{
    //public class EmployeeRequestDto
    //{
    //    public string? Name { get; set; }

    //    public int Age { get; set; }

    //    public string? Position { get; set; }

    //    public Decimal Salary { get; set; }

    //    public string? CompanyId {get; set;}
    //    public DateTime CreatedOn { get; set; } = DateTime.Now;
    //    public DateTime ModifiedOn { get; set; } = DateTime.Now;

    //}

    public record AddEmployeeRequestDto(
        string Name, 
        int Age, 
        string Position, 
        decimal Salary,  
        DateTime CreatedOn,
        DateTime ModifiedOn, 
        string CompanyId);
}
