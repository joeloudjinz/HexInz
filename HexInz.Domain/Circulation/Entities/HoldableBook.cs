using HexInz.Core.Domain.Circulation.Events;
using HexInz.Core.Domain.Circulation.ValueObjects;
using HexInz.Core.Domain.Common;

namespace HexInz.Core.Domain.Circulation.Entities;

public class HoldableBook(BookId bookId)
{
    public BookId BookId { get; private set; } = bookId;
    public List<Hold> Holds { get; private set; } = [];
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<DomainEvent> _domainEvents = [];
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void PlaceHold(PatronId patronId)
    {
        // Check if the patron already has a hold on this book
        if (Holds.Any(h => h.PatronId.Value == patronId.Value && !h.IsFulfilled))
        {
            throw new InvalidOperationException($"Patron {patronId.Value} already has a hold on this book");
        }

        var hold = new Hold(Guid.NewGuid(), patronId, DateTime.UtcNow);
        Holds.Add(hold);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new BookPlacedOnHold(BookId.Value, patronId.Value, hold.Id, hold.PlacedAt));
    }

    public void FulfillNextHold()
    {
        var nextHold = Holds
            .Where(h => !h.IsFulfilled)
            .OrderBy(h => h.PlacedAt)
            .FirstOrDefault();

        if (nextHold == null) return;
        nextHold.Fulfill();
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveHold(Guid holdId)
    {
        var hold = Holds.FirstOrDefault(h => h.Id == holdId && !h.IsFulfilled);
        if (hold == null) return;
        hold.Cancel();
        UpdatedAt = DateTime.UtcNow;
    }

    public int GetHoldQueuePosition(PatronId patronId)
    {
        var hold = Holds.FirstOrDefault(h => h.PatronId.Value == patronId.Value && !h.IsFulfilled);
        if (hold == null) return -1; // Patron doesn't have a hold

        return Holds
            .Where(h => !h.IsFulfilled)
            .OrderBy(h => h.PlacedAt)
            .ToList()
            .FindIndex(h => h.Id == hold.Id) + 1;
    }

    public List<Hold> GetActiveHolds()
    {
        return Holds.Where(h => !h.IsFulfilled).OrderBy(h => h.PlacedAt).ToList();
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