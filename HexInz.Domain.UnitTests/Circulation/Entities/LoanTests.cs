using FluentAssertions;
using HexInz.Core.Domain.Circulation.Entities;
using HexInz.Core.Domain.Circulation.ValueObjects;

namespace HexInz.Domain.UnitTests.Circulation.Entities;

public class LoanTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldSetProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddDays(14));

        // Act
        var loan = new Loan(id, bookCopyId, patronId, dueDate);

        // Assert
        loan.Id.Should().Be(id);
        loan.BookCopyId.Should().Be(bookCopyId);
        loan.PatronId.Should().Be(patronId);
        loan.DueDate.Should().Be(dueDate);
        loan.CheckoutDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        loan.ReturnDate.Should().BeNull();
        loan.IsReturned.Should().BeFalse();
        loan.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        loan.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void ReturnBook_WhenBookNotReturned_ShouldUpdateReturnDate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddDays(14));
        var loan = new Loan(id, bookCopyId, patronId, dueDate);

        // Act
        loan.ReturnBook();

        // Assert
        loan.ReturnDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        loan.IsReturned.Should().BeTrue();
        loan.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ReturnBook_WhenBookAlreadyReturned_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddDays(14));
        var loan = new Loan(id, bookCopyId, patronId, dueDate);
        
        // First return
        loan.ReturnBook();

        // Act
        var action = () => loan.ReturnBook();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Book already returned");
    }

    [Fact]
    public void IsOverdue_WithFutureDueDateAndNotReturned_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddDays(7)); // Future date
        var loan = new Loan(id, bookCopyId, patronId, dueDate);

        // Act
        var result = loan.IsOverdue();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsOverdue_WithDueDateInPastAfterCreation_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddMilliseconds(100)); // Due date in 100ms (practically past after some time)
        var loan = new Loan(id, bookCopyId, patronId, dueDate);

        // Wait a bit to ensure the due date has passed
        Thread.Sleep(200);

        // Act
        var result = loan.IsOverdue();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsOverdue_WithPastDueDateAndBookReturned_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddMilliseconds(100)); // Due date in 100ms
        var loan = new Loan(id, bookCopyId, patronId, dueDate);

        // Wait a bit to ensure the due date has passed
        Thread.Sleep(200);

        // Return the book
        loan.ReturnBook();

        // Act
        var result = loan.IsOverdue();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsOverdue_WithDueDateEqualToCurrentDateAndNotReturned_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddDays(1)); // Tomorrow
        var loan = new Loan(id, bookCopyId, patronId, dueDate);

        // Act
        var result = loan.IsOverdue();

        // Assert
        result.Should().BeFalse();
    }
}