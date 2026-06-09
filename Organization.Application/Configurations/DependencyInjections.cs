using Microsoft.Extensions.DependencyInjection;

namespace Organization.Application.Configurations
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
