namespace CatalogService.Contracts.Cuisine.Events;

public record CuisineDeletedEvent
{
    public required string Id { get; init; }
    public DateTime DeletedOnUtc { get; init; }
}