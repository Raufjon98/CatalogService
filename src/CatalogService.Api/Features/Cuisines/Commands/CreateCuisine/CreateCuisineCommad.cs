using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Cuisine.Events;
using CatalogService.Contracts.Cuisine.Requests;
using CatalogService.Contracts.Cuisine.Responses;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Cuisines.Commands;

public record CreateCuisineCommad(CreateCuisineRequest Cuisine) : IRequest<CuisineResponse>;

public class CreateCuisineCommandHandler : IRequestHandler<CreateCuisineCommad, CuisineResponse>
{
    private readonly ICuisineRepository _cuisineRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateCuisineCommandHandler(ICuisineRepository cuisineRepository, IPublishEndpoint publishEndpoint)
    {
        _cuisineRepository = cuisineRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CuisineResponse> Handle(CreateCuisineCommad request, CancellationToken cancellationToken)
    {
        Cuisine cuisine = new Cuisine()
        {
            Name = request.Cuisine.Name,
            Description = request.Cuisine.Description,
        };
        var result = await _cuisineRepository.CreateAsync(cuisine, cancellationToken);
        if (result is null)
        {
            throw new Exception("Failed to create cuisine");
        }

        await _publishEndpoint.Publish(
            new CuisineCreatedEvent
            {
                Id = result.Id,
                CreatedOnUtc = DateTime.UtcNow
            }, cancellationToken);
            
        CuisineResponse cuisineResponse = new CuisineResponse()
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
        };
        return cuisineResponse;
    }
}