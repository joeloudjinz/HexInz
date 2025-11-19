using FluentAssertions;
using HexInz.Core.Domain.Circulation.Entities;
using HexInz.Core.Domain.Circulation.ValueObjects;

namespace HexInz.Domain.UnitTests.Circulation.Entities;

public class HoldableBookTests
{
    [Fact]
    public void Constructor_WithValidBookId_ShouldSetProperties()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());

        // Act
        var holdableBook = new HoldableBook(bookId);

        // Assert
        holdableBook.BookId.Should().Be(bookId);
        holdableBook.Holds.Should().BeEmpty();
        holdableBook.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        holdableBook.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void PlaceHold_WithValidPatronId_ShouldAddHold()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());
        var holdableBook = new HoldableBook(bookId);
        var patronId = new PatronId(Guid.NewGuid());

        // Act
        holdableBook.PlaceHold(patronId);

        // Assert
        holdableBook.Holds.Should().HaveCount(1);
        holdableBook.Holds.First().PatronId.Should().Be(patronId);
        holdableBook.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void PlaceHold_WithSamePatronIdTwice_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());
        var holdableBook = new HoldableBook(bookId);
        var patronId = new PatronId(Guid.NewGuid());

        // Place first hold
        holdableBook.PlaceHold(patronId);

        // Act
        var action = () => holdableBook.PlaceHold(patronId);

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage($"*Patron {patronId.Value} already has a hold on this book*");
    }

    [Fact]
    public void PlaceHold_WithDifferentPatronIds_ShouldAddMultipleHolds()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());
        var holdableBook = new HoldableBook(bookId);
        var patronId1 = new PatronId(Guid.NewGuid());
        var patronId2 = new PatronId(Guid.NewGuid());

        // Act
        holdableBook.PlaceHold(patronId1);
        holdableBook.PlaceHold(patronId2);

        // Assert
        holdableBook.Holds.Should().HaveCount(2);
        holdableBook.Holds.Select(h => h.PatronId).Should().Contain([patronId1, patronId2]);
    }

    [Fact]
    public void FulfillNextHold_WithHoldsAvailable_ShouldFulfillFirstHold()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());
        var holdableBook = new HoldableBook(bookId);
        var patronId1 = new PatronId(Guid.NewGuid());
        var patronId2 = new PatronId(Guid.NewGuid());

        holdableBook.PlaceHold(patronId1);
        holdableBook.PlaceHold(patronId2);

        var firstHold = holdableBook.Holds.First(h => h.PatronId == patronId1);

        // Act
        holdableBook.FulfillNextHold();

        // Assert
        firstHold.IsFulfilled.Should().BeTrue();
        holdableBook.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void FulfillNextHold_WithNoHoldsAvailable_ShouldNotThrow()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());
        var holdableBook = new HoldableBook(bookId);

        // Act
        var action = () => holdableBook.FulfillNextHold();

        // Assert
        action.Should().NotThrow();
        holdableBook.Holds.Should().BeEmpty();
    }

    [Fact]
    public void RemoveHold_WithValidHoldId_ShouldCancelHold()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());
        var holdableBook = new HoldableBook(bookId);
        var patronId = new PatronId(Guid.NewGuid());
        
        holdableBook.PlaceHold(patronId);
        var holdId = holdableBook.Holds.First().Id;

        // Act
        holdableBook.RemoveHold(holdId);

        // Assert
        var hold = holdableBook.Holds.First();
        hold.IsCancelled.Should().BeTrue();
        hold.CancelledAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        holdableBook.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void RemoveHold_WithInvalidHoldId_ShouldNotChangeAnything()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());
        var holdableBook = new HoldableBook(bookId);
        var patronId = new PatronId(Guid.NewGuid());
        
        holdableBook.PlaceHold(patronId);
        var validHoldId = holdableBook.Holds.First().Id;
        var invalidHoldId = Guid.NewGuid();

        // Act
        holdableBook.RemoveHold(invalidHoldId);

        // Assert
        var hold = holdableBook.Holds.First();
        hold.Id.Should().Be(validHoldId);
        hold.IsCancelled.Should().BeFalse();
    }

    [Fact]
    public void GetHoldQueuePosition_WithPatronHavingHold_ShouldReturnPosition()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());
        var holdableBook = new HoldableBook(bookId);
        var patronId1 = new PatronId(Guid.NewGuid());
        var patronId2 = new PatronId(Guid.NewGuid());
        var patronId3 = new PatronId(Guid.NewGuid());

        holdableBook.PlaceHold(patronId1);
        Thread.Sleep(10); // Ensure different placement times
        holdableBook.PlaceHold(patronId2);
        Thread.Sleep(10);
        holdableBook.PlaceHold(patronId3);

        // Act
        var position = holdableBook.GetHoldQueuePosition(patronId2);

        // Assert
        position.Should().Be(2); // Second in the queue
    }

    [Fact]
    public void GetHoldQueuePosition_WithPatronNotHavingHold_ShouldReturnMinusOne()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());
        var holdableBook = new HoldableBook(bookId);
        var patronId = new PatronId(Guid.NewGuid());

        // Act
        var position = holdableBook.GetHoldQueuePosition(patronId);

        // Assert
        position.Should().Be(-1);
    }

    [Fact]
    public void GetActiveHolds_WithMultipleHolds_ShouldReturnUnfulfilledHoldsInOrder()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());
        var holdableBook = new HoldableBook(bookId);
        var patronId1 = new PatronId(Guid.NewGuid());
        var patronId2 = new PatronId(Guid.NewGuid());
        var patronId3 = new PatronId(Guid.NewGuid());

        holdableBook.PlaceHold(patronId1);
        Thread.Sleep(10);
        holdableBook.PlaceHold(patronId2);
        Thread.Sleep(10);
        holdableBook.PlaceHold(patronId3);

        // Fulfill the first hold to test that it's not included
        holdableBook.FulfillNextHold();

        // Act
        var activeHolds = holdableBook.GetActiveHolds();

        // Assert
        activeHolds.Should().HaveCount(2);
        activeHolds.First().PatronId.Should().Be(patronId2);
        activeHolds.Last().PatronId.Should().Be(patronId3);
        activeHolds.Should().ContainSingle(h => h.PatronId.Value == patronId2.Value);
        activeHolds.Should().ContainSingle(h => h.PatronId.Value == patronId3.Value);
    }
}