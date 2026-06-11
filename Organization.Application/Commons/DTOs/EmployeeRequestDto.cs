namespace Organization.Application.Commons.DTOs
{
    public class EmployeeRequestDto
    {
        public string? Name { get; set; }

        public int Age { get; set; }

        public string? Position { get; set; }

        public Decimal Salary { get; set; }

        public string? CompanyId {get; set;}
    }
}
