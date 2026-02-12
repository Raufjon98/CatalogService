using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Api.Domain.Entities;

public class Address : BaseAuditableEntity
{
    [BsonElement("city"), BsonRepresentation(BsonType.String)]
    public string City { get; set; } = string.Empty;

    [BsonElement("state"), BsonRepresentation(BsonType.String)]
    public string State { get; set; } = string.Empty;

    [BsonElement("zipCode"), BsonRepresentation(BsonType.String)]
    public string ZipCode { get; set; } = string.Empty;
    [BsonElement("street"), BsonRepresentation(BsonType.String)]
    public string Street { get; set; } = string.Empty;
    [BsonElement("house"), BsonRepresentation(BsonType.String)]
    public string House { get; set; } = string.Empty;
    [BsonElement("description"), BsonRepresentation(BsonType.String)]
    public string Description { get; set; } = string.Empty;
}