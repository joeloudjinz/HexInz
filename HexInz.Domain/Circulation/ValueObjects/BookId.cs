namespace HexInz.Core.Domain.Circulation.ValueObjects;

public class BookId
{
    public Guid Value { get; private set; }

    public BookId(Guid value)
    {
        if (value == Guid.Empty) throw new ArgumentException("BookId cannot be empty", nameof(value));

        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is BookId other) return Value.Equals(other.Value);
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}