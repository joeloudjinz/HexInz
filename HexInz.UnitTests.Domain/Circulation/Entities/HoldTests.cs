using FluentAssertions;
using HexInz.Core.Domain.Circulation.Entities;
using HexInz.Core.Domain.Circulation.ValueObjects;

namespace HexInz.Domain.UnitTests.Circulation.Entities;

public class HoldTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldSetProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var patronId = new PatronId(Guid.NewGuid());

        // Act
        var hold = new Hold(id, patronId, DateTime.UtcNow);

        // Assert
        hold.Id.Should().Be(id);
        hold.PatronId.Should().Be(patronId);
        hold.PlacedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        hold.FulfilledAt.Should().BeNull();
        hold.CancelledAt.Should().BeNull();
        hold.IsFulfilled.Should().BeFalse();
        hold.IsCancelled.Should().BeFalse();
        hold.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Fulfill_WhenNotFulfilledOrCancelled_ShouldUpdateFulfilledAt()
    {
        // Arrange
        var id = Guid.NewGuid();
        var patronId = new PatronId(Guid.NewGuid());
        var hold = new Hold(id, patronId, DateTime.UtcNow.AddMinutes(-10));

        // Act
        hold.Fulfill();

        // Assert
        hold.FulfilledAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        hold.IsFulfilled.Should().BeTrue();
        hold.IsCancelled.Should().BeFalse();
        hold.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Fulfill_WhenAlreadyFulfilled_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var patronId = new PatronId(Guid.NewGuid());
        var hold = new Hold(id, patronId, DateTime.UtcNow.AddMinutes(-10));
        
        // Fulfill first
        hold.Fulfill();

        // Act
        var action = () => hold.Fulfill();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot fulfill a hold that is already fulfilled or cancelled");
    }

    [Fact]
    public void Fulfill_WhenAlreadyCancelled_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var patronId = new PatronId(Guid.NewGuid());
        var hold = new Hold(id, patronId, DateTime.UtcNow.AddMinutes(-10));
        
        // Cancel first
        hold.Cancel();

        // Act
        var action = () => hold.Fulfill();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot fulfill a hold that is already fulfilled or cancelled");
    }

    [Fact]
    public void Cancel_WhenNotFulfilledOrCancelled_ShouldUpdateCancelledAt()
    {
        // Arrange
        var id = Guid.NewGuid();
        var patronId = new PatronId(Guid.NewGuid());
        var hold = new Hold(id, patronId, DateTime.UtcNow.AddMinutes(-10));

        // Act
        hold.Cancel();

        // Assert
        hold.CancelledAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        hold.IsCancelled.Should().BeTrue();
        hold.IsFulfilled.Should().BeFalse();
        hold.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Cancel_WhenAlreadyFulfilled_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var patronId = new PatronId(Guid.NewGuid());
        var hold = new Hold(id, patronId, DateTime.UtcNow.AddMinutes(-10));
        
        // Fulfill first
        hold.Fulfill();

        // Act
        var action = () => hold.Cancel();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot cancel a hold that is already fulfilled or cancelled");
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var patronId = new PatronId(Guid.NewGuid());
        var hold = new Hold(id, patronId, DateTime.UtcNow.AddMinutes(-10));
        
        // Cancel first
        hold.Cancel();

        // Act
        var action = () => hold.Cancel();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot cancel a hold that is already fulfilled or cancelled");
    }
}