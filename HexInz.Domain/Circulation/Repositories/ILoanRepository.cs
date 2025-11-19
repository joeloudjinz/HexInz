using HexInz.Core.Domain.Circulation.Entities;

namespace HexInz.Core.Domain.Circulation.Repositories;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(Guid id);
    Task<IEnumerable<Loan>> GetByPatronIdAsync(Guid patronId);
    Task<IEnumerable<Loan>> GetOverdueLoansAsync();
    Task<IEnumerable<Loan>> GetAllAsync();
    Task AddAsync(Loan loan);
    Task UpdateAsync(Loan loan);
    Task DeleteAsync(Guid id);
}