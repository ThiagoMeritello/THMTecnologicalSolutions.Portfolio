using Microsoft.Extensions.DependencyInjection;
using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Infrastructure.Repositories;

namespace THMTecnologicalSolutions.Portfolio.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();

        return services;
    }
}
