using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    public record UpdateCompanyCommand(string Id, string Name, string Address, string Country) : IRequest;
   
}
