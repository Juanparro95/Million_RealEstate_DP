using MongoDB.Bson.Serialization.Attributes;

namespace RealEstateAPI.Domain.Entities;

[BsonCollection("properties")]
public class Property : BaseEntity
{
    [BsonElement("idProperty")]
    public string IdProperty { get; set; } = string.Empty;
    
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    
    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;
    
    [BsonElement("price")]
    public decimal Price { get; set; }
    
    [BsonElement("codeInternal")]
    public string CodeInternal { get; set; } = string.Empty;
    
    [BsonElement("year")]
    public int Year { get; set; }
    
    [BsonElement("idOwner")]
    public string IdOwner { get; set; } = string.Empty;
    
    // Navigation properties
    [BsonIgnore]
    public virtual Owner Owner { get; set; } = null!;
    
    [BsonIgnore]
    public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
    
    [BsonIgnore]
    public virtual ICollection<PropertyTrace> PropertyTraces { get; set; } = new List<PropertyTrace>();
}