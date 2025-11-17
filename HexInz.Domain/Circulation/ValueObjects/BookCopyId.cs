namespace HexInz.Core.Domain.Circulation.ValueObjects;

public class BookCopyId
{
    public Guid Value { get; private set; }

    public BookCopyId(Guid value)
    {
        if (value == Guid.Empty) throw new ArgumentException("BookCopyId cannot be empty", nameof(value));

        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is BookCopyId other) return Value.Equals(other.Value);
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}