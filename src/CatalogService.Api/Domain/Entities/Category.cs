using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Api.Domain.Entities;

public class Category : BaseAuditableEntity
{
    [BsonElement("name"), BsonRepresentation(BsonType.String)]  
    public required string Name { get; set; }
    [BsonElement("description"), BsonRepresentation(BsonType.String)]
    public string? Descriprion { get; set; }
    [BsonElement("isAvtive"), BsonRepresentation(BsonType.Boolean)]
    public bool IsActive { get; set; } =  true;
}