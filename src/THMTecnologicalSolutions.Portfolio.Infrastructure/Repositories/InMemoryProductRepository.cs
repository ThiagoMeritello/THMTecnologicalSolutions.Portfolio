using System.Collections.Concurrent;
using THMTecnologicalSolutions.Portfolio.Application.Abstractions.Repositories;
using THMTecnologicalSolutions.Portfolio.Domain.Entities;

namespace THMTecnologicalSolutions.Portfolio.Infrastructure.Repositories;

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _products[product.Id] = product;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<Product> products = _products.Values.ToList();
        return Task.FromResult(products);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _products[product.Id] = product;
        return Task.CompletedTask;
    }
}
