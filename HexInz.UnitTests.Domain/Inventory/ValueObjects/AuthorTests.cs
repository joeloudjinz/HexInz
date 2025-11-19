using FluentAssertions;
using HexInz.Core.Domain.Inventory.ValueObjects;

namespace HexInz.Domain.UnitTests.Inventory.ValueObjects;

public class AuthorTests
{
    [Fact]
    public void Constructor_WithValidNames_ShouldNotThrow()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var action = () => new Author(firstName, lastName);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithValidNames_ShouldSetProperties()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var author = new Author(firstName, lastName);

        // Assert
        author.FirstName.Should().Be(firstName);
        author.LastName.Should().Be(lastName);
    }

    [Fact]
    public void Constructor_WithWhitespaceNames_ShouldTrim()
    {
        // Arrange
        var firstName = "  John  ";
        var lastName = "  Doe  ";

        // Act
        var author = new Author(firstName, lastName);

        // Assert
        author.FirstName.Should().Be("John");
        author.LastName.Should().Be("Doe");
    }

    [Fact]
    public void Constructor_WithEmptyFirstName_ShouldThrowArgumentException()
    {
        // Arrange
        var firstName = "";
        var lastName = "Doe";

        // Act
        var action = () => new Author(firstName, lastName);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*First name cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithNullFirstName_ShouldThrowArgumentException()
    {
        // Arrange
        string? firstName = null;
        var lastName = "Doe";

        // Act
        var action = () => new Author(firstName!, lastName);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*First name cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithWhitespaceOnlyFirstName_ShouldThrowArgumentException()
    {
        // Arrange
        var firstName = "   ";
        var lastName = "Doe";

        // Act
        var action = () => new Author(firstName, lastName);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*First name cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithEmptyLastName_ShouldThrowArgumentException()
    {
        // Arrange
        var firstName = "John";
        var lastName = "";

        // Act
        var action = () => new Author(firstName, lastName);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Last name cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithNullLastName_ShouldThrowArgumentException()
    {
        // Arrange
        var firstName = "John";
        string? lastName = null;

        // Act
        var action = () => new Author(firstName, lastName!);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Last name cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithWhitespaceOnlyLastName_ShouldThrowArgumentException()
    {
        // Arrange
        var firstName = "John";
        var lastName = "   ";

        // Act
        var action = () => new Author(firstName, lastName);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Last name cannot be null or empty*");
    }

    [Fact]
    public void Equals_WithSameAuthorValues_ShouldReturnTrue()
    {
        // Arrange
        var author1 = new Author("John", "Doe");
        var author2 = new Author("John", "Doe");

        // Act
        var result = author1.Equals(author2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentFirstNames_ShouldReturnFalse()
    {
        // Arrange
        var author1 = new Author("John", "Doe");
        var author2 = new Author("Jane", "Doe");

        // Act
        var result = author1.Equals(author2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentLastNames_ShouldReturnFalse()
    {
        // Arrange
        var author1 = new Author("John", "Doe");
        var author2 = new Author("John", "Smith");

        // Act
        var result = author1.Equals(author2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentCaseNames_ShouldReturnTrue()
    {
        // Arrange
        var author1 = new Author("john", "doe");
        var author2 = new Author("John", "Doe");

        // Act
        var result = author1.Equals(author2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_WithSameAuthorValues_ShouldReturnSameHash()
    {
        // Arrange
        var author1 = new Author("John", "Doe");
        var author2 = new Author("John", "Doe");

        // Act
        var hash1 = author1.GetHashCode();
        var hash2 = author2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedName()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var author = new Author(firstName, lastName);

        // Act
        var result = author.ToString();

        // Assert
        result.Should().Be("John Doe");
    }
}