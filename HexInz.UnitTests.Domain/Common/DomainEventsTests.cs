using FluentAssertions;
using HexInz.Core.Domain.Circulation.Events;
using HexInz.Core.Domain.Memberships.Events;
using HexInz.Core.Domain.Memberships.ValueObjects;

namespace HexInz.Domain.UnitTests.Common;

public class DomainEventsTests
{
    [Fact]
    public void BookCheckedOut_Event_ShouldHaveCorrectProperties()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var bookCopyId = Guid.NewGuid();
        var patronId = Guid.NewGuid();
        var checkoutDate = DateTime.UtcNow;
        var dueDate = DateTime.UtcNow.AddDays(14);

        // Act
        var bookCheckedOut = new BookCheckedOut(loanId, bookCopyId, patronId, checkoutDate, dueDate);

        // Assert
        bookCheckedOut.LoanId.Should().Be(loanId);
        bookCheckedOut.BookCopyId.Should().Be(bookCopyId);
        bookCheckedOut.PatronId.Should().Be(patronId);
        bookCheckedOut.CheckoutDate.Should().Be(checkoutDate);
        bookCheckedOut.DueDate.Should().Be(dueDate);
        bookCheckedOut.OccurredOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        bookCheckedOut.EventId.Should().NotBeEmpty();
    }

    [Fact]
    public void BookReturned_Event_ShouldHaveCorrectProperties()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var bookCopyId = Guid.NewGuid();
        var patronId = Guid.NewGuid();
        var returnDate = DateTime.UtcNow;

        // Act
        var bookReturned = new BookReturned(loanId, bookCopyId, patronId, returnDate);

        // Assert
        bookReturned.LoanId.Should().Be(loanId);
        bookReturned.BookCopyId.Should().Be(bookCopyId);
        bookReturned.PatronId.Should().Be(patronId);
        bookReturned.ReturnDate.Should().Be(returnDate);
        bookReturned.OccurredOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        bookReturned.EventId.Should().NotBeEmpty();
    }

    [Fact]
    public void BookPlacedOnHold_Event_ShouldHaveCorrectProperties()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var patronId = Guid.NewGuid();
        var holdId = Guid.NewGuid();
        var holdPlacedAt = DateTime.UtcNow;

        // Act
        var bookPlacedOnHold = new BookPlacedOnHold(bookId, patronId, holdId, holdPlacedAt);

        // Assert
        bookPlacedOnHold.BookId.Should().Be(bookId);
        bookPlacedOnHold.PatronId.Should().Be(patronId);
        bookPlacedOnHold.HoldId.Should().Be(holdId);
        bookPlacedOnHold.HoldPlacedAt.Should().Be(holdPlacedAt);
        bookPlacedOnHold.OccurredOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        bookPlacedOnHold.EventId.Should().NotBeEmpty();
    }

    [Fact]
    public void PatronRegistered_Event_ShouldHaveCorrectProperties()
    {
        // Arrange
        var patronId = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");

        // Act
        var patronRegistered = new PatronRegistered(patronId, fullName, address);

        // Assert
        patronRegistered.PatronId.Should().Be(patronId);
        patronRegistered.FullName.Should().Be(fullName);
        patronRegistered.Address.Should().Be(address);
        patronRegistered.OccurredOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        patronRegistered.EventId.Should().NotBeEmpty();
    }

    [Fact]
    public void MembershipStatusChanged_Event_ShouldHaveCorrectProperties()
    {
        // Arrange
        var patronId = Guid.NewGuid();
        var previousStatus = MembershipStatus.Active;
        var newStatus = MembershipStatus.Suspended;
        var fineAmount = 5.50m;
        var reason = "Overdue books";

        // Act
        var membershipStatusChanged = new MembershipStatusChanged(patronId, previousStatus, newStatus, fineAmount, reason);

        // Assert
        membershipStatusChanged.PatronId.Should().Be(patronId);
        membershipStatusChanged.PreviousStatus.Should().Be(previousStatus);
        membershipStatusChanged.NewStatus.Should().Be(newStatus);
        membershipStatusChanged.FineAmount.Should().Be(fineAmount);
        membershipStatusChanged.Reason.Should().Be(reason);
        membershipStatusChanged.OccurredOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        membershipStatusChanged.EventId.Should().NotBeEmpty();
    }
}