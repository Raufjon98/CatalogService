using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Requests;
using CatalogService.Contracts.Restaurant.Responses;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Commands;

public record CreateRestaurantCommand(CreateRestaurantRequest CreateRestaurantDto) : IRequest<RestaurantResponse>;

public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, RestaurantResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public CreateRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }
    public async Task<RestaurantResponse> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        Restaurant restaurant = new Restaurant()
        {
            Name = request.CreateRestaurantDto.Name,
            Description = request.CreateRestaurantDto.Description,
            CategoryId = request.CreateRestaurantDto.CategoryId,
            CuisineId = request.CreateRestaurantDto.CuisineId,
            AddressId = request.CreateRestaurantDto.AddressId,
            Phone = request.CreateRestaurantDto.Phone,
            Email = request.CreateRestaurantDto.Email,
        };
        var result = await _restaurantRepository.CreateAsync(restaurant, cancellationToken);
        if (result is null)
        {
            throw new Exception("Failed to create restaurant");
        }
        RestaurantResponse restaurantResponse = new RestaurantResponse()
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            CategoryId = result.CategoryId,
            CuisineId = result.CuisineId,
            AddressId = result.AddressId,
            Phone = result.Phone,
            Email = result.Email,
            IsAvailable = result.Availability,
        };
        return restaurantResponse;
    }
}
