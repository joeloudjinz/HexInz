using FluentAssertions;
using HexInz.Core.Domain.Circulation.ValueObjects;

namespace HexInz.Domain.UnitTests.Circulation.ValueObjects;

public class BookCopyIdTests
{
    [Fact]
    public void Constructor_WithValidGuid_ShouldNotThrow()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var action = () => new BookCopyId(guid);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithValidGuid_ShouldSetValue()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var bookCopyId = new BookCopyId(guid);

        // Assert
        bookCopyId.Value.Should().Be(guid);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ShouldThrowArgumentException()
    {
        // Arrange
        var guid = Guid.Empty;

        // Act
        var action = () => new BookCopyId(guid);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*BookCopyId cannot be empty*");
    }

    [Fact]
    public void Equals_WithSameBookCopyIdValues_ShouldReturnTrue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var bookCopyId1 = new BookCopyId(guid);
        var bookCopyId2 = new BookCopyId(guid);

        // Act
        var result = bookCopyId1.Equals(bookCopyId2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentBookCopyIdValues_ShouldReturnFalse()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var bookCopyId1 = new BookCopyId(guid1);
        var bookCopyId2 = new BookCopyId(guid2);

        // Act
        var result = bookCopyId1.Equals(bookCopyId2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameBookCopyIdValues_ShouldReturnSameHash()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var bookCopyId1 = new BookCopyId(guid);
        var bookCopyId2 = new BookCopyId(guid);

        // Act
        var hash1 = bookCopyId1.GetHashCode();
        var hash2 = bookCopyId2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var bookCopyId = new BookCopyId(guid);

        // Act
        var result = bookCopyId.ToString();

        // Assert
        result.Should().Be(guid.ToString());
    }
}