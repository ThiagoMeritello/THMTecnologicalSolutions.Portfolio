using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Application.Exceptions;
using THMTecnologicalSolutions.Portfolio.Application.Products.Common;

namespace THMTecnologicalSolutions.Portfolio.Application.Products.Get;

public sealed class GetProductByIdService
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdService(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            throw new NotFoundException($"Product '{id}' was not found.");

        return ProductMapper.ToResponse(product);
    }
}
