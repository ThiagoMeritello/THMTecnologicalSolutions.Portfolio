using FluentValidation;

namespace THMTecnologicalSolutions.Portfolio.Application.Products.Stock;

public sealed class ChangeStockValidator
    : AbstractValidator<ChangeStockRequest>
{
    public ChangeStockValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(
                "Stock quantity must be greater than zero.");
    }
}