using HexInz.Core.Domain.Common;

namespace HexInz.Core.Domain.Circulation.Events;

public record BookCheckedOut(
    Guid LoanId,
    Guid BookCopyId,
    Guid PatronId,
    DateTime CheckoutDate,
    DateTime DueDate
) : DomainEvent;

public record BookReturned(
    Guid LoanId,
    Guid BookCopyId,
    Guid PatronId,
    DateTime ReturnDate
) : DomainEvent;

public record BookPlacedOnHold(
    Guid BookId,
    Guid PatronId,
    Guid HoldId,
    DateTime HoldPlacedAt
) : DomainEvent;