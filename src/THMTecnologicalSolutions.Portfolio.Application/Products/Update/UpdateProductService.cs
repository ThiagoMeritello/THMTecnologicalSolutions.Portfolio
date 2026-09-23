using FluentValidation;
using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Application.Exceptions;
using THMTecnologicalSolutions.Portfolio.Application.Products.Common;

namespace THMTecnologicalSolutions.Portfolio.Application.Products.Update;

public sealed class UpdateProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<UpdateProductRequest> _validator;

    public UpdateProductService(
        IProductRepository productRepository,
        IValidator<UpdateProductRequest> validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<ProductResponse> ExecuteAsync(Guid id, UpdateProductRequest request, 
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            throw new NotFoundException($"Product '{id}' was not found.");

        product.ChangeName(request.Name);
        product.ChangeDescription(request.Description);
        product.ChangePrice(request.Price);

        if (request.IsActive)
            product.Activate();
        else
            product.Deactivate();

        await _productRepository.UpdateAsync(product, cancellationToken);

        return ProductMapper.ToResponse(product);
    }
}
