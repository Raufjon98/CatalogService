using MessagePack;

namespace CatalogService.Contracts.Category.Responses;

[MessagePackObject]
public record CategoryResponse
{
    [Key(0)]
    public string? Id { get; set; }
    [Key(1)]
    public required string Name { get; set; }
    [Key(2)]
    public string? Descriprion { get; set; }
};