namespace RealEstateAPI.Domain.ValueObjects;

public record Address(
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode
)
{
    public override string ToString()
    {
        return $"{Street}, {City}, {State}, {Country} {ZipCode}";
    }
    
    public static Address Create(string fullAddress)
    {
        // Simple parser - en un proyecto real sería más robusto
        var parts = fullAddress.Split(',').Select(p => p.Trim()).ToArray();
        
        return new Address(
            Street: parts.Length > 0 ? parts[0] : string.Empty,
            City: parts.Length > 1 ? parts[1] : string.Empty,
            State: parts.Length > 2 ? parts[2] : string.Empty,
            Country: parts.Length > 3 ? parts[3] : "Colombia",
            ZipCode: parts.Length > 4 ? parts[4] : string.Empty
        );
    }
}