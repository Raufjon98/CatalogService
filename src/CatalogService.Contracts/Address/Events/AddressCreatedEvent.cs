namespace CatalogService.Contracts.Address.Events;

public record AddressCreatedEvent
{
    public required string Id { get; init; }
    public DateTime CreatedOnUtc { get; init; }
}