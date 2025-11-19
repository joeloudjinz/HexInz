namespace HexInz.Core.Domain.Inventory.ValueObjects;

public class PublicationDetails
{
    public string Publisher { get; private set; }
    public int PublicationYear { get; private set; }

    public PublicationDetails(string publisher, int publicationYear)
    {
        if (string.IsNullOrWhiteSpace(publisher)) throw new ArgumentException("Publisher cannot be null or empty", nameof(publisher));
        if (publicationYear < 1000 || publicationYear > DateTime.Now.Year + 1) throw new ArgumentException("Publication year must be a valid year", nameof(publicationYear));

        Publisher = publisher.Trim();
        PublicationYear = publicationYear;
    }

    public override bool Equals(object? obj)
    {
        if (obj is PublicationDetails other) return Publisher.Equals(other.Publisher, StringComparison.OrdinalIgnoreCase) && PublicationYear == other.PublicationYear;
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(Publisher?.ToLowerInvariant(), PublicationYear);
}