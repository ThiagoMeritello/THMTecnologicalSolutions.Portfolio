using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using THMTecnologicalSolutions.Portfolio.Application.Products.Common;
using THMTecnologicalSolutions.Portfolio.Application.Products.Create;
using THMTecnologicalSolutions.Portfolio.Application.Products.Update;

namespace THMTecnologicalSolutions.Portfolio.Api.IntegrationTests.Controllers;

public sealed class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Monitor Samsung G4",
            "Gaming monitor 240Hz",
            1799.90m,
            5);

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/v1/products",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        Assert.NotNull(response.Headers.Location);        

        var product = await response.Content.ReadFromJsonAsync<ProductResponse>();

        Assert.Contains($"/api/v1/products/{product?.Id}", response.Headers.Location.ToString());

        Assert.NotNull(product);
        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal(request.Name, product.Name);
        Assert.Equal(request.Price, product.Price);
        Assert.Equal(5, product.StockQuantity);
        Assert.True(product.IsActive);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new CreateProductRequest(
            "",
            null,
            -100m,
            -5);

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/v1/products",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task GetById_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Keyboard Logitech",
            "Mechanical keyboard",
            499.90m,
            10);

        var createResponse = await _client.PostAsJsonAsync(
            "/api/v1/products",
            request);

        var createdProduct =
            await createResponse.Content
                .ReadFromJsonAsync<ProductResponse>();

        Assert.NotNull(createdProduct);

        // Act
        var response = await _client.GetAsync(
            $"/api/v1/products/{createdProduct.Id}");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var product =
            await response.Content
                .ReadFromJsonAsync<ProductResponse>();

        Assert.NotNull(product);
        Assert.Equal(createdProduct.Id, product.Id);
        Assert.Equal("Keyboard Logitech", product.Name);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            $"/api/v1/products/{id}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldPersistChanges_WhenProductExists()
    {
        // Arrange
        var createRequest = new CreateProductRequest(
            "Notebook Dell",
            "Development notebook",
            4500m,
            10);

        var createResponse = await _client.PostAsJsonAsync(
            "/api/v1/products",
            createRequest);

        var createdProduct =
            await createResponse.Content
                .ReadFromJsonAsync<ProductResponse>();

        Assert.NotNull(createdProduct);

        var updateRequest = new UpdateProductRequest(
            "Notebook Dell G15",
            "Gaming and development notebook",
            5200m,
            true);

        // Act
        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/v1/products/{createdProduct.Id}",
            updateRequest);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            updateResponse.StatusCode);

        var getResponse = await _client.GetAsync(
            $"/api/v1/products/{createdProduct.Id}");

        var updatedProduct =
            await getResponse.Content
                .ReadFromJsonAsync<ProductResponse>();

        Assert.NotNull(updatedProduct);

        Assert.Equal(
            "Notebook Dell G15",
            updatedProduct.Name);

        Assert.Equal(
            5200m,
            updatedProduct.Price);

        // Estoque não deve ser alterado pelo PUT.
        Assert.Equal(
            10,
            updatedProduct.StockQuantity);

        Assert.NotNull(
            updatedProduct.UpdatedAt);
    }

    [Fact]
    public async Task Delete_ShouldDeactivateProduct_WhenProductExists()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Mouse Logitech",
            null,
            299.90m,
            5);

        var createResponse = await _client.PostAsJsonAsync(
            "/api/v1/products",
            request);

        var createdProduct =
            await createResponse.Content
                .ReadFromJsonAsync<ProductResponse>();

        Assert.NotNull(createdProduct);

        // Act
        var deleteResponse = await _client.DeleteAsync(
            $"/api/v1/products/{createdProduct.Id}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            deleteResponse.StatusCode);

        var product =
            await _client.GetFromJsonAsync<ProductResponse>(
                $"/api/v1/products/{createdProduct.Id}");

        Assert.NotNull(product);
        Assert.False(product.IsActive);
        Assert.NotNull(product.UpdatedAt);
    }
}
