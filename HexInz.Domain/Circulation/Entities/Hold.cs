using HexInz.Core.Domain.Circulation.ValueObjects;

namespace HexInz.Core.Domain.Circulation.Entities;

public class Hold
{
    public Guid Id { get; private set; }
    public PatronId PatronId { get; private set; }
    public DateTime PlacedAt { get; private set; }
    public DateTime? FulfilledAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public bool IsFulfilled => FulfilledAt.HasValue;
    public bool IsCancelled => CancelledAt.HasValue;
    public bool IsActive => !IsFulfilled && !IsCancelled;

    public Hold(Guid id, PatronId patronId, DateTime placedAt)
    {
        Id = id;
        PatronId = patronId;
        PlacedAt = placedAt;
        FulfilledAt = null;
        CancelledAt = null;
    }

    public void Fulfill()
    {
        if (IsFulfilled || IsCancelled) throw new InvalidOperationException("Cannot fulfill a hold that is already fulfilled or cancelled");

        FulfilledAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (IsFulfilled || IsCancelled) throw new InvalidOperationException("Cannot cancel a hold that is already fulfilled or cancelled");

        CancelledAt = DateTime.UtcNow;
    }
}