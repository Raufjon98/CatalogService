using FluentValidation;

namespace CatalogService.Api.Features.Foods.Commands.DeleteFood;

public class DeleteFoodCommandValidator : AbstractValidator<DeleteFoodCommand>
{
    public DeleteFoodCommandValidator()
    {
        RuleFor(x => x.FoodId).NotEmpty();
    }    
}