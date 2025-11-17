using HexInz.Core.Domain.Circulation.Events;
using HexInz.Core.Domain.Circulation.ValueObjects;
using HexInz.Core.Domain.Common;

namespace HexInz.Core.Domain.Circulation.Entities;

public class Loan
{
    public Guid Id { get; private set; }
    public BookCopyId BookCopyId { get; private set; }
    public PatronId PatronId { get; private set; }
    public DateTime CheckoutDate { get; private set; }
    public DueDate DueDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }
    public bool IsReturned => ReturnDate.HasValue;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<DomainEvent> _domainEvents = [];
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public Loan(Guid id, BookCopyId bookCopyId, PatronId patronId, DueDate dueDate)
    {
        Id = id;
        BookCopyId = bookCopyId;
        PatronId = patronId;
        CheckoutDate = DateTime.UtcNow;
        DueDate = dueDate;
        ReturnDate = null;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new BookCheckedOut(Id, bookCopyId.Value, patronId.Value, CheckoutDate, dueDate.Value));
    }

    public void ReturnBook()
    {
        if (IsReturned) throw new InvalidOperationException("Book already returned");

        ReturnDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new BookReturned(Id, BookCopyId.Value, PatronId.Value, ReturnDate.Value));
    }

    public bool IsOverdue()
    {
        return !IsReturned && DueDate.Value < DateTime.UtcNow;
    }

    private void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}