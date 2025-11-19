# HexInz.Domain - Domain Layer

This project contains the domain layer for the HexInz library management system, implementing Domain-Driven Design (DDD) principles. The domain layer encapsulates the business logic, rules, and concepts of the library system.

## Table of Contents
- [Architecture Overview](#architecture-overview)
- [Bounded Contexts](#bounded-contexts)
- [Inventory Bounded Context](#inventory-bounded-context)
- [Memberships Bounded Context](#memberships-bounded-context)
- [Circulation Bounded Context](#circulation-bounded-context)
- [Common Components](#common-components)
- [Domain Patterns Used](#domain-patterns-used)

## Architecture Overview

The domain layer is organized using a strategic domain-driven design approach with three main bounded contexts:

1. **Inventory** - Manages book catalog information
2. **Memberships** - Manages patron information and membership status
3. **Circulation** - Manages the lending and returning of books

### Context Map
- **Inventory** is Upstream of Circulation
- **Memberships** is Upstream of Circulation
- **Circulation** is Downstream and uses an Anti-Corruption Layer (ACL) to translate models from upstream contexts

## Bounded Contexts

### Inventory Bounded Context

The Inventory context is the authority for what books the library owns as intellectual works, focusing on cataloging of books.

#### Entities
- **Book** - The aggregate root representing a unique literary work with consistency boundaries for all information about a single catalog entry.

#### Value Objects
- **ISBN** - A unique, validated International Standard Book Number with format validation for both ISBN-10 and ISBN-13.
- **Author** - Contains the first and last name of the author with proper validation.
- **PublicationDetails** - Contains the publisher and publication year.

#### Repositories
- **IBookRepository** - Interface for persisting and retrieving Book aggregates with methods like `GetByIsbnAsync()`.

### Memberships Bounded Context

The Memberships context manages all aspects of a library patron and is the authority on patrons and their membership status.

#### Entities
- **Patron** - The aggregate root representing a library member that enforces invariants like "a patron cannot have an expired card" or "a patron with overdue fines is suspended".

#### Value Objects
- **FullName** - Contains the patron's first and last name.
- **Address** - The patron's physical address with street, city, state, postal code, and country.
- **MembershipStatus** - Enum representing the state (Active, Suspended, Expired).

#### Domain Events
- **PatronRegistered** - Fired when a new Patron aggregate is created.
- **MembershipStatusChanged** - Fired when a patron's status changes (Active/Suspended).

#### Repositories
- **IPatronRepository** - Interface for persisting and retrieving Patron aggregates.

### Circulation Bounded Context

The Circulation context is the most behavior-rich context, managing the interaction between books and patrons. It's downstream and uses an Anti-Corruption Layer to interact with upstream contexts.

#### Entities
- **Loan** - The aggregate root representing the loan of a specific BookCopy to a specific Patron with transactional boundaries for checking out and returning books.
- **HoldableBook** - The aggregate root representing a book from the catalog for which patrons can place holds, managing the queue of holds and enforcing hold rules.
  - **Hold** - Nested entity representing a single patron's place in the queue.

#### Value Objects
- **PatronId** - Reference to a Patron from the Memberships context.
- **BookCopyId** - Reference to a specific physical copy of a book.
- **BookId** - Reference to a Book from the Inventory context.
- **DueDate** - The date the book is due to be returned with validation to ensure it's in the future.

#### Domain Events
- **BookCheckedOut** - Fired when a Loan is created.
- **BookReturned** - Fired when a Loan is completed.
- **BookPlacedOnHold** - Fired when a PatronId is successfully added to the HoldableBook queue.

#### Domain Services
- **OverdueFineService** - Calculates fines based on complex rules (book type, patron type, duration) that don't naturally fit within the Loan aggregate.

#### Repositories
- **ILoanRepository** - Interface for managing the persistence of Loan aggregates.
- **IHoldableBookRepository** - Interface for managing the persistence of HoldableBook aggregates.

## Common Components

The domain layer includes common components that are shared across bounded contexts:

- **DomainEvent** - Base class for all domain events, providing common functionality like event ID and occurrence timestamp.

## Domain Patterns Used

This domain layer implements several Domain-Driven Design patterns:

- **Aggregate Root** - Ensures consistency and encapsulates invariants within each aggregate boundary.
- **Value Object** - Immutable objects that are defined by their attributes rather than identity.
- **Domain Event** - Captures things that happened in the domain that domain experts care about.
- **Domain Service** - Encapsulates domain logic that doesn't naturally fit within an Entity or Value Object.
- **Repository** - Abstraction for persisting and retrieving aggregates from storage.
- **Anti-Corruption Layer** - Protects the Circulation context from changes in upstream contexts by translating their models.

## Key Business Rules

### Inventory Context
- Books must have valid ISBNs (either ISBN-10 or ISBN-13 format)
- Authors must have valid first and last names
- Publication years must be valid

### Memberships Context
- A patron cannot borrow books if their membership is suspended or expired
- A patron with outstanding fines cannot borrow books
- Membership status changes trigger domain events

### Circulation Context
- A patron can only have one hold per book
- Books must be returned by their due date
- Overdue fines are calculated based on days overdue
- Hold queues are processed in first-come, first-served order

## Design Decisions

1. **Context Isolation**: Each bounded context is isolated with clear boundaries and minimal dependencies between contexts.
2. **Event-Driven Communication**: Domain events are used for communication between contexts when needed.
3. **Anti-Corruption Layer**: The Circulation context protects itself from upstream changes by using simplified local models.
4. **Rich Domain Model**: Business logic is encapsulated within the domain objects rather than in anemic models.
5. **Immutability**: Value objects are immutable to ensure consistency and prevent invalid state.
6. **Validation**: Proper validation is implemented at the domain level to maintain data integrity.

This domain layer provides a solid foundation for the HexInz library management system, with clear separation of concerns and adherence to Domain-Driven Design principles.