using FluentAssertions;
using HexInz.Core.Domain.Memberships.ValueObjects;

namespace HexInz.Domain.UnitTests.Memberships.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldNotThrow()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        var state = "State";
        var postalCode = "12345";
        var country = "Country";

        // Act
        var action = () => new Address(street, city, state, postalCode, country);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldSetProperties()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        var state = "State"; 
        var postalCode = "12345";
        var country = "Country";

        // Act
        var address = new Address(street, city, state, postalCode, country);

        // Assert
        address.Street.Should().Be(street);
        address.City.Should().Be(city);
        address.State.Should().Be(state);
        address.PostalCode.Should().Be(postalCode);
        address.Country.Should().Be(country);
    }

    [Fact]
    public void Constructor_WithWhitespaceParameters_ShouldTrim()
    {
        // Arrange
        var street = "  123 Main St  ";
        var city = "  City  ";
        var state = "  State  ";
        var postalCode = "  12345  ";
        var country = "  Country  ";

        // Act
        var address = new Address(street, city, state, postalCode, country);

        // Assert
        address.Street.Should().Be("123 Main St");
        address.City.Should().Be("City");
        address.State.Should().Be("State");
        address.PostalCode.Should().Be("12345");
        address.Country.Should().Be("Country");
    }

    [Fact]
    public void Constructor_WithEmptyStreet_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "";
        var city = "City";
        var state = "State";
        var postalCode = "12345";
        var country = "Country";

        // Act
        var action = () => new Address(street, city, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Street cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithNullStreet_ShouldThrowArgumentException()
    {
        // Arrange
        string? street = null;
        var city = "City";
        var state = "State";
        var postalCode = "12345";
        var country = "Country";

        // Act
        var action = () => new Address(street!, city, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Street cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithWhitespaceOnlyStreet_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "   ";
        var city = "City";
        var state = "State";
        var postalCode = "12345";
        var country = "Country";

        // Act
        var action = () => new Address(street, city, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Street cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithEmptyCity_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        var city = "";
        var state = "State";
        var postalCode = "12345";
        var country = "Country";

        // Act
        var action = () => new Address(street, city, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*City cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithNullCity_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        string? city = null;
        var state = "State";
        var postalCode = "12345";
        var country = "Country";

        // Act
        var action = () => new Address(street, city!, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*City cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithWhitespaceOnlyCity_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        var city = "   ";
        var state = "State";
        var postalCode = "12345";
        var country = "Country";

        // Act
        var action = () => new Address(street, city, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*City cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithEmptyState_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        var state = "";
        var postalCode = "12345";
        var country = "Country";

        // Act
        var action = () => new Address(street, city, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*State cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithNullState_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        string? state = null;
        var postalCode = "12345";
        var country = "Country";

        // Act
        var action = () => new Address(street, city, state!, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*State cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithWhitespaceOnlyState_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        var state = "   ";
        var postalCode = "12345";
        var country = "Country";

        // Act
        var action = () => new Address(street, city, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*State cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithEmptyPostalCode_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        var state = "State";
        var postalCode = "";
        var country = "Country";

        // Act
        var action = () => new Address(street, city, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Postal code cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithNullPostalCode_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        var state = "State";
        string? postalCode = null;
        var country = "Country";

        // Act
        var action = () => new Address(street, city, state, postalCode!, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Postal code cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithWhitespaceOnlyPostalCode_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        var state = "State";
        var postalCode = "   ";
        var country = "Country";

        // Act
        var action = () => new Address(street, city, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Postal code cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithEmptyCountry_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        var state = "State";
        var postalCode = "12345";
        var country = "";

        // Act
        var action = () => new Address(street, city, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Country cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithNullCountry_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        var state = "State";
        var postalCode = "12345";
        string? country = null;

        // Act
        var action = () => new Address(street, city, state, postalCode, country!);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Country cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithWhitespaceOnlyCountry_ShouldThrowArgumentException()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        var state = "State";
        var postalCode = "12345";
        var country = "   ";

        // Act
        var action = () => new Address(street, city, state, postalCode, country);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Country cannot be null or empty*");
    }

    [Fact]
    public void Equals_WithSameAddressValues_ShouldReturnTrue()
    {
        // Arrange
        var address1 = new Address("123 Main St", "City", "State", "12345", "Country");
        var address2 = new Address("123 Main St", "City", "State", "12345", "Country");

        // Act
        var result = address1.Equals(address2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentStreets_ShouldReturnFalse()
    {
        // Arrange
        var address1 = new Address("123 Main St", "City", "State", "12345", "Country");
        var address2 = new Address("456 Oak Ave", "City", "State", "12345", "Country");

        // Act
        var result = address1.Equals(address2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentCities_ShouldReturnFalse()
    {
        // Arrange
        var address1 = new Address("123 Main St", "City", "State", "12345", "Country");
        var address2 = new Address("123 Main St", "OtherCity", "State", "12345", "Country");

        // Act
        var result = address1.Equals(address2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentStates_ShouldReturnFalse()
    {
        // Arrange
        var address1 = new Address("123 Main St", "City", "State", "12345", "Country");
        var address2 = new Address("123 Main St", "City", "OtherState", "12345", "Country");

        // Act
        var result = address1.Equals(address2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentPostalCodes_ShouldReturnFalse()
    {
        // Arrange
        var address1 = new Address("123 Main St", "City", "State", "12345", "Country");
        var address2 = new Address("123 Main St", "City", "State", "54321", "Country");

        // Act
        var result = address1.Equals(address2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentCountries_ShouldReturnFalse()
    {
        // Arrange
        var address1 = new Address("123 Main St", "City", "State", "12345", "Country");
        var address2 = new Address("123 Main St", "City", "State", "12345", "OtherCountry");

        // Act
        var result = address1.Equals(address2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentCaseValues_ShouldReturnTrue()
    {
        // Arrange
        var address1 = new Address("123 main st", "city", "state", "12345", "country");
        var address2 = new Address("123 Main St", "City", "State", "12345", "Country");

        // Act
        var result = address1.Equals(address2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_WithSameAddressValues_ShouldReturnSameHash()
    {
        // Arrange
        var address1 = new Address("123 Main St", "City", "State", "12345", "Country");
        var address2 = new Address("123 Main St", "City", "State", "12345", "Country");

        // Act
        var hash1 = address1.GetHashCode();
        var hash2 = address2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedAddress()
    {
        // Arrange
        var street = "123 Main St";
        var city = "City";
        var state = "State";
        var postalCode = "12345";
        var country = "Country";
        var address = new Address(street, city, state, postalCode, country);

        // Act
        var result = address.ToString();

        // Assert
        result.Should().Be("123 Main St, City, State 12345, Country");
    }
}