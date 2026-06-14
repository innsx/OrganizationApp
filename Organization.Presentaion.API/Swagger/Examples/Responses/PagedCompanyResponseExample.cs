using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Utilities;
using Swashbuckle.AspNetCore.Filters;

namespace Organization.Presentaion.API.Swagger.Examples.Responses
{
    public sealed class PagedCompanyResponseExample : IExamplesProvider<PageList<CompanyResponseDto>>
    {
        public PageList<CompanyResponseDto> GetExamples()
        {
            var exampleCompanyResponse = new List<CompanyResponseDto>()
            {
                new CompanyResponseDto
                {
                  Name = "name1",
                  Address = "address1",
                  Country = "country1"
                },
              new CompanyResponseDto
              {
                  Name = "name2",
                  Address = "address2",
                  Country = "country2"
              }
            };

            var pageNumber = 1;
            var pageSize = 100;
            var totalCount = 1000;
            return PageList<CompanyResponseDto>.Create(exampleCompanyResponse, pageNumber, pageSize, totalCount);
        }
    }
    
}
