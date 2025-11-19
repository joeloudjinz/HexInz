namespace HexInz.Core.Domain.Inventory.ValueObjects;

public class Author
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public Author(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name cannot be null or empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public override string ToString() => $"{FirstName} {LastName}";

    public override bool Equals(object? obj)
    {
        if (obj is Author other) return FirstName.Equals(other.FirstName, StringComparison.OrdinalIgnoreCase) && LastName.Equals(other.LastName, StringComparison.OrdinalIgnoreCase);
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(FirstName?.ToLowerInvariant(), LastName?.ToLowerInvariant());
}