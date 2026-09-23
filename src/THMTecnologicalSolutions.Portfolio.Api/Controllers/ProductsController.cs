using Microsoft.AspNetCore.Mvc;
using THMTecnologicalSolutions.Portfolio.Application.Products.Common;
using THMTecnologicalSolutions.Portfolio.Application.Products.Create;
using THMTecnologicalSolutions.Portfolio.Application.Products.Delete;
using THMTecnologicalSolutions.Portfolio.Application.Products.Get;
using THMTecnologicalSolutions.Portfolio.Application.Products.Stock;
using THMTecnologicalSolutions.Portfolio.Application.Products.Update;

namespace THMTecnologicalSolutions.Portfolio.Api.Controllers;

[ApiController]
[Route("api/v1/products")]
[Produces("application/json")]
[Tags("Products")]
public sealed class ProductsController : ControllerBase
{
    private readonly CreateProductService _createProductService;
    private readonly GetProductsService _getProductsService;
    private readonly GetProductByIdService _getProductByIdService;
    private readonly UpdateProductService _updateProductService;
    private readonly DeleteProductService _deleteProductService;
    private readonly AddProductStockService _addProductStockService;
    private readonly RemoveProductStockService _removeProductStockService;

    public ProductsController(
        CreateProductService createProductService,
        GetProductsService getProductsService,
        GetProductByIdService getProductByIdService,
        UpdateProductService updateProductService,
        DeleteProductService deleteProductService,
        AddProductStockService addProductStockService,
        RemoveProductStockService removeProductStockService)
    {
        _createProductService = createProductService;
        _getProductsService = getProductsService;
        _getProductByIdService = getProductByIdService;
        _updateProductService = updateProductService;
        _deleteProductService = deleteProductService;
        _addProductStockService = addProductStockService;
        _removeProductStockService = removeProductStockService;
    }

    /// <summary>
    /// Returns all products.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<ProductResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ProductResponse>>> GetAllAsync(CancellationToken cancellationToken)
        => Ok(await _getProductsService.ExecuteAsync(cancellationToken));

    /// <summary>
    /// Returns a product by its identifier.
    /// </summary>
    [HttpGet("{id:guid}", Name = "GetProductById")]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => Ok(await _getProductByIdService.ExecuteAsync(id, cancellationToken));

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="request">Product information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created product.</returns>
    [HttpPost]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductResponse>> CreateAsync([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var response = await _createProductService.ExecuteAsync(request, cancellationToken);
        return CreatedAtRoute("GetProductById", new { id = response.Id }, response);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> UpdateAsync(Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
        => Ok(await _updateProductService.ExecuteAsync(id, request, cancellationToken));

    /// <summary>
    /// Deactivates a product using soft delete.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(
    StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _deleteProductService.ExecuteAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Adds quantity to the product stock.
    /// </summary>
    [HttpPost("{id:guid}/stock/add")]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> AddStockAsync(Guid id, [FromBody] ChangeStockRequest request, CancellationToken cancellationToken)
        => Ok(await _addProductStockService.ExecuteAsync(id, request, cancellationToken));

    /// <summary>
    /// Removes quantity from the product stock.
    /// </summary>
    [HttpPost("{id:guid}/stock/remove")]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> RemoveStockAsync(Guid id, [FromBody] ChangeStockRequest request, CancellationToken cancellationToken)
        =>Ok(await _removeProductStockService.ExecuteAsync(id, request, cancellationToken));    
}
