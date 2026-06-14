using Organization.Application.Commons.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Organization.Presentaion.API.Swagger.Examples.Requests
{
    public class CompanyRequestExample : IExamplesProvider<CompanyRequestDto>
    {
        public CompanyRequestDto GetExamples()
        {
            return new CompanyRequestDto
            {
                Name = "name of the company",
                Address = "address of the company",
                Country = "country of the company"
            };
        }

    }
}
