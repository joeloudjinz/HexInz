using HexInz.Core.Domain.Inventory.ValueObjects;

namespace HexInz.Core.Domain.Inventory.Entities;

public class Book(Guid id, ISBN isbn, string title, Author author, PublicationDetails publicationDetails)
{
    public Guid Id { get; private set; } = id;
    public ISBN ISBN { get; private set; } = isbn;
    public string Title { get; private set; } = title;
    public Author Author { get; private set; } = author;
    public PublicationDetails PublicationDetails { get; private set; } = publicationDetails;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public void UpdateBook(string title, Author author, PublicationDetails publicationDetails)
    {
        Title = title;
        Author = author;
        PublicationDetails = publicationDetails;
        UpdatedAt = DateTime.UtcNow;
    }
}