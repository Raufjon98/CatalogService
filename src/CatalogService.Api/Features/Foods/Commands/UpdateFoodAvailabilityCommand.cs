using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Responses;
using MediatR;

namespace CatalogService.Api.Features.Foods.Commands;

public record UpdateFoodAvailabilityCommand(string Id, bool IsAvailbale) :  IRequest<FoodResponse>;

public class UpdateFoodAvailabilityCommandHandler : IRequestHandler<UpdateFoodAvailabilityCommand, FoodResponse>
{
    private readonly IFoodRepository _foodRepository;

    public UpdateFoodAvailabilityCommandHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }
    public async Task<FoodResponse> Handle(UpdateFoodAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var result = await _foodRepository.UpdateAvailabilityAsync(request.Id, request.IsAvailbale, cancellationToken);
        if (result is null)
        {
            throw new Exception("Couldn't update food");
        }
        FoodResponse updatedFood = new FoodResponse()
        {
            Id = result.Id,
            Name = result.Name,
            Stock = result.Stock,
            FoodCategoryId = result.FoodCategoryId,
            RestaurantId = result.RestaurantId,
            Price = result.Price,
            IsAvailable = result.Availability,
        };
        return updatedFood;
    }
}