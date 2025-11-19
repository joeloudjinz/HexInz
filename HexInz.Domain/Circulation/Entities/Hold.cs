using HexInz.Core.Domain.Circulation.ValueObjects;

namespace HexInz.Core.Domain.Circulation.Entities;

public class Hold(Guid id, PatronId patronId, DateTime placedAt)
{
    public Guid Id { get; private set; } = id;
    public PatronId PatronId { get; private set; } = patronId;
    public DateTime PlacedAt { get; private set; } = placedAt;
    public DateTime? FulfilledAt { get; private set; } = null;
    public DateTime? CancelledAt { get; private set; } = null;
    public bool IsFulfilled => FulfilledAt.HasValue;
    public bool IsCancelled => CancelledAt.HasValue;
    public bool IsActive => !IsFulfilled && !IsCancelled;

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