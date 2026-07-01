namespace Organization.Application.Commons.DTOs
{
    public record UpdateEmployeeRequestDto(
        string Name, 
        int Age, 
        string Position, 
        decimal Salary, 
        DateTime CreatedOn,
        DateTime ModifiedOn, 
        string CompanyId);
}
