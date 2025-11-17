using FluentAssertions;
using HexInz.Core.Domain.Circulation.ValueObjects;

namespace HexInz.Domain.UnitTests.Circulation.ValueObjects;

public class BookIdTests
{
    [Fact]
    public void Constructor_WithValidGuid_ShouldNotThrow()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var action = () => new BookId(guid);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithValidGuid_ShouldSetValue()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var bookId = new BookId(guid);

        // Assert
        bookId.Value.Should().Be(guid);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ShouldThrowArgumentException()
    {
        // Arrange
        var guid = Guid.Empty;

        // Act
        var action = () => new BookId(guid);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*BookId cannot be empty*");
    }

    [Fact]
    public void Equals_WithSameBookIdValues_ShouldReturnTrue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var bookId1 = new BookId(guid);
        var bookId2 = new BookId(guid);

        // Act
        var result = bookId1.Equals(bookId2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentBookIdValues_ShouldReturnFalse()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var bookId1 = new BookId(guid1);
        var bookId2 = new BookId(guid2);

        // Act
        var result = bookId1.Equals(bookId2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameBookIdValues_ShouldReturnSameHash()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var bookId1 = new BookId(guid);
        var bookId2 = new BookId(guid);

        // Act
        var hash1 = bookId1.GetHashCode();
        var hash2 = bookId2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var bookId = new BookId(guid);

        // Act
        var result = bookId.ToString();

        // Assert
        result.Should().Be(guid.ToString());
    }
}