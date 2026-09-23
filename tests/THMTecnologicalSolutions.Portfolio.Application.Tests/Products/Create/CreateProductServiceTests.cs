using FluentValidation;
using NSubstitute;
using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Application.Products.Create;
using THMTecnologicalSolutions.Portfolio.Domain.Entities;

namespace THMTecnologicalSolutions.Portfolio.Application.Tests.Products.Create;

public sealed class CreateProductServiceTests
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<CreateProductRequest> _validator;
    private readonly CreateProductService _service;

    public CreateProductServiceTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _validator = new CreateProductValidator();
        _service = new CreateProductService(_productRepository, _validator);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCreateProduct_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Monitor Samsung G4",
            "Gaming monitor 240Hz",
            1799.90m,
            5);

        // Act
        var response = await _service.ExecuteAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(request.Name, response.Name);
        Assert.Equal(request.Description, response.Description);
        Assert.Equal(request.Price, response.Price);
        Assert.Equal(request.StockQuantity, response.StockQuantity);
        Assert.True(response.IsActive);

        await _productRepository
            .Received(1)
            .AddAsync(
                Arg.Any<Product>(),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowValidationException_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new CreateProductRequest(
            "",
            null,
            -100m,
            -5);

        // Act
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.ExecuteAsync(request));

        // Assert
        await _productRepository
            .DidNotReceive()
            .AddAsync(
                Arg.Any<Product>(),
                Arg.Any<CancellationToken>());
    }
}
