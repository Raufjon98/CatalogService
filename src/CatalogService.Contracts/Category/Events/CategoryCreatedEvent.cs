namespace CatalogService.Contracts.Category.Events;

public record CategoryCreatedEvent
{
    public required string Id { get; init; }
    public DateTime CreatedOnUtc { get; init; }
}