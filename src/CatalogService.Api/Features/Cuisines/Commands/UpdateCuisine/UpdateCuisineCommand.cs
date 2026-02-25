using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Cuisine.Events;
using CatalogService.Contracts.Cuisine.Requests;
using CatalogService.Contracts.Cuisine.Responses;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Cuisines.Commands;

public record UpdateCuisineCommand(string Id, CreateCuisineRequest Cuisine) : IRequest<CuisineResponse>;

public class UpdateCuisineCommandHandler : IRequestHandler<UpdateCuisineCommand, CuisineResponse>
{
    private readonly ICuisineRepository _cuisineRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateCuisineCommandHandler(ICuisineRepository cuisineRepository, IPublishEndpoint publishEndpoint)
    {
        _cuisineRepository = cuisineRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CuisineResponse> Handle(UpdateCuisineCommand request, CancellationToken cancellationToken)
    {
        Cuisine cuisine = new Cuisine()
        {
            Id = request.Id,
            Name = request.Cuisine.Name,
            Description = request.Cuisine.Description,
        };
        var result = await _cuisineRepository.UpdateAsync(cuisine, cancellationToken);
        if (result is null)
        {
            throw new Exception("Couldn't update cuisine");
        }
        
        await _publishEndpoint.Publish(
            new CuisineUpdatedEvent
            {
                Id = cuisine.Id,
                UpdatedOnUtc = DateTime.UtcNow
            },
            cancellationToken);
        
        CuisineResponse cuisineResponse = new CuisineResponse()
        {   
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
        };
        return cuisineResponse;
    }
}