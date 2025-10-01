using MongoDB.Bson.Serialization.Attributes;

namespace RealEstateAPI.Domain.Entities;

[BsonCollection("propertyTraces")]
public class PropertyTrace : BaseEntity
{
    [BsonElement("idPropertyTrace")]
    public string IdPropertyTrace { get; set; } = string.Empty;
    
    [BsonElement("dateSale")]
    public DateTime DateSale { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    
    [BsonElement("value")]
    public decimal Value { get; set; }
    
    [BsonElement("tax")]
    public decimal Tax { get; set; }
    
    [BsonElement("idProperty")]
    public string IdProperty { get; set; } = string.Empty;
    
    // Navigation property
    [BsonIgnore]
    public virtual Property Property { get; set; } = null!;
}