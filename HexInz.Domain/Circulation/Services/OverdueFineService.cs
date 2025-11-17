using HexInz.Core.Domain.Circulation.Entities;

namespace HexInz.Core.Domain.Circulation.Services;

public interface IOverdueFineService
{
    decimal CalculateFine(Loan loan);
}

public class OverdueFineService : IOverdueFineService
{
    private readonly decimal _dailyFineRate;
    private readonly decimal _maximumFine;

    public OverdueFineService(decimal dailyFineRate = 0.25m, decimal maximumFine = 10.00m)
    {
        _dailyFineRate = dailyFineRate;
        _maximumFine = maximumFine;
    }

    public decimal CalculateFine(Loan loan)
    {
        if (!loan.IsOverdue()) return 0m;

        var daysOverdue = (DateTime.UtcNow - loan.DueDate.Value).Days;
        if (daysOverdue <= 0) return 0m;

        var calculatedFine = daysOverdue * _dailyFineRate;
        return Math.Min(calculatedFine, _maximumFine);
    }
}