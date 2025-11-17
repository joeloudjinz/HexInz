using FluentAssertions;
using HexInz.Core.Domain.Circulation.ValueObjects;

namespace HexInz.Domain.UnitTests.Circulation.ValueObjects;

public class DueDateTests
{
    [Fact]
    public void Constructor_WithFutureDate_ShouldNotThrow()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);

        // Act
        var action = () => new DueDate(futureDate);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithFutureDate_ShouldSetValue()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);

        // Act
        var dueDate = new DueDate(futureDate);

        // Assert
        dueDate.Value.Should().Be(futureDate);
    }

    [Fact]
    public void Constructor_WithCurrentDate_ShouldThrowArgumentException()
    {
        // Arrange
        var currentDate = DateTime.UtcNow;

        // Act
        var action = () => new DueDate(currentDate);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Due date must be in the future*");
    }

    [Fact]
    public void Constructor_WithPastDate_ShouldThrowArgumentException()
    {
        // Arrange
        var pastDate = DateTime.UtcNow.AddDays(-1);

        // Act
        var action = () => new DueDate(pastDate);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Due date must be in the future*");
    }

    [Fact]
    public void Equals_WithSameDueDateValues_ShouldReturnTrue()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);
        var dueDate1 = new DueDate(futureDate);
        var dueDate2 = new DueDate(futureDate);

        // Act
        var result = dueDate1.Equals(dueDate2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentDueDateValues_ShouldReturnFalse()
    {
        // Arrange
        var futureDate1 = DateTime.UtcNow.AddDays(1);
        var futureDate2 = DateTime.UtcNow.AddDays(2);
        var dueDate1 = new DueDate(futureDate1);
        var dueDate2 = new DueDate(futureDate2);

        // Act
        var result = dueDate1.Equals(dueDate2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameDueDateValues_ShouldReturnSameHash()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);
        var dueDate1 = new DueDate(futureDate);
        var dueDate2 = new DueDate(futureDate);

        // Act
        var hash1 = dueDate1.GetHashCode();
        var hash2 = dueDate2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);
        var dueDate = new DueDate(futureDate);

        // Act
        var result = dueDate.ToString();

        // Assert
        result.Should().Be(futureDate.ToString());
    }
}