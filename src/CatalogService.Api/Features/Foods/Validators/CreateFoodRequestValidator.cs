using CatalogService.Api.Domain.Entities;
using CatalogService.Contracts.Food.Requests;
using FluentValidation;

namespace CatalogService.Api.Features.Foods.Validators;

public class CreateFoodRequestValidator : AbstractValidator<CreateFoodRequest>
{
    public CreateFoodRequestValidator()
    {
        RuleFor(f => f.Name).NotEmpty().MaximumLength(100);
        RuleFor(f => f.Price).GreaterThan(0);
        RuleFor(f=>f.FoodCategoryId).NotEmpty();
        RuleFor(f=>f.restaurantId).NotEmpty();
    }
}