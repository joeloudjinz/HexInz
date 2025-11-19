using HexInz.Core.Domain.Memberships.Entities;

namespace HexInz.Core.Domain.Memberships.Repositories;

public interface IPatronRepository
{
    Task<Patron?> GetByIdAsync(Guid id);
    Task<Patron?> GetByFullNameAsync(string firstName, string lastName);
    Task<IEnumerable<Patron>> GetAllAsync();
    Task AddAsync(Patron patron);
    Task UpdateAsync(Patron patron);
    Task DeleteAsync(Guid id);
}