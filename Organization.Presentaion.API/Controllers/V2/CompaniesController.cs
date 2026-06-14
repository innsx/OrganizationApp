using Microsoft.AspNetCore.Mvc;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Company;
using Organization.Domain.Company.Models;

namespace Organization.Presentaion.API.Controllers.V2
{
    //[Route("api/[controller]")]   //setup for Query String or HEADER API Versioning
    [Route("api/v{v:apiVersion}/[controller]")] //setup for URI API Versioning
    [ApiVersion("2.0")]  //specified version
    [ApiController]
    [Produces("application/json")]
    public class CompaniesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        //public IGenericRepository<Company> companyRepository;

        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            var companies = await _unitOfWork.Companies.GetCompaniesByQueryAsync(companyQueryParameters);
            //var companies = await companyRepository.GetAsync(companyQueryParameters);

            return Ok(companies);
        }

        /// <summary>
        /// This endpoint gets a particular company table based of this {id}.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <response code="200">Gets a comapny successfully.</response>
        /// <response code="404">Could not find the company.</response>
        /// <returns>Company</returns>
        [HttpGet("{id:length(22)}")]
        public async Task<ActionResult<Company>> GetCompanyById(string id)
        {
            //var company = await _unitOfWork.Companies.GetByIdAsync(id);
            //var company = await companyRepository.QueryOneToManyParentChildRelationshipAsync(id);

            var company = await _unitOfWork.Companies.QueryOneToManyParentChildRelationshipAsync(id);

            if (company == null)
            {
                return NotFound(company);
            }

            return Ok(company);
        }


        /// <summary>
        /// This endpoint adds a company in the system.
        /// </summary>
        /// <param name="companyRequestDto">**companyRequestDto**</param>
        /// <response code="201">Adds a company successfullly</response>
        [HttpPost]
        public async Task<IActionResult> AddCompany([FromBody] CompanyRequestDto companyRequestDto)
        {
            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            var newCompanyId = await _unitOfWork.Companies.AddAsnyc(new Company
            {
                Name = companyRequestDto.Name,
                Address = companyRequestDto.Address,
                Country = companyRequestDto.Country,
            });

            //var newCompanyId = await companyRepository.AddAsnyc(new Company
            //{
            //    Name = companyRequestDto.Name,
            //    Address = companyRequestDto.Address,
            //    Country = companyRequestDto.Country,
            //});

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            return CreatedAtAction(nameof(GetCompanyById), new { id = newCompanyId }, companyRequestDto);
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
            var companyToUpdate = await _unitOfWork.Companies.GetByIdAsync(id);
            //var companyToUpdate = await companyRepository.GetByIdAsync(id);

            if (companyToUpdate == null)
            {
                return NotFound($"Company with Id: {id} is not found.");
            }

            companyToUpdate.Name = companyRequestDto.Name;
            companyToUpdate.Address = companyRequestDto.Address;
            companyToUpdate.Country = companyRequestDto.Country;

            _unitOfWork.OpenConnectionAndBeginDbTransaction();
            await _unitOfWork.Companies.UpdateAsync(companyToUpdate);
            //await companyRepository.UpdateAsync(companyToUpdate);
            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            return CreatedAtAction("GetCompanyById", new { id }, companyToUpdate);
        }


        /// <summary>
        /// This endpoint SoftDeletes a company in the system.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <param name="isSoftDeleteRecordHasRelatedChildTableColumn">**CompanyRequest**</param>
        /// <response code="201">SoftDeletes a company successfullly</response>
        [HttpDelete("{id:length(22)}")]
        public async Task<IActionResult> DeleteCompany(string id, [FromBody] bool isSoftDeleteRecordHasRelatedChildTableColumn = false)
        {
            var companyToSoftDelete = await _unitOfWork.Companies.GetByIdAsync(id);
            //var companyToSoftDelete = await companyRepository.GetByIdAsync(id);

            if (companyToSoftDelete == null)
            {
                return NotFound($"Company with Id: {id} not found.");
            }

            _unitOfWork.OpenConnectionAndBeginDbTransaction();
            await _unitOfWork.Companies.SoftDeleteAsync(id, isSoftDeleteRecordHasRelatedChildTableColumn);
            //await companyRepository.SoftDeleteAsync(id, isSoftDeleteRecordHasRelatedChildTableColumn);
            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            if (isSoftDeleteRecordHasRelatedChildTableColumn == true)
            {
                return Ok($"Company with Id: {id} is successfully Soft-Deleted in Parent Table column and Child Table column");
            }
            else
            {
                return Ok($"Company with Id: {id} is successfully Soft-Deleted in Parent Table column");

            }
        }

    
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _unitOfWork.Companies.GetTotalCountAsync();

            return Ok(count);
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
