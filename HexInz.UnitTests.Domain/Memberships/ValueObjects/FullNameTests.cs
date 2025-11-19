using FluentAssertions;
using HexInz.Core.Domain.Memberships.ValueObjects;

namespace HexInz.Domain.UnitTests.Memberships.ValueObjects;

public class FullNameTests
{
    [Fact]
    public void Constructor_WithValidNames_ShouldNotThrow()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var action = () => new FullName(firstName, lastName);

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
        var fullName = new FullName(firstName, lastName);

        // Assert
        fullName.FirstName.Should().Be(firstName);
        fullName.LastName.Should().Be(lastName);
    }

    [Fact]
    public void Constructor_WithWhitespaceNames_ShouldTrim()
    {
        // Arrange
        var firstName = "  John  ";
        var lastName = "  Doe  ";

        // Act
        var fullName = new FullName(firstName, lastName);

        // Assert
        fullName.FirstName.Should().Be("John");
        fullName.LastName.Should().Be("Doe");
    }

    [Fact]
    public void Constructor_WithEmptyFirstName_ShouldThrowArgumentException()
    {
        // Arrange
        var firstName = "";
        var lastName = "Doe";

        // Act
        var action = () => new FullName(firstName, lastName);

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
        var action = () => new FullName(firstName!, lastName);

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
        var action = () => new FullName(firstName, lastName);

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
        var action = () => new FullName(firstName, lastName);

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
        var action = () => new FullName(firstName, lastName!);

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
        var action = () => new FullName(firstName, lastName);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Last name cannot be null or empty*");
    }

    [Fact]
    public void Equals_WithSameFullNameValues_ShouldReturnTrue()
    {
        // Arrange
        var fullName1 = new FullName("John", "Doe");
        var fullName2 = new FullName("John", "Doe");

        // Act
        var result = fullName1.Equals(fullName2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentFirstNames_ShouldReturnFalse()
    {
        // Arrange
        var fullName1 = new FullName("John", "Doe");
        var fullName2 = new FullName("Jane", "Doe");

        // Act
        var result = fullName1.Equals(fullName2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentLastNames_ShouldReturnFalse()
    {
        // Arrange
        var fullName1 = new FullName("John", "Doe");
        var fullName2 = new FullName("John", "Smith");

        // Act
        var result = fullName1.Equals(fullName2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentCaseNames_ShouldReturnTrue()
    {
        // Arrange
        var fullName1 = new FullName("john", "doe");
        var fullName2 = new FullName("John", "Doe");

        // Act
        var result = fullName1.Equals(fullName2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_WithSameFullNameValues_ShouldReturnSameHash()
    {
        // Arrange
        var fullName1 = new FullName("John", "Doe");
        var fullName2 = new FullName("John", "Doe");

        // Act
        var hash1 = fullName1.GetHashCode();
        var hash2 = fullName2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedName()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var fullName = new FullName(firstName, lastName);

        // Act
        var result = fullName.ToString();

        // Assert
        result.Should().Be("John Doe");
    }
}