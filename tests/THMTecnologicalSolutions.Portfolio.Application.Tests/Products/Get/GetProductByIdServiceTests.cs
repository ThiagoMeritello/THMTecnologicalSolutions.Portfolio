using NSubstitute;
using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Application.Exceptions;
using THMTecnologicalSolutions.Portfolio.Application.Products.Get;
using THMTecnologicalSolutions.Portfolio.Domain.Entities;

namespace THMTecnologicalSolutions.Portfolio.Application.Tests.Products.Get;

public sealed class GetProductByIdServiceTests
{
    private readonly IProductRepository _productRepository;
    private readonly GetProductByIdService _service;

    public GetProductByIdServiceTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _service = new GetProductByIdService(_productRepository);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product(
            "Monitor Samsung G4",
            "Gaming monitor",
            1799.90m,
            5);

        _productRepository
            .GetByIdAsync(
                product.Id,
                Arg.Any<CancellationToken>())
            .Returns(product);

        // Act
        var response = await _service.ExecuteAsync(product.Id);

        // Assert
        Assert.Equal(product.Id, response.Id);
        Assert.Equal(product.Name, response.Name);
        Assert.Equal(product.Price, response.Price);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _productRepository
            .GetByIdAsync(
                id,
                Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act
        var exception =
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.ExecuteAsync(id));

        // Assert
        Assert.Equal(
            $"Product '{id}' was not found.",
            exception.Message);
    }
}