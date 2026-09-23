using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using THMTecnologicalSolutions.Portfolio.Application.Products.Create;
using THMTecnologicalSolutions.Portfolio.Application.Products.Delete;
using THMTecnologicalSolutions.Portfolio.Application.Products.Get;
using THMTecnologicalSolutions.Portfolio.Application.Products.Stock;
using THMTecnologicalSolutions.Portfolio.Application.Products.Update;

namespace THMTecnologicalSolutions.Portfolio.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<CreateProductService>();
        services.AddScoped<GetProductsService>();
        services.AddScoped<GetProductByIdService>();
        services.AddScoped<UpdateProductService>();
        services.AddScoped<DeleteProductService>();
        services.AddScoped<AddProductStockService>();
        services.AddScoped<RemoveProductStockService>();

        return services;
    }
}