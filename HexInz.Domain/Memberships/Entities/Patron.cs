using HexInz.Core.Domain.Common;
using HexInz.Core.Domain.Memberships.Events;
using HexInz.Core.Domain.Memberships.ValueObjects;

namespace HexInz.Core.Domain.Memberships.Entities;

public class Patron
{
    public Guid Id { get; private set; }
    public FullName FullName { get; private set; }
    public Address Address { get; private set; }
    public MembershipStatus Status { get; private set; }
    public decimal OutstandingFines { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<DomainEvent> _domainEvents = [];
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public Patron(Guid id, FullName fullName, Address address)
    {
        Id = id;
        FullName = fullName;
        Address = address;
        Status = MembershipStatus.Active;
        OutstandingFines = 0m;
        CreatedAt = DateTime.UtcNow;
            
        AddDomainEvent(new PatronRegistered(Id, fullName, address));
    }

    public void UpdateAddress(Address address)
    {
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SuspendMembership(decimal fineAmount = 0m, string? reason = null)
    {
        var previousStatus = Status;
        Status = MembershipStatus.Suspended;
        OutstandingFines += fineAmount;
        UpdatedAt = DateTime.UtcNow;

        if (previousStatus != MembershipStatus.Suspended)
        {
            AddDomainEvent(new MembershipStatusChanged(Id, previousStatus, MembershipStatus.Suspended, fineAmount, reason));
        }
    }

    public void ActivateMembership()
    {
        var previousStatus = Status;
        Status = MembershipStatus.Active;
        UpdatedAt = DateTime.UtcNow;

        if (previousStatus != MembershipStatus.Active)
        {
            AddDomainEvent(new MembershipStatusChanged(Id, previousStatus, MembershipStatus.Active, 0m, null));
        }
    }

    public void ExpireMembership()
    {
        var previousStatus = Status;
        Status = MembershipStatus.Expired;
        UpdatedAt = DateTime.UtcNow;

        if (previousStatus != MembershipStatus.Expired)
        {
            AddDomainEvent(new MembershipStatusChanged(Id, previousStatus, MembershipStatus.Expired, 0m, null));
        }
    }

    public bool CanBorrow()
    {
        return Status == MembershipStatus.Active && OutstandingFines <= 0;
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