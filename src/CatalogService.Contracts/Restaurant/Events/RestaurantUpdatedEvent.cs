namespace CatalogService.Contracts.Restaurant.Events;

public record RestaurantUpdatedEvent
{
    public required string Id {get; init;}
    public DateTime UpdatedOnUtc { get; init; }
}