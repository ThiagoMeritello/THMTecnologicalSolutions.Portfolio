using System;
using System.Collections.Generic;
using System.Text;

namespace THMTecnologicalSolutions.Portfolio.Application.Products.Create;

public sealed record CreateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    int StockQuantity);
