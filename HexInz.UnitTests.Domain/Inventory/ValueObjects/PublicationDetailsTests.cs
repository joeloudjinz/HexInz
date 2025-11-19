using FluentAssertions;
using HexInz.Core.Domain.Inventory.ValueObjects;

namespace HexInz.Domain.UnitTests.Inventory.ValueObjects;

public class PublicationDetailsTests
{
    [Fact]
    public void Constructor_WithValidDetails_ShouldNotThrow()
    {
        // Arrange
        var publisher = "Publisher Name";
        var publicationYear = DateTime.Now.Year;

        // Act
        var action = () => new PublicationDetails(publisher, publicationYear);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithValidDetails_ShouldSetProperties()
    {
        // Arrange
        var publisher = "Publisher Name";
        var publicationYear = 2020;

        // Act
        var details = new PublicationDetails(publisher, publicationYear);

        // Assert
        details.Publisher.Should().Be(publisher);
        details.PublicationYear.Should().Be(publicationYear);
    }

    [Fact]
    public void Constructor_WithWhitespacePublisher_ShouldTrim()
    {
        // Arrange
        var publisher = "  Publisher Name  ";
        var publicationYear = 2020;

        // Act
        var details = new PublicationDetails(publisher, publicationYear);

        // Assert
        details.Publisher.Should().Be("Publisher Name");
    }

    [Fact]
    public void Constructor_WithEmptyPublisher_ShouldThrowArgumentException()
    {
        // Arrange
        var publisher = "";
        var publicationYear = 2020;

        // Act
        var action = () => new PublicationDetails(publisher, publicationYear);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Publisher cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithNullPublisher_ShouldThrowArgumentException()
    {
        // Arrange
        string? publisher = null;
        var publicationYear = 2020;

        // Act
        var action = () => new PublicationDetails(publisher!, publicationYear);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Publisher cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithWhitespaceOnlyPublisher_ShouldThrowArgumentException()
    {
        // Arrange
        var publisher = "   ";
        var publicationYear = 2020;

        // Act
        var action = () => new PublicationDetails(publisher, publicationYear);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Publisher cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithFuturePublicationYear_ShouldThrowArgumentException()
    {
        // Arrange
        var publisher = "Publisher Name";
        var publicationYear = DateTime.Now.Year + 2; // More than 1 year in the future

        // Act
        var action = () => new PublicationDetails(publisher, publicationYear);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Publication year must be a valid year*");
    }

    [Fact]
    public void Constructor_WithValidFuturePublicationYear_ShouldNotThrow()
    {
        // Arrange
        var publisher = "Publisher Name";
        var publicationYear = DateTime.Now.Year + 1; // 1 year in the future is allowed

        // Act
        var action = () => new PublicationDetails(publisher, publicationYear);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithPastPublicationYear_ShouldNotThrow()
    {
        // Arrange
        var publisher = "Publisher Name";
        var publicationYear = 1900; // Past year

        // Act
        var action = () => new PublicationDetails(publisher, publicationYear);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithInvalidPublicationYear_ShouldThrowArgumentException()
    {
        // Arrange
        var publisher = "Publisher Name";
        var publicationYear = 999; // Before 1000

        // Act
        var action = () => new PublicationDetails(publisher, publicationYear);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Publication year must be a valid year*");
    }

    [Fact]
    public void Equals_WithSamePublicationDetailsValues_ShouldReturnTrue()
    {
        // Arrange
        var details1 = new PublicationDetails("Publisher", 2020);
        var details2 = new PublicationDetails("Publisher", 2020);

        // Act
        var result = details1.Equals(details2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentPublishers_ShouldReturnFalse()
    {
        // Arrange
        var details1 = new PublicationDetails("Publisher1", 2020);
        var details2 = new PublicationDetails("Publisher2", 2020);

        // Act
        var result = details1.Equals(details2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentPublicationYears_ShouldReturnFalse()
    {
        // Arrange
        var details1 = new PublicationDetails("Publisher", 2020);
        var details2 = new PublicationDetails("Publisher", 2021);

        // Act
        var result = details1.Equals(details2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentCasePublishers_ShouldReturnTrue()
    {
        // Arrange
        var details1 = new PublicationDetails("publisher", 2020);
        var details2 = new PublicationDetails("Publisher", 2020);

        // Act
        var result = details1.Equals(details2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_WithSamePublicationDetailsValues_ShouldReturnSameHash()
    {
        // Arrange
        var details1 = new PublicationDetails("Publisher", 2020);
        var details2 = new PublicationDetails("Publisher", 2020);

        // Act
        var hash1 = details1.GetHashCode();
        var hash2 = details2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }
}