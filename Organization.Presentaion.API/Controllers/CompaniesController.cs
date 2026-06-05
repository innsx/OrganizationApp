using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organization.Domain.Models;

namespace Organization.Presentaion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {

        List<Company> _companies = new List<Company>()
        {
            new Company() { Id = 1, Name = "Company 1", Address = "Address 1" },
            new Company() { Id = 2, Name = "Company 2", Address = "Address 2" },
            new Company() { Id = 3, Name = "Company 3", Address = "Address 3" },
        };

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var companies = await Task.FromResult(_companies);
            await Task.CompletedTask;

            return Ok(_companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(int id)
        {
            var company = _companies.Find(x => x.Id == id);

            if (company == null)
            {
                return NotFound("Not Found.");
            }

            //await Task.FromResult(company);
            await Task.CompletedTask;

            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany([FromBody] Company company)
        {
            var addCompany = new Company
            {
                Name = company.Name,
                Address = company.Address
            };

            _companies.Add(addCompany);

            //await Task.FromResult(addCompany);
            await Task.CompletedTask;

            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id}, addCompany);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] Company company)
        {
            var companyToUpdate = _companies.Find(x => x.Id == id);

            if (companyToUpdate == null)
            {
                return NotFound();
            }

            companyToUpdate.Name = company.Name;
            companyToUpdate.Address = company.Address;

            //await Task.FromResult(companyToUpdate);
            await Task.CompletedTask;

            return CreatedAtRoute("GetCompanyById", new { id = id}, companyToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var companyToDelete = _companies.Find(x => x.Id == id);

            if (companyToDelete == null)
            {
                return NotFound("Company not found.");
            }

            _companies.Remove(companyToDelete);

            //await Task.FromResult(companyToDelete);
            await Task.CompletedTask;

            return Ok("Successfully Deleted.");
        }
    }
}
