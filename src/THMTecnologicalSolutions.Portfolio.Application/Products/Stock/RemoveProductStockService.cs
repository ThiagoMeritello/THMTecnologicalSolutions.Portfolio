using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Application.Exceptions;
using THMTecnologicalSolutions.Portfolio.Application.Products.Common;

namespace THMTecnologicalSolutions.Portfolio.Application.Products.Stock;

public sealed class RemoveProductStockService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<ChangeStockRequest> _validator;

    public RemoveProductStockService(
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

        product.RemoveStock(request.Quantity);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return ProductMapper.ToResponse(product);
    }
}