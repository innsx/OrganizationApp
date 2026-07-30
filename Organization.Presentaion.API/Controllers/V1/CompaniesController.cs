using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Organization.Application.Commons.ApplicationConfigOptions;
using Organization.Application.Commons.CQRS.CompanyModule.Commands;
using Organization.Application.Commons.CQRS.CompanyModule.Queries;
using Organization.Application.Commons.CQRS.CompanyModule.Queries.GetCompanyCount;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Company;
using System.Reflection;

namespace Organization.Presentaion.API.Controllers.V1
{
    //[Route("api/[controller]")]   //setup for Query String or HEADER API Versioning
    [Route("api/v{v:apiVersion}/[controller]")] //setup for URI API Versioning
    [ApiVersion("1.0")]  //specified version
    [ApiController]
    [Produces("application/json")]
    public sealed class CompaniesController : BaseAPIController
    {
        //initializing a repository of the type IGenericRepository<TEntity>
        //public IGenericRepository<Company> companyRepository;
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        private readonly OrganizationOption _organizationOption;

        public CompaniesController(ISender sender, IMapper mapper, IOptions<OrganizationOption> organizationOption)
        {           
            _sender = sender;
            _mapper = mapper;
            _organizationOption = organizationOption.Value;

            //using the FACTORY REPOSITORY pattern,
            //companyRepository = _unitOfWork.RepositoryFactory<Company>();
        }

