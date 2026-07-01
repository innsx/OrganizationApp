using Mapster;
using Organization.Application.Commons.CQRS.CompanyModule.Commands;
using Organization.Application.Commons.CQRS.EmployeeModule.Commands;
using Organization.Application.Commons.DTOs;

namespace Organization.Presentaion.API.Common.Mappings
{
    public class GlobalMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //Mappings: Map<Destination, Source>
            //since CompanyRequestDto DOES NOT have 'Id" property,
            //  and UpdateCompanyCommand has 'Id' property
            //we need to map the 'Id' property from the SOURCE(CompanyRequestDto)
            // to the  DESTINATION(UpdateCompanyCommand)
            config.NewConfig<(string Id, CompanyRequestDto companyRequestDto), UpdateCompanyCommand>()
                .Map(dest => dest.Id, src => src.Id)			//mapping dest.Id from src.Id
                .Map(dest => dest, src => src.companyRequestDto); //mapping the REST of dest Obj from src obj	

            config.NewConfig<(string Id, UpdateEmployeeRequestDto updateEmployeeRequestDto), UpdateEmployeeCommand>()
                .Map(dest => dest.Id, src => src.Id)			//mapping dest.Id from src.Id
                .Map(dest => dest, src => src.updateEmployeeRequestDto); //mapping the REST of dest Obj from src obj	
        }
    }
}
