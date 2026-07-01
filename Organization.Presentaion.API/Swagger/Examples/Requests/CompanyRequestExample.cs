using Organization.Application.Commons.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Organization.Presentaion.API.Swagger.Examples.Requests
{
    public class CompanyRequestExample : IExamplesProvider<CompanyRequestDto>
    {
        public CompanyRequestDto GetExamples()
        {
            //Change a Class into a RECORD & returns it
            //return new CompanyRequestDto(
            //    Name = "name of the company",
            //    Address = "address of the company",
            //    Country = "country of the company",
            //;

            //returns companyRequestDto as a RECORD
            return new CompanyRequestDto(
                "name of the company", 
                "address of the company", 
                "country of the company");
        }

    }
}
