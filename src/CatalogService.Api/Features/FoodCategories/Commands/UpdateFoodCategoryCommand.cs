using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.FoodCategory.Requests;
using CatalogService.Contracts.FoodCategory.Responses;
using MediatR;

namespace CatalogService.Api.Features.FoodCategories.Commands;

public record UpdateFoodCategoryCommand(string Id, CreateFoodCategoryRequest FoodCategory) : IRequest<FoodCategoryResponse>;
public class UpdateFoodCategoryCommandHandler : IRequestHandler<UpdateFoodCategoryCommand, FoodCategoryResponse>
{
    private readonly IFoodCategoryRepository _foodCategoryRepository;

    public UpdateFoodCategoryCommandHandler(IFoodCategoryRepository foodCategoryRepository)
    {
        _foodCategoryRepository = foodCategoryRepository;
    }
    public async Task<FoodCategoryResponse> Handle(UpdateFoodCategoryCommand request, CancellationToken cancellationToken)
    {
        FoodCategory foodCategory = new FoodCategory()
        {
            Id = request.Id,
            Name = request.FoodCategory.Name,
            Availability = request.FoodCategory.IsAvailable,
        };
        var result = await _foodCategoryRepository.UpdateAsync(foodCategory, cancellationToken);

        if (result is null)
        {
            throw new Exception("Couldn't update food category");
        }
        
        FoodCategoryResponse foodCategoryDto = new FoodCategoryResponse()
        {
            Id = foodCategory.Id,
            Name = foodCategory.Name,
            Availability = foodCategory.Availability,
        };
        return foodCategoryDto;
    }
}