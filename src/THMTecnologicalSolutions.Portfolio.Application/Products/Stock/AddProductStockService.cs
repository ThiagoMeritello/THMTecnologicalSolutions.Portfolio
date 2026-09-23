using FluentValidation;
using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Application.Exceptions;
using THMTecnologicalSolutions.Portfolio.Application.Products.Common;

namespace THMTecnologicalSolutions.Portfolio.Application.Products.Stock;

public sealed class AddProductStockService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<ChangeStockRequest> _validator;

    public AddProductStockService(
        IProductRepository productRepository,
        IValidator<ChangeStockRequest> validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<ProductResponse> ExecuteAsync(Guid id, ChangeStockRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            throw new NotFoundException($"Product '{id}' was not found.");

        product.AddStock(request.Quantity);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return ProductMapper.ToResponse(product);
    }
}