using FluentAssertions;
using HexInz.Core.Domain.Memberships.Entities;
using HexInz.Core.Domain.Memberships.ValueObjects;

namespace HexInz.Domain.UnitTests.Memberships.Entities;

public class PatronTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldSetProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");

        // Act
        var patron = new Patron(id, fullName, address);

        // Assert
        patron.Id.Should().Be(id);
        patron.FullName.Should().Be(fullName);
        patron.Address.Should().Be(address);
        patron.Status.Should().Be(MembershipStatus.Active);
        patron.OutstandingFines.Should().Be(0m);
        patron.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        patron.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldSetInitialStatusToActive()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");

        // Act
        var patron = new Patron(id, fullName, address);

        // Assert
        patron.Status.Should().Be(MembershipStatus.Active);
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldSetInitialOutstandingFinesToZero()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");

        // Act
        var patron = new Patron(id, fullName, address);

        // Assert
        patron.OutstandingFines.Should().Be(0m);
    }

    [Fact]
    public void UpdateAddress_WithValidAddress_ShouldUpdateAddress()
    {
        // Arrange
        var id = Guid.NewGuid();
        var originalFullName = new FullName("John", "Doe");
        var originalAddress = new Address("123 Main St", "City", "State", "12345", "Country");
        var patron = new Patron(id, originalFullName, originalAddress);

        var newAddress = new Address("456 Oak Ave", "NewCity", "NewState", "54321", "NewCountry");

        // Act
        patron.UpdateAddress(newAddress);

        // Assert
        patron.Address.Should().Be(newAddress);
        patron.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void SuspendMembership_WithoutFineAmount_ShouldUpdateStatusAndFines()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");
        var patron = new Patron(id, fullName, address);

        // Act
        patron.SuspendMembership();

        // Assert
        patron.Status.Should().Be(MembershipStatus.Suspended);
        patron.OutstandingFines.Should().Be(0m);
        patron.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void SuspendMembership_WithFineAmount_ShouldUpdateStatusAndAddToOutstandingFines()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");
        var patron = new Patron(id, fullName, address);

        var fineAmount = 5.50m;

        // Act
        patron.SuspendMembership(fineAmount);

        // Assert
        patron.Status.Should().Be(MembershipStatus.Suspended);
        patron.OutstandingFines.Should().Be(fineAmount);
        patron.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void SuspendMembership_WithExistingFines_ShouldAddToExistingFines()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");
        var patron = new Patron(id, fullName, address);

        // First suspend with a fine
        patron.SuspendMembership(3.00m);
        
        var additionalFine = 2.50m;

        // Act
        patron.SuspendMembership(additionalFine);

        // Assert
        patron.OutstandingFines.Should().Be(5.50m); // 3.00 + 2.50
    }

    [Fact]
    public void ActivateMembership_ShouldUpdateStatusToActive()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");
        var patron = new Patron(id, fullName, address);

        // Suspend first to have a different status
        patron.SuspendMembership();
        
        // Act
        patron.ActivateMembership();

        // Assert
        patron.Status.Should().Be(MembershipStatus.Active);
        patron.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ExpireMembership_ShouldUpdateStatusToExpired()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");
        var patron = new Patron(id, fullName, address);

        // Act
        patron.ExpireMembership();

        // Assert
        patron.Status.Should().Be(MembershipStatus.Expired);
        patron.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CanBorrow_WithActiveStatusAndNoFines_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");
        var patron = new Patron(id, fullName, address);

        // Act
        var result = patron.CanBorrow();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanBorrow_WithSuspendedStatus_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");
        var patron = new Patron(id, fullName, address);

        // Suspend membership
        patron.SuspendMembership();

        // Act
        var result = patron.CanBorrow();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CanBorrow_WithExpiredStatus_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");
        var patron = new Patron(id, fullName, address);

        // Expire membership
        patron.ExpireMembership();

        // Act
        var result = patron.CanBorrow();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CanBorrow_WithActiveStatusButOutstandingFines_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");
        var patron = new Patron(id, fullName, address);

        // Add some fines
        patron.SuspendMembership(5.00m);

        // Activate again but with fines
        patron.ActivateMembership();

        // Act
        var result = patron.CanBorrow();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CanBorrow_WithActiveStatusAndZeroFines_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fullName = new FullName("John", "Doe");
        var address = new Address("123 Main St", "City", "State", "12345", "Country");
        var patron = new Patron(id, fullName, address);

        // Suspend and then activate to set some fines, then clear them
        patron.SuspendMembership(5.00m);
        patron.ActivateMembership(); // This will not clear fines but keep status active

        // Simulate clearing fines manually by setting to zero (this is a test scenario)
        // Actually, in the model, activating membership doesn't clear fines, so let's test
        // with zero fines from suspension
        var patron2 = new Patron(Guid.NewGuid(), fullName, address);
        patron2.SuspendMembership(0m);  // Fine is 0
        patron2.ActivateMembership();

        var result = patron2.CanBorrow();

        // Assert
        result.Should().BeTrue();
    }
}