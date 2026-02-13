namespace CatalogService.Contracts.Restaurant.Events;

public record RestaurantCreatedEvent
{
    public required string Id {get; init;}
    public DateTime CreatedOnUtc { get; init; }
}