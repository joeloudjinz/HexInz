using HexInz.Core.Domain.Common;
using HexInz.Core.Domain.Memberships.ValueObjects;

namespace HexInz.Core.Domain.Memberships.Events;

public record PatronRegistered(
    Guid PatronId,
    FullName FullName,
    Address Address
) : DomainEvent;

public record MembershipStatusChanged(
    Guid PatronId,
    MembershipStatus PreviousStatus,
    MembershipStatus NewStatus,
    decimal FineAmount,
    string? Reason
) : DomainEvent;