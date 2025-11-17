using FluentAssertions;
using HexInz.Core.Domain.Circulation.ValueObjects;

namespace HexInz.Domain.UnitTests.Circulation.ValueObjects;

public class PatronIdTests
{
    [Fact]
    public void Constructor_WithValidGuid_ShouldNotThrow()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var action = () => new PatronId(guid);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithValidGuid_ShouldSetValue()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var patronId = new PatronId(guid);

        // Assert
        patronId.Value.Should().Be(guid);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ShouldThrowArgumentException()
    {
        // Arrange
        var guid = Guid.Empty;

        // Act
        var action = () => new PatronId(guid);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*PatronId cannot be empty*");
    }

    [Fact]
    public void Equals_WithSamePatronIdValues_ShouldReturnTrue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var patronId1 = new PatronId(guid);
        var patronId2 = new PatronId(guid);

        // Act
        var result = patronId1.Equals(patronId2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentPatronIdValues_ShouldReturnFalse()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var patronId1 = new PatronId(guid1);
        var patronId2 = new PatronId(guid2);

        // Act
        var result = patronId1.Equals(patronId2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSamePatronIdValues_ShouldReturnSameHash()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var patronId1 = new PatronId(guid);
        var patronId2 = new PatronId(guid);

        // Act
        var hash1 = patronId1.GetHashCode();
        var hash2 = patronId2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var patronId = new PatronId(guid);

        // Act
        var result = patronId.ToString();

        // Assert
        result.Should().Be(guid.ToString());
    }
}