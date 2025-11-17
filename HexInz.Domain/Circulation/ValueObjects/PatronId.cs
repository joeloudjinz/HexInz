namespace HexInz.Core.Domain.Circulation.ValueObjects;

public class PatronId
{
    public Guid Value { get; private set; }

    public PatronId(Guid value)
    {
        if (value == Guid.Empty) throw new ArgumentException("PatronId cannot be empty", nameof(value));
            
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is PatronId other) return Value.Equals(other.Value);
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();
        
    public override string ToString() => Value.ToString();
}