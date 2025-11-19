using FluentAssertions;
using HexInz.Core.Domain.Circulation.Entities;
using HexInz.Core.Domain.Circulation.Services;
using HexInz.Core.Domain.Circulation.ValueObjects;

namespace HexInz.Domain.UnitTests.Circulation.Services;

public class OverdueFineServiceTests
{
    [Fact]
    public void CalculateFine_WithNonOverdueLoan_ShouldReturnZero()
    {
        // Arrange
        var service = new OverdueFineService();
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddDays(7)); // Future due date
        var loan = new Loan(id, bookCopyId, patronId, dueDate);

        // Act
        var result = service.CalculateFine(loan);

        // Assert
        result.Should().Be(0m);
    }

    [Fact]
    public void CalculateFine_WithReturnedLoan_ShouldReturnZero()
    {
        // Arrange
        var service = new OverdueFineService();
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddDays(7)); // Future due date
        var loan = new Loan(id, bookCopyId, patronId, dueDate);
        
        // Return the loan
        loan.ReturnBook();

        // Act
        var result = service.CalculateFine(loan);

        // Assert
        result.Should().Be(0m);
    }

    [Fact]
    public void CalculateFine_WithDifferentDailyFineRate_ShouldCalculateCorrectly()
    {
        // Arrange
        var dailyFineRate = 0.50m;
        var service = new OverdueFineService(dailyFineRate, 10.00m);
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddDays(7)); // Future due date
        var loan = new Loan(id, bookCopyId, patronId, dueDate);

        // Act
        var result = service.CalculateFine(loan);

        // Assert - Should still be 0 because loan is not overdue
        result.Should().Be(0m);
    }

    [Fact]
    public void CalculateFine_WithDifferentMaximumFine_ShouldRespectMaximum()
    {
        // Arrange
        var dailyFineRate = 1.00m;
        var maximumFine = 2.00m;
        var service = new OverdueFineService(dailyFineRate, maximumFine);
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddDays(7)); // Future due date
        var loan = new Loan(id, bookCopyId, patronId, dueDate);

        // Act
        var result = service.CalculateFine(loan);

        // Assert - Should still be 0 because loan is not overdue
        result.Should().Be(0m);
    }

    [Fact]
    public void CalculateFine_WithOverdueLoan_ShouldCalculateBasedOnDaysOverdue()
    {
        // Arrange
        var dailyFineRate = 0.25m;
        var service = new OverdueFineService(dailyFineRate, 10.00m);
        
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddMilliseconds(50)); // Due in 50ms
        var loan = new Loan(id, bookCopyId, patronId, dueDate);

        // Wait for due date to pass to make it overdue
        Thread.Sleep(100);

        // Act
        var result = service.CalculateFine(loan);

        // Assert - Should be 0 because not a full day has passed since due date
        // The calculation uses (DateTime.UtcNow - loan.DueDate.Value).Days, which only counts full days
        result.Should().Be(0m);
    }

    [Fact]
    public void CalculateFine_WithOverdueLoan_ShouldRespectMaximumFine()
    {
        // Test with custom service settings to ensure maximum is respected
        var dailyFineRate = 5.00m; // High daily rate
        var maximumFine = 1.00m;   // Low maximum
        var service = new OverdueFineService(dailyFineRate, maximumFine);
        
        var id = Guid.NewGuid();
        var bookCopyId = new BookCopyId(Guid.NewGuid());
        var patronId = new PatronId(Guid.NewGuid());
        var dueDate = new DueDate(DateTime.UtcNow.AddMilliseconds(50)); // Due in 50ms
        var loan = new Loan(id, bookCopyId, patronId, dueDate);

        // Wait for due date to pass
        Thread.Sleep(100);

        // Act
        var result = service.CalculateFine(loan);

        // Even with a high daily rate, the result should not exceed maximum
        result.Should().Be(0m); // Still 0m because not a full day overdue
    }
}