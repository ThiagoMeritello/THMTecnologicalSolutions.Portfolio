using System;
using System.Collections.Generic;
using System.Text;
using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Application.Products.Common;

namespace THMTecnologicalSolutions.Portfolio.Application.Products.Get;

public class GetProductsService
{
    private readonly IProductRepository _productRepository;

    public GetProductsService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyCollection<ProductResponse>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return products.Select(ProductMapper.ToResponse).ToList();
    }
}
