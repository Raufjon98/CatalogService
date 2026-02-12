using MessagePack;

namespace CatalogService.Contracts.Category.Requests;

[MessagePackObject]
public record CreateCategoryRequest
{
    [Key(0)]
    public required string Name { get; set; }
    [Key(1)]
    public string? Descriprion { get; set; }
};