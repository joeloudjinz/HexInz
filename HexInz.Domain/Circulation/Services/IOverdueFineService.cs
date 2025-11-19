using HexInz.Core.Domain.Circulation.Entities;

namespace HexInz.Core.Domain.Circulation.Services;

public interface IOverdueFineService
{
    decimal CalculateFine(Loan loan);
}