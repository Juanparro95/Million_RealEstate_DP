namespace RealEstateAPI.Domain.ValueObjects;

public record Money(decimal Amount, string Currency = "COP")
{
    public static Money Zero => new(0);
    
    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies");
            
        return new Money(left.Amount + right.Amount, left.Currency);
    }
    
    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot subtract money with different currencies");
            
        return new Money(left.Amount - right.Amount, left.Currency);
    }
    
    public static bool operator >(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare money with different currencies");
            
        return left.Amount > right.Amount;
    }
    
    public static bool operator <(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare money with different currencies");
            
        return left.Amount < right.Amount;
    }
    
    public static bool operator >=(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare money with different currencies");
            
        return left.Amount >= right.Amount;
    }
    
    public static bool operator <=(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare money with different currencies");
            
        return left.Amount <= right.Amount;
    }
    
    public override string ToString()
    {
        return $"{Amount:C} {Currency}";
    }
}