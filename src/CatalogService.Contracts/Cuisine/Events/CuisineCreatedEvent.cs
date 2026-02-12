namespace CatalogService.Contracts.Cuisine.Events;

public record CuisineCreatedEvent
{
    public required string Id { get; init; }
    public DateTime CreatedOnUtc { get; init; }
}