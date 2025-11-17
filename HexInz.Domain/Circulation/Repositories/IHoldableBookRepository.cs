using HexInz.Core.Domain.Circulation.Entities;
using HexInz.Core.Domain.Circulation.ValueObjects;

namespace HexInz.Core.Domain.Circulation.Repositories;

public interface IHoldableBookRepository
{
    Task<HoldableBook?> GetByBookIdAsync(BookId bookId);
    Task<IEnumerable<HoldableBook>> GetAllAsync();
    Task AddAsync(HoldableBook holdableBook);
    Task UpdateAsync(HoldableBook holdableBook);
    Task DeleteAsync(BookId bookId);
}