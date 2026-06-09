using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Interfaces.Persistance.RepositoriesFactory;
using Organization.Infrastructure.Persistance;
using Organization.Infrastructure.Persistance.RepositoriesFactory;

namespace Organization.Presentaion.API.Configurations
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddScoped<IRepositoryFactory, RepositoryFactory>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
