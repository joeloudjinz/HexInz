using FluentAssertions;
using HexInz.Core.Domain.Common;

namespace HexInz.Domain.UnitTests.Common;

public class DomainEventTests
{
    [Fact]
    public void Constructor_ShouldSetEventIdAndOccurredOn()
    {
        // Act
        var domainEvent = new TestDomainEvent();

        // Assert
        domainEvent.EventId.Should().NotBeEmpty();
        domainEvent.OccurredOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}

// Create a test implementation of DomainEvent since it has a protected constructor
public record TestDomainEvent : DomainEvent
{
    public TestDomainEvent() : base() { }
}