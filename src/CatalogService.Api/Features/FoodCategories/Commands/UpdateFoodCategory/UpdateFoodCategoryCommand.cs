using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.FoodCategory.Events;
using CatalogService.Contracts.FoodCategory.Requests;
using CatalogService.Contracts.FoodCategory.Responses;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.FoodCategories.Commands;

public record UpdateFoodCategoryCommand(string Id, CreateFoodCategoryRequest FoodCategory) : IRequest<FoodCategoryResponse>;
public class UpdateFoodCategoryCommandHandler : IRequestHandler<UpdateFoodCategoryCommand, FoodCategoryResponse>
{
    private readonly IFoodCategoryRepository _foodCategoryRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateFoodCategoryCommandHandler(IFoodCategoryRepository foodCategoryRepository, IPublishEndpoint publishEndpoint)
    {
        _foodCategoryRepository = foodCategoryRepository;
        _publishEndpoint = publishEndpoint;
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

        await _publishEndpoint.Publish(
            new FoodCategoryUpdatedEvent
            {
                Id = foodCategory.Id,
                UpdatedOnUtc = DateTime.UtcNow
            },
            cancellationToken);
        
        FoodCategoryResponse foodCategoryDto = new FoodCategoryResponse()
        {
            Id = foodCategory.Id,
            Name = foodCategory.Name,
            Availability = foodCategory.Availability,
        };
        return foodCategoryDto;
    }
}