using THMTecnologicalSolutions.Portfolio.Domain.Exceptions;

namespace THMTecnologicalSolutions.Portfolio.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; }

    public string? Description { get; private set; }

    public decimal Price { get; private set; }

    public int StockQuantity { get; private set; }

    public Product(
        string name,
        string? description,
        decimal price,
        int stockQuantity)
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidatePrice(price);
        ValidateStockQuantity(stockQuantity);

        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
        StockQuantity = stockQuantity;
    }

    public void ChangeName(string name)
    {
        ValidateName(name);

        Name = name.Trim();
        SetUpdatedAt();
    }

    public void ChangeDescription(string? description)
    {
        ValidateDescription(description);

        Description = description?.Trim();
        SetUpdatedAt();
    }

    public void ChangePrice(decimal price)
    {
        ValidatePrice(price);

        Price = price;
        SetUpdatedAt();
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(
                "The quantity to add must be greater than zero.");

        StockQuantity += quantity;
        SetUpdatedAt();
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(
                "The quantity to remove must be greater than zero.");

        if (quantity > StockQuantity)
            throw new DomainException(
                "There is not enough stock available.");

        StockQuantity -= quantity;
        SetUpdatedAt();
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(
                "Product name is required.");

        if (name.Length > 150)
            throw new DomainException(
                "Product name cannot exceed 150 characters.");
    }

    private static void ValidateDescription(string? description)
    {
        if (description?.Length > 500)
            throw new DomainException(
                "Product description cannot exceed 500 characters.");
    }

    private static void ValidatePrice(decimal price)
    {
        if (price < 0)
            throw new DomainException(
                "Product price cannot be negative.");
    }

    private static void ValidateStockQuantity(int stockQuantity)
    {
        if (stockQuantity < 0)
            throw new DomainException(
                "Product stock quantity cannot be negative.");
    }
}
