using MessagePack;

namespace CatalogService.Contracts.Address.Requests;

[MessagePackObject]
public record CreateAddressRequest
{
    [Key(0)]
    public string City { get; set; } = string.Empty;
    [Key(1)]
    public string State { get; set; } = string.Empty;
    [Key(2)]
    public string ZipCode { get; set; } = string.Empty;
    [Key(3)]
    public string Street { get; set; } = string.Empty;
    [Key(4)]
    public string House { get; set; } = string.Empty;
    [Key(5)]
    public string Description { get; set; } = string.Empty;
};