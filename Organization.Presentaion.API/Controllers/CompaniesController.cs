using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Company.Models;

namespace Organization.Presentaion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public IGenericRepository<Company> companyRepository;
        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            companyRepository = _unitOfWork.RepositoryFactory<Company>();
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            //var companies = await _unitOfWork.Companies.GetAsync();

            //var companyRepository = _unitOfWork.RepositoryFactory<Company>();
            var companies = await companyRepository.GetAsync();

            return Ok(companies);
        }

        [HttpGet("{id:length(22)}")]
        public async Task<ActionResult<Company>> GetCompanyById(string id)
        {
            //var company = await _unitOfWork.Companies.GetByIdAsync(id);
            //var companyRepository = _unitOfWork.RepositoryFactory<Company>();
            var company = await companyRepository.GetAsync(id);

            if (company == null)
            {
                return NotFound(company);
            }

            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany([FromBody] CompanyRequestDto companyRequestDto)
        {
            _unitOfWork.OpenConnectionAndBeginTransaction();

            //var newCompanyId = await _unitOfWork.Companies.AddAsnyc(new Company
            //{
            //    Name = companyRequestDto.Name,
            //    Address = companyRequestDto.Address,
            //    Country = companyRequestDto.Country,
            //});

            //var companyRepository = _unitOfWork.RepositoryFactory<Company>();

            var newCompanyId = await companyRepository.AddAsnyc(new Company
            {
                Name = companyRequestDto.Name,
                Address = companyRequestDto.Address,
                Country = companyRequestDto.Country,
            });

            _unitOfWork.CommitTransactionDisposeAndCloseConnectionDispose();

            return CreatedAtAction(nameof(GetCompanyById), new { id = newCompanyId }, companyRequestDto);
        }

        [HttpPut("{id:length(22)}")]
        public async Task<IActionResult> UpdateCompany(string id, [FromBody] CompanyRequestDto companyRequestDto)
        {
            //var companyToUpdate = await _unitOfWork.Companies.GetByIdAsync(id);
            //var companyRepository = _unitOfWork.RepositoryFactory<Company>();
            var companyToUpdate = await companyRepository.GetByIdAsync(id);

            if (companyToUpdate == null)
            {
                return NotFound($"Company with Id: {id} is not found.");
            }

            companyToUpdate.Name = companyRequestDto.Name;
            companyToUpdate.Address = companyRequestDto.Address;
            companyToUpdate.Country = companyRequestDto.Country;

            _unitOfWork.OpenConnectionAndBeginTransaction();
            //await _unitOfWork.Companies.UpdateAsync(companyToUpdate);
            await companyRepository.UpdateAsync(companyToUpdate);
            _unitOfWork.CommitTransactionDisposeAndCloseConnectionDispose();

            return CreatedAtAction("GetCompanyById", new { id = id }, companyToUpdate);
        }

        [HttpDelete("{id:length(22)}")]
        public async Task<IActionResult> DeleteCompany(string id, [FromBody] bool isSoftDeleteColumnHasAssociatedChildTableColumn)
        {
            //var companyToSoftDelete = await _unitOfWork.Companies.GetByIdAsync(id);
            //var companyRepository = _unitOfWork.RepositoryFactory<Company>();
            var companyToSoftDelete = await companyRepository.GetByIdAsync(id);

            if (companyToSoftDelete == null)
            {
                return NotFound($"Company with Id: {id} not found.");
            }

            _unitOfWork.OpenConnectionAndBeginTransaction();
            //await _unitOfWork.Companies.SoftDeleteAsync(id, isSoftDeleteColumnHasAssociatedChildTableColumn);
            await companyRepository.SoftDeleteAsync(id, isSoftDeleteColumnHasAssociatedChildTableColumn);
            _unitOfWork.CommitTransactionDisposeAndCloseConnectionDispose();

            if (isSoftDeleteColumnHasAssociatedChildTableColumn == true)
            {
                return Ok($"Company with Id: {id} is successfully Soft-Deleted in Parent Table column and Child Table column");
            }
            else
            {
                return Ok($"Company with Id: {id} is successfully Soft-Deleted in Parent Table column");

            }
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
