namespace CatalogService.Contracts.FoodCategory.Events;

public record FoodCategoryCreatedEvent
{
    public required string Id {get; init;}
    public DateTime CreatedOnUtc { get; init; }
}