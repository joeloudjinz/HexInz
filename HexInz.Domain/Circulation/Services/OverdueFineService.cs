using HexInz.Core.Domain.Circulation.Entities;

namespace HexInz.Core.Domain.Circulation.Services;

internal class OverdueFineService(decimal dailyFineRate = 0.25m, decimal maximumFine = 10.00m) : IOverdueFineService
{
    public decimal CalculateFine(Loan loan)
    {
        if (!loan.IsOverdue()) return 0m;

        var daysOverdue = (DateTime.UtcNow - loan.DueDate.Value).Days;
        if (daysOverdue <= 0) return 0m;

        var calculatedFine = daysOverdue * dailyFineRate;
        return Math.Min(calculatedFine, maximumFine);
    }
}