namespace HexInz.Core.Domain.Memberships.ValueObjects;

public class Address
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }

    public Address(string street, string city, string state, string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("Street cannot be null or empty", nameof(street));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City cannot be null or empty", nameof(city));
        if (string.IsNullOrWhiteSpace(state)) throw new ArgumentException("State cannot be null or empty", nameof(state));
        if (string.IsNullOrWhiteSpace(postalCode)) throw new ArgumentException("Postal code cannot be null or empty", nameof(postalCode));
        if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("Country cannot be null or empty", nameof(country));

        Street = street.Trim();
        City = city.Trim();
        State = state.Trim();
        PostalCode = postalCode.Trim();
        Country = country.Trim();
    }

    public override string ToString() => $"{Street}, {City}, {State} {PostalCode}, {Country}";

    public override bool Equals(object? obj)
    {
        if (obj is Address other)
        {
            return Street.Equals(other.Street, StringComparison.OrdinalIgnoreCase) &&
                   City.Equals(other.City, StringComparison.OrdinalIgnoreCase) &&
                   State.Equals(other.State, StringComparison.OrdinalIgnoreCase) &&
                   PostalCode.Equals(other.PostalCode, StringComparison.OrdinalIgnoreCase) &&
                   Country.Equals(other.Country, StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    public override int GetHashCode() => HashCode.Combine(
        Street?.ToLowerInvariant(),
        City?.ToLowerInvariant(),
        State?.ToLowerInvariant(),
        PostalCode?.ToLowerInvariant(),
        Country?.ToLowerInvariant()
    );
}