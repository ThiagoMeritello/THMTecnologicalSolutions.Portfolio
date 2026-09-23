using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Application.Exceptions;

namespace THMTecnologicalSolutions.Portfolio.Application.Products.Delete;

public sealed class DeleteProductService
{
    private readonly IProductRepository _productRepository;

    public DeleteProductService(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            throw new NotFoundException($"Product '{id}' was not found.");

        product.Deactivate();

        await _productRepository.UpdateAsync(product, cancellationToken);
    }
}
