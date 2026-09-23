using FluentValidation;
using NSubstitute;
using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Application.Products.Stock;
using THMTecnologicalSolutions.Portfolio.Domain.Entities;

namespace THMTecnologicalSolutions.Portfolio.Application.Tests.Products.Stock;

public sealed class AddProductStockServiceTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldAddStockAndUpdateRepository_WhenRequestIsValid()
    {
        // Arrange
        var repository = Substitute.For<IProductRepository>();
        IValidator<ChangeStockRequest> validator = new ChangeStockValidator();
        var service = new AddProductStockService(repository,validator);

        var product = new Product(
            "Monitor Samsung G4",
            null,
            1799.90m,
            5);

        repository
            .GetByIdAsync(
                product.Id,
                Arg.Any<CancellationToken>())
            .Returns(product);

        var request = new ChangeStockRequest(3);

        // Act
        var response = await service.ExecuteAsync(
            product.Id,
            request);

        // Assert
        Assert.Equal(8, response.StockQuantity);

        await repository
            .Received(1)
            .UpdateAsync(
                product,
                Arg.Any<CancellationToken>());
    }
}
