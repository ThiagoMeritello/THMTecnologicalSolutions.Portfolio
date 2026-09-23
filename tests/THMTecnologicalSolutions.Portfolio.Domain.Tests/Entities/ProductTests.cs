using THMTecnologicalSolutions.Portfolio.Domain.Entities;
using THMTecnologicalSolutions.Portfolio.Domain.Exceptions;

namespace THMTecnologicalSolutions.Portfolio.Domain.Tests.Entities;

public sealed class ProductTests
{
    //Arrange
    //↓
    //prepara os dados
    //Act
    //↓
    //executa aquilo que queremos testar
    //Assert
    // ↓
    //verifica o resultado

    [Fact]
    public void Constructor_ShouldCreateProduct_WhenDataIsValid()
    {
        // Arrange
        const string name = "Monitor Samsung G4";
        const string description = "Gaming monitor 240Hz";
        const decimal price = 1799.90m;
        const int stockQuantity = 5;

        // Act
        var product = new Product(
            name,
            description,
            price,
            stockQuantity);

        // Assert
        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal(name, product.Name);
        Assert.Equal(description, product.Description);
        Assert.Equal(price, product.Price);
        Assert.Equal(stockQuantity, product.StockQuantity);
        Assert.True(product.IsActive);
        Assert.NotEqual(default, product.CreatedAt);
        Assert.Null(product.UpdatedAt);
    }

    [Fact]
    public void Constructor_ShouldThrowDomainException_WhenPriceIsNegative()
    {
        // Arrange
        const decimal invalidPrice = -100m;

        // Act
        var exception = Assert.Throws<DomainException>(() =>
            new Product(
                "Monitor Samsung G4",
                "Gaming monitor",
                invalidPrice,
                5));

        // Assert
        Assert.Equal("Product price cannot be negative.", exception.Message);
    }

    [Fact]
    public void AddStock_ShouldIncreaseStockQuantity_WhenQuantityIsValid()
    {
        // Arrange
        var product = new Product(
            "Monitor Samsung G4",
            null,
            1799.90m,
            5);

        // Act
        product.AddStock(3);

        // Assert
        Assert.Equal(8, product.StockQuantity);
        Assert.NotNull(product.UpdatedAt);
    }

    [Fact]
    public void RemoveStock_ShouldDecreaseStockQuantity_WhenStockIsAvailable()
    {
        // Arrange
        var product = new Product(
            "Monitor Samsung G4",
            null,
            1799.90m,
            10);

        // Act
        product.RemoveStock(4);

        // Assert
        Assert.Equal(6, product.StockQuantity);
        Assert.NotNull(product.UpdatedAt);
    }

    [Fact]
    public void RemoveStock_ShouldThrowDomainException_WhenStockIsInsufficient()
    {
        // Arrange
        var product = new Product(
            "Monitor Samsung G4",
            null,
            1799.90m,
            5);

        // Act
        var exception = Assert.Throws<DomainException>(() =>
            product.RemoveStock(10));

        // Assert
        Assert.Equal(
            "There is not enough stock available.",
            exception.Message);

        Assert.Equal(5, product.StockQuantity);
    }

    [Fact]
    public void Deactivate_ShouldSetProductAsInactive()
    {
        // Arrange
        var product = new Product(
            "Monitor Samsung G4",
            null,
            1799.90m,
            5);

        // Act
        product.Deactivate();

        // Assert
        Assert.False(product.IsActive);
        Assert.NotNull(product.UpdatedAt);
    }

    [Fact]
    public void Activate_ShouldSetProductAsActive_WhenProductIsInactive()
    {
        // Arrange
        var product = new Product(
            "Monitor Samsung G4",
            null,
            1799.90m,
            5);

        product.Deactivate();

        // Act
        product.Activate();

        // Assert
        Assert.True(product.IsActive);
        Assert.NotNull(product.UpdatedAt);
    }
}
