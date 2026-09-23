using FluentValidation;
using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Application.Products.Common;
using THMTecnologicalSolutions.Portfolio.Domain.Entities;

namespace THMTecnologicalSolutions.Portfolio.Application.Products.Create;

public sealed class CreateProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<CreateProductRequest> _validator;

    public CreateProductService(
        IProductRepository productRepository,
        IValidator<CreateProductRequest> validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<ProductResponse> ExecuteAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var product = new Product(request.Name, request.Description, request.Price, request.StockQuantity);

        await _productRepository.AddAsync(product, cancellationToken);

        return ProductMapper.ToResponse(product);
    }
}