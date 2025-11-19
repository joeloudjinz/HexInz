using FluentAssertions;
using HexInz.Core.Domain.Inventory.Entities;
using HexInz.Core.Domain.Inventory.ValueObjects;

namespace HexInz.Domain.UnitTests.Inventory.Entities;

public class BookTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldSetProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var isbn = new ISBN("0306406152");
        var title = "Sample Book";
        var author = new Author("John", "Doe");
        var publicationDetails = new PublicationDetails("Publisher", 2020);

        // Act
        var book = new Book(id, isbn, title, author, publicationDetails);

        // Assert
        book.Id.Should().Be(id);
        book.ISBN.Should().Be(isbn);
        book.Title.Should().Be(title);
        book.Author.Should().Be(author);
        book.PublicationDetails.Should().Be(publicationDetails);
        book.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        book.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void UpdateBook_WithValidParameters_ShouldUpdateProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var originalIsbn = new ISBN("0306406152");
        var originalTitle = "Original Title";
        var originalAuthor = new Author("Original", "Author");
        var originalPublicationDetails = new PublicationDetails("Original Publisher", 2020);
        var book = new Book(id, originalIsbn, originalTitle, originalAuthor, originalPublicationDetails);

        var newTitle = "Updated Title";
        var newAuthor = new Author("Updated", "Author");
        var newPublicationDetails = new PublicationDetails("Updated Publisher", 2021);

        // Act
        book.UpdateBook(newTitle, newAuthor, newPublicationDetails);

        // Assert
        book.Title.Should().Be(newTitle);
        book.Author.Should().Be(newAuthor);
        book.PublicationDetails.Should().Be(newPublicationDetails);
        book.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UpdateBook_WithValidParameters_ShouldUpdateUpdatedAt()
    {
        // Arrange
        var id = Guid.NewGuid();
        var isbn = new ISBN("0306406152");
        var originalTitle = "Original Title";
        var originalAuthor = new Author("Original", "Author");
        var originalPublicationDetails = new PublicationDetails("Original Publisher", 2020);
        var book = new Book(id, isbn, originalTitle, originalAuthor, originalPublicationDetails);

        // Act
        book.UpdateBook("Updated Title", new Author("Updated", "Author"), new PublicationDetails("Updated Publisher", 2021));

        // Assert
        book.UpdatedAt.Should().NotBeNull();
        book.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}