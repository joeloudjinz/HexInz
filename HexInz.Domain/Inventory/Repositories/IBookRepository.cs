using HexInz.Core.Domain.Inventory.Entities;
using HexInz.Core.Domain.Inventory.ValueObjects;

namespace HexInz.Core.Domain.Inventory.Repositories;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id);
    Task<Book?> GetByIsbnAsync(ISBN isbn);
    Task<IEnumerable<Book>> GetAllAsync();
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Guid id);
}