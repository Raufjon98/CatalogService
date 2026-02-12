using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Api.Domain.Entities;

public class FoodCategory : BaseAuditableEntity
{
    [BsonElement ("name"), BsonRepresentation(BsonType.String)]
    public required string Name { get; set; }
    [BsonElement("availability"), BsonRepresentation(BsonType.Boolean)]
    public bool Availability { get; set; } =  true;
}