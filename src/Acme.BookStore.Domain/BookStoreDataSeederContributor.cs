using System;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Publications;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore;

public class BookStoreDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Book, Guid> _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly AuthorManager _authorManager;

    private readonly IPublicationRepository _publicationRepository;
    private readonly PublicationManager _publicationManager;

    public BookStoreDataSeederContributor(
        IRepository<Book, Guid> bookRepository,
        IAuthorRepository authorRepository,
        AuthorManager authorManager,
        IPublicationRepository publicationRepository,
        PublicationManager publicationManager)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _authorManager = authorManager;

        _publicationRepository = publicationRepository;
        _publicationManager = publicationManager;
    }

    // public async Task SeedAsync(DataSeedContext context)
    // {
    //     // 1️⃣ Seed Publications
    //     var amin = await _publicationRepository.FindAsync(x => x.Name == "Amin Publication");
    //     var sunrise = await _publicationRepository.FindAsync(x => x.Name == "Sunrise Publications");

    //     if (amin == null)
    //     {
    //         amin = await _publicationRepository.InsertAsync(
    //             await _publicationManager.CreateAsync(
    //                 "Amin Publication",
    //                 "Dinajpur, Bangladesh",
    //                 "https://www.aminpublication.com"
    //             ),
    //             autoSave: true
    //         );
    //     }

    //     if (sunrise == null)
    //     {
    //         sunrise = await _publicationRepository.InsertAsync(
    //             await _publicationManager.CreateAsync(
    //                 "Sunrise Publications",
    //                 "Dhaka, Bangladesh",
    //                 "https://www.sunrisepub.com"
    //             ),
    //             autoSave: true
    //         );
    //     }

    //     // 2️⃣ Seed Authors
    //     var orwell = await _authorRepository.FindAsync(x => x.Name == "George Orwell");
    //     var douglas = await _authorRepository.FindAsync(x => x.Name == "Douglas Adams");

    //     if (orwell == null)
    //     {
    //         orwell = await _authorRepository.InsertAsync(
    //             await _authorManager.CreateAsync(
    //                 "George Orwell",
    //                 new DateTime(1903, 06, 25),
    //                 "Orwell produced literary criticism..."
    //             )
    //         );
    //     }

    //     if (douglas == null)
    //     {
    //         douglas = await _authorRepository.InsertAsync(
    //             await _authorManager.CreateAsync(
    //                 "Douglas Adams",
    //                 new DateTime(1952, 03, 11),
    //                 "Douglas Adams was an English author..."
    //             )
    //         );
    //     }

    //     // 3️⃣ Seed Books
    //     var book1984 = await _bookRepository.FindAsync(x => x.Name == "1984");
    //     var hitchhiker = await _bookRepository.FindAsync(x => x.Name == "The Hitchhiker's Guide to the Galaxy");

    //     if (book1984 == null)
    //     {
    //         await _bookRepository.InsertAsync(
    //             new Book
    //             {
    //                 AuthorId = orwell.Id,
    //                 PublicationId = amin.Id,
    //                 Name = "1984",
    //                 Type = BookType.Dystopia,
    //                 PublishDate = new DateTime(1949, 6, 8),
    //                 Price = 19.84f
    //             },
    //             autoSave: true
    //         );
    //     }

    //     if (hitchhiker == null)
    //     {
    //         await _bookRepository.InsertAsync(
    //             new Book
    //             {
    //                 AuthorId = douglas.Id,
    //                 PublicationId = sunrise.Id,
    //                 Name = "The Hitchhiker's Guide to the Galaxy",
    //                 Type = BookType.ScienceFiction,
    //                 PublishDate = new DateTime(1995, 9, 27),
    //                 Price = 42.0f
    //             },
    //             autoSave: true
    //         );
    //     }
    // }

    public async Task SeedAsync(DataSeedContext context)
    {
        // Seed Publications
        var amin = await SeedPublicationAsync(
            "Amin Publication",
            "Dinajpur, Bangladesh",
            "https://www.aminpublication.com"
        );

        var sunrise = await SeedPublicationAsync(
            "Sunrise Publications",
            "Dhaka, Bangladesh",
            "https://www.sunrisepub.com"
        );

        // Seed Authors
        var orwell = await SeedAuthorAsync(
            "George Orwell",
            new DateTime(1903, 6, 25),
            "Orwell produced literary criticism..."
        );

        var douglas = await SeedAuthorAsync(
            "Douglas Adams",
            new DateTime(1952, 3, 11),
            "Douglas Adams was an English author..."
        );

        // Seed Books
        await SeedBookAsync(
            "1984",
            orwell.Id,
            amin.Id,
            BookType.Dystopia,
            new DateTime(1949, 6, 8),
            19.84f
        );

        await SeedBookAsync(
            "The Hitchhiker's Guide to the Galaxy",
            douglas.Id,
            sunrise.Id,
            BookType.ScienceFiction,
            new DateTime(1995, 9, 27),
            42.0f
        );
    }
    // SeedPublicationAsync()
    private async Task<Publication> SeedPublicationAsync(string name, string address, string website)
    {
        var existing = await _publicationRepository.FindAsync(x => x.Name == name);
        if (existing != null)
            return existing;

        return await _publicationRepository.InsertAsync(
            await _publicationManager.CreateAsync(name, address, website),
            autoSave: true
        );
    }
    // SeedAuthorAsync()
    private async Task<Author> SeedAuthorAsync(string name, DateTime birthDate, string bio)
    {
        var existing = await _authorRepository.FindAsync(x => x.Name == name);
        if (existing != null)
            return existing;

        return await _authorRepository.InsertAsync(
            await _authorManager.CreateAsync(name, birthDate, bio),
            autoSave: true
        );
    }
    // SeedBookAsync()
    private async Task SeedBookAsync(
        string name,
        Guid authorId,
        Guid publicationId,
        BookType type,
        DateTime publishDate,
        float price)
    {
        var existing = await _bookRepository.FindAsync(x => x.Name == name);
        if (existing != null)
            return;

        await _bookRepository.InsertAsync(
            new Book
            {
                Name = name,
                AuthorId = authorId,
                PublicationId = publicationId,
                Type = type,
                PublishDate = publishDate,
                Price = price
            },
            autoSave: true
        );
    }

}
