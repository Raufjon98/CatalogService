namespace CatalogService.Contracts.Restaurant.Events;

public record RestaurantDeletedEvent
{
    public required string Id {get; init;}
    public DateTime DeletedOnUtc { get; init; }
}