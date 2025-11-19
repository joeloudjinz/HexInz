namespace HexInz.Core.Domain.Circulation.ValueObjects;

public class DueDate
{
    public DateTime Value { get; private set; }

    public DueDate(DateTime value)
    {
        // Ensure the due date is in the future
        if (value <= DateTime.UtcNow) throw new ArgumentException("Due date must be in the future", nameof(value));

        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is DueDate other) return Value.Equals(other.Value);
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}