using CatalogService.Api.Features.Foods.Validators;
using FluentValidation;

namespace CatalogService.Api.Features.Foods.Commands.CreateFood;

public class CreateFoodCommandValidator : AbstractValidator<CreateFoodCommand>
{
    public CreateFoodCommandValidator()
    {
        RuleFor(x => x.CreateFoodDto).NotNull()
            .SetValidator(new CreateFoodRequestValidator());

    }
}