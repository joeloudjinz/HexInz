using HexInz.Core.Domain.Inventory.ValueObjects;

namespace HexInz.Core.Domain.Inventory.Entities;

public class Book
{
    public Guid Id { get; private set; }
    public ISBN ISBN { get; private set; }
    public string Title { get; private set; }
    public Author Author { get; private set; }
    public PublicationDetails PublicationDetails { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Book(Guid id, ISBN isbn, string title, Author author, PublicationDetails publicationDetails)
    {
        Id = id;
        ISBN = isbn;
        Title = title;
        Author = author;
        PublicationDetails = publicationDetails;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateBook(string title, Author author, PublicationDetails publicationDetails)
    {
        Title = title;
        Author = author;
        PublicationDetails = publicationDetails;
        UpdatedAt = DateTime.UtcNow;
    }
}