using FluentAssertions;
using HexInz.Core.Domain.Memberships.ValueObjects;

namespace HexInz.Domain.UnitTests.Memberships.ValueObjects;

public class MembershipStatusTests
{
    [Fact]
    public void MembershipStatus_ShouldHaveActiveValue()
    {
        // Arrange & Act
        var active = MembershipStatus.Active;

        // Assert
        active.Should().Be(MembershipStatus.Active);
        ((int)active).Should().Be(0);
    }

    [Fact]
    public void MembershipStatus_ShouldHaveSuspendedValue()
    {
        // Arrange & Act
        var suspended = MembershipStatus.Suspended;

        // Assert
        suspended.Should().Be(MembershipStatus.Suspended);
        ((int)suspended).Should().Be(1);
    }

    [Fact]
    public void MembershipStatus_ShouldHaveExpiredValue()
    {
        // Arrange & Act
        var expired = MembershipStatus.Expired;

        // Assert
        expired.Should().Be(MembershipStatus.Expired);
        ((int)expired).Should().Be(2);
    }

    [Fact]
    public void MembershipStatus_Values_ShouldBeSequential()
    {
        // Arrange
        var values = Enum.GetValues<MembershipStatus>().ToList();

        // Act & Assert
        values.Count.Should().Be(3);
        values.Should().Contain(MembershipStatus.Active);
        values.Should().Contain(MembershipStatus.Suspended);
        values.Should().Contain(MembershipStatus.Expired);
        
        // Check that values are sequential starting from 0
        ((int)MembershipStatus.Active).Should().Be(0);
        ((int)MembershipStatus.Suspended).Should().Be(1);
        ((int)MembershipStatus.Expired).Should().Be(2);
    }
}