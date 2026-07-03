namespace Organization.Application.Commons.DTOs
{
    public record UpdateEmployeeRequestDto(
        //string Id,
        string Name, 
        int Age, 
        string Position, 
        decimal Salary, 
        DateTime CreatedOn,
        DateTime ModifiedOn, 
        string CompanyId);
}
