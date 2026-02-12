using MessagePack;

namespace CatalogService.Contracts.Cuisine.Responses;

[MessagePackObject]
public record CuisineResponse
{
    [Key(0)]
    public string? Id { get; set; }
    [Key(1)]
    public required string Name { get; set; }
    [Key(2)]
    public string Description { get; set; } = string.Empty;
};