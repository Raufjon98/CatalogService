using MessagePack;

namespace CatalogService.Contracts.Address.Resposes;

[MessagePackObject]
public record AddressResponse
{
    [Key(0)]
    public string? Id { get; set; }
    [Key(1)]
    public string City { get; set; } = string.Empty;
    [Key(2)]
    public string State { get; set; } = string.Empty;
    [Key(3)]
    public string ZipCode { get; set; } = string.Empty;
    [Key(4)]
    public string Street { get; set; } = string.Empty;
    [Key(5)]
    public string House { get; set; } = string.Empty;
    [Key(6)]
    public string Description { get; set; } = string.Empty;
};