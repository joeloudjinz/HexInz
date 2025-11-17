using System.Text.RegularExpressions;

namespace HexInz.Core.Domain.Inventory.ValueObjects;

public class ISBN
{
    public string Value { get; private set; }

    public ISBN(string value)
    {
        if (!IsValidIsbn(value)) throw new ArgumentException("Invalid ISBN format", nameof(value));
        Value = value.Replace("-", "").Replace(" ", ""); // Normalize the ISBN
    }

    private static bool IsValidIsbn(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn)) return false;

        // Remove hyphens and spaces
        var cleanIsbn = isbn.Replace("-", "").Replace(" ", "");

        return cleanIsbn.Length switch
        {
            // Check if it's ISBN-10 or ISBN-13
            10 => IsValidIsbn10(cleanIsbn),
            13 => IsValidIsbn13(cleanIsbn),
            _ => false
        };
    }

    private static bool IsValidIsbn10(string isbn)
    {
        if (!Regex.IsMatch(isbn, @"^\d{9}[\dX]$")) return false;

        var sum = 0;
        for (var i = 0; i < 9; i++)
        {
            if (!char.IsDigit(isbn[i])) return false;
            sum += (isbn[i] - '0') * (10 - i);
        }

        var lastChar = isbn[9];
        if (lastChar == 'X') sum += 10;
        else if (char.IsDigit(lastChar)) sum += (lastChar - '0');
        else return false;

        return (sum % 11 == 0);
    }

    private static bool IsValidIsbn13(string isbn)
    {
        if (!Regex.IsMatch(isbn, @"^\d{13}$")) return false;

        var sum = 0;
        for (var i = 0; i < 12; i++)
        {
            var digit = isbn[i] - '0';
            sum += (i % 2 == 0) ? digit : digit * 3;
        }

        var checkDigit = (10 - (sum % 10)) % 10;
        return checkDigit == (isbn[12] - '0');
    }

    public override string ToString() => Value;
        
    public override bool Equals(object? obj)
    {
        if (obj is ISBN other) return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        return false;
    }

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;
}