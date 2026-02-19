using CatalogService.Api.Features.Foods.Validators;
using FluentValidation;

namespace CatalogService.Api.Features.Foods.Commands.UpdateFood;

public class UpdateFoodCommandValidator : AbstractValidator<UpdateFoodCommand>
{
    public UpdateFoodCommandValidator()
    {
        RuleFor(x => x.FoodId).NotEmpty();
        RuleFor(x => x.CreateFoodDto).NotNull()
            .SetValidator(new CreateFoodRequestValidator());
    }   
}