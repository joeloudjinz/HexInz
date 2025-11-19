using FluentAssertions;
using HexInz.Core.Domain.Inventory.ValueObjects;

namespace HexInz.Domain.UnitTests.Inventory.ValueObjects;

public class ISBNTests
{
    [Fact]
    public void Constructor_WithValidISBN10_ShouldNotThrow()
    {
        // Arrange
        var isbn = "0306406152"; // Valid ISBN-10

        // Act
        var action = () => new ISBN(isbn);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithValidISBN10WithHyphens_ShouldNormalizeAndNotThrow()
    {
        // Arrange
        var isbn = "0-306-40615-2";

        // Act
        var result = new ISBN(isbn);

        // Assert
        result.Value.Should().Be("0306406152");
    }

    [Fact]
    public void Constructor_WithValidISBN13_ShouldNotThrow()
    {
        // Arrange
        var isbn = "9780306406157"; // Valid ISBN-13

        // Act
        var action = () => new ISBN(isbn);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithValidISBN13WithHyphens_ShouldNormalizeAndNotThrow()
    {
        // Arrange
        var isbn = "978-0-306-40615-7";

        // Act
        var result = new ISBN(isbn);

        // Assert
        result.Value.Should().Be("9780306406157");
    }

    [Fact]
    public void Constructor_WithInvalidISBN10_ShouldThrowArgumentException()
    {
        // Arrange
        var isbn = "123456789"; // Only 9 digits

        // Act
        var action = () => new ISBN(isbn);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid ISBN format*");
    }

    [Fact]
    public void Constructor_WithInvalidISBN13_ShouldThrowArgumentException()
    {
        // Arrange
        var isbn = "123456789012"; // Only 12 digits

        // Act
        var action = () => new ISBN(isbn);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid ISBN format*");
    }

    [Fact]
    public void Constructor_WithEmptyString_ShouldThrowArgumentException()
    {
        // Arrange
        var isbn = "";

        // Act
        var action = () => new ISBN(isbn);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid ISBN format*");
    }

    [Fact]
    public void Constructor_WithNullString_ShouldThrowArgumentException()
    {
        // Arrange
        string? isbn = null;

        // Act
        var action = () => new ISBN(isbn!);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid ISBN format*");
    }

    [Fact]
    public void Constructor_WithISBN10WithInvalidCheckDigit_ShouldThrowArgumentException()
    {
        // Arrange
        var isbn = "0306406153"; // Valid 10 digits but incorrect check digit

        // Act
        var action = () => new ISBN(isbn);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid ISBN format*");
    }

    [Fact]
    public void Constructor_WithISBN13WithInvalidCheckDigit_ShouldThrowArgumentException()
    {
        // Arrange
        var isbn = "9780306406158"; // Valid 13 digits but incorrect check digit

        // Act
        var action = () => new ISBN(isbn);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid ISBN format*");
    }

    [Fact]
    public void Equals_WithSameISBNValues_ShouldReturnTrue()
    {
        // Arrange
        var isbn1 = new ISBN("0306406152");
        var isbn2 = new ISBN("0306406152");

        // Act
        var result = isbn1.Equals(isbn2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentISBNValues_ShouldReturnFalse()
    {
        // Arrange
        var isbn1 = new ISBN("0306406152");
        var isbn2 = new ISBN("9780306406157");

        // Act
        var result = isbn1.Equals(isbn2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithSameISBNValuesButDifferentFormat_ShouldReturnTrue()
    {
        // Arrange
        var isbn1 = new ISBN("0-306-40615-2"); // With hyphens
        var isbn2 = new ISBN("0306406152");     // Without hyphens

        // Act
        var result = isbn1.Equals(isbn2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_WithSameISBNValues_ShouldReturnSameHash()
    {
        // Arrange
        var isbn1 = new ISBN("0306406152");
        var isbn2 = new ISBN("0306406152");

        // Act
        var hash1 = isbn1.GetHashCode();
        var hash2 = isbn2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var expected = "0306406152";
        var isbn = new ISBN(expected);

        // Act
        var result = isbn.ToString();

        // Assert
        result.Should().Be(expected);
    }
}