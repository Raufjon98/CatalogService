namespace CatalogService.Contracts.Cuisine.Events;

public record CuisineUpdatedEvent
{
    public required string Id { get; init; }
    public DateTime UpdatedOnUtc { get; init; }
}