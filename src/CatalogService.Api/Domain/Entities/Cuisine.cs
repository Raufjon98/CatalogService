using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Api.Domain.Entities;

public class Cuisine :BaseAuditableEntity
{
    [BsonElement("name"), BsonRepresentation(BsonType.String)]
    public required string Name { get; set; }
    [BsonElement("description"), BsonRepresentation(BsonType.String)]
    public string Description { get; set; } = string.Empty;
}