        /// <summary>
        /// This endpoint gets all the companies in the system.
        /// </summary>
        /// <respone code="200">Returns paged list of all companies in the system</respone>
        [ProducesResponseType(typeof(PageList<CompanyResponseDto>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyQueryParameters companyQueryParameters)
        {
            //if we were using the FACTORY REPOSITORY pattern,
            //we would have called the GetAsync method of the companyRepository
            //to get the companies based on the query parameters.
            //var companies = await companyRepository.GetAsync(companyQueryParameters);

            var result = await _sender.Send(new GetCompaniesQuery(companyQueryParameters));
            return Ok(result);
        }

        /// <summary>
        /// This endpoint gets a particular company table based of this {id}.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <response code="200">Gets a comapny successfully.</response>
        /// <response code="404">Could not find the company.</response>
        /// <returns>Company</returns>
        [HttpGet("{id:length(22)}")]
        public async Task<IActionResult> GetCompanyById(string id, bool hasAssociatedObject = false)
        {
            //NOTE: we are now RETURNING  Task of type IActionResult instead of ActionResult<CompanyResponseDto>
            //because we are now returning ErrorOr<CompanyResponseDto> the GetCompanyByIdQueryHandler
            var result = await _sender.Send(new GetCompanyByIdQuery(id, hasAssociatedObject));

            return result.Match(
                r => Ok(r),
                errors => GetProblemFromErrorsCollection(errors)
            );
        }
         

        /// <summary>
        /// This endpoint adds a company in the system.
        /// </summary>
        /// <param name="companyRequestDto">**companyRequestDto**</param>
        /// <response code="201">Adds a company successfullly</response>
        [HttpPost]
        public async Task<IActionResult> AddCompany([FromBody] CompanyRequestDto companyRequestDto)
        {
            //note: this is manually mapping SOURCE CompanyRequestDto properties
            //  to DESTINATION AddCompanyCommand properties
            //var addCompanyCommand = new AddCompanyCommand(companyRequestDto.Name, 
            //                                            companyRequestDto.Address, 
            //                                            companyRequestDto.Country);

            //Mappings with Mapster: Map<TDestination>(TSource)
            var mappedCompany = _mapper.Map<AddCompanyCommand>(companyRequestDto);


            // we call the ISender Send( )
            //string companyId = await _sender.Send(addCompanyCommand);
            var result = await _sender.Send(mappedCompany);

            //return CreatedAtAction(nameof(GetCompanyById), new { id = companyId }, companyRequestDto);
            return result.Match(
                    result => Ok(result),
                    errors => GetProblemFromErrorsCollection(errors)
                    );

        }

        /// <summary>
        /// This endpoint updates a company in the system.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <param name="companyRequestDto">**companyRequestDto**</param>
        /// <response code="201">Updates a company successfullly</response>
        [HttpPut("{id:length(22)}")]
        public async Task<IActionResult> UpdateCompany(string id, [FromBody] CompanyRequestDto companyRequestDto)
        {
            //Mappings with Mapster: Map<TDestination>(TSource)
            var mappedCompany = _mapper.Map<UpdateCompanyCommand>((id, companyRequestDto));

            var result = await _sender.Send(mappedCompany);

            return result.Match(
                result => Ok(CreatedAtAction(nameof(GetCompanyById), new { id }, companyRequestDto)),
                errors => GetProblemFromErrorsCollection(errors)
            );            
        }


        /// <summary>
        /// This endpoint SoftDeletes a company in the system.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <param name="isDeleteHasAssociations">**CompanyRequest**</param>
        /// <response code="201">SoftDeletes a company successfullly</response>
        [HttpDelete("{id:length(22)}")]
        public async Task<IActionResult> DeleteCompany(string id, bool isDeleteHasAssociations = false)
        {
            var result = await _sender.Send(new DeleteCompanyCommand(id, isDeleteHasAssociations));

            return result.Match(
                result => Ok($"Successfully SoftDeleted Company with Id: {id}."),
                errors => GetProblemFromErrorsCollection(errors)
            );
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCompanyCount()
        {
            var organizationSection = _organizationOption;

            Console.WriteLine("\nConsole WriteLine: \n");
            Console.WriteLine(organizationSection.Name);
            Console.WriteLine(organizationSection.Address);
            Console.WriteLine(organizationSection.ConfidentialData);
            Console.WriteLine("\n\n");

            return Ok(await _sender.Send(new GetCompanyCountQuery()));
        }
    }
}







//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Organization.Domain.Company.Models;

//namespace Organization.Presentaion.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CompaniesController : ControllerBase
//    {

//        List<Company> _companies = new List<Company>()
//        {
//            new Company() { Id = "1", Name = "Company 1", Address = "Address 1" },
//            new Company() { Id = "2", Name = "Company 2", Address = "Address 2" },
//            new Company() { Id = "3", Name = "Company 3", Address = "Address 3" },
//        };

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            //var companies = await Task.FromResult(_companies);
//            await Task.CompletedTask;

//            return Ok(_companies);
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<Company>> GetCompanyById(string id)
//        {
//            var company = _companies.Find(x => x.Id == id);

//            if (company == null)
//            {
//                return NotFound("Not Found.");
//            }

//            //await Task.FromResult(company);
//            await Task.CompletedTask;

//            return Ok(company);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddCompany([FromBody] Company company)
//        {
//            var addCompany = new Company
//            {
//                Name = company.Name,
//                Address = company.Address
//            };

//            _companies.Add(addCompany);

//            //await Task.FromResult(addCompany);
//            await Task.CompletedTask;

//            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id}, addCompany);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateCompany(string id, [FromBody] Company company)
//        {
//            var companyToUpdate = _companies.Find(x => x.Id == id);

//            if (companyToUpdate == null)
//            {
//                return NotFound();
//            }

//            companyToUpdate.Name = company.Name;
//            companyToUpdate.Address = company.Address;

//            //await Task.FromResult(companyToUpdate);
//            await Task.CompletedTask;

//            return CreatedAtRoute("GetCompanyById", new { id = id}, companyToUpdate);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteCompany(string id)
//        {
//            var companyToDelete = _companies.Find(x => x.Id == id);

//            if (companyToDelete == null)
//            {
//                return NotFound("Company not found.");
//            }

//            _companies.Remove(companyToDelete);

//            //await Task.FromResult(companyToDelete);
//            await Task.CompletedTask;

//            return Ok("Successfully Deleted.");
//        }
//    }
//}
