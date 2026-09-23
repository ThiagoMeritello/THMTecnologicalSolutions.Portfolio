namespace THMTecnologicalSolutions.Portfolio.Application.Products.Update;

public sealed record UpdateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    bool IsActive);
