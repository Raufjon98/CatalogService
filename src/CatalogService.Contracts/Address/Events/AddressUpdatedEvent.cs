namespace CatalogService.Contracts.Address.Events;

public record AddressUpdatedEvent
{
    public required string Id { get; init; }
    public DateTime UpdatedOnUtc { get; init; }
}