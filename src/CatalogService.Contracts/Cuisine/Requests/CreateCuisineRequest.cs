using MessagePack;

namespace CatalogService.Contracts.Cuisine.Requests;

[MessagePackObject]
public record CreateCuisineRequest
{
    [Key(0)]
    public required string Name { get; set; }
    [Key(1)]
    public string Description { get; set; } = string.Empty;
};