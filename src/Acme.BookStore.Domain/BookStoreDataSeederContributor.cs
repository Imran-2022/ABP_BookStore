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

    public async Task SeedAsync(DataSeedContext context)
    {
        // --- Books & Authors seeding ---
        if (await _bookRepository.GetCountAsync() == 0)
        {
            var orwell = await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "George Orwell",
                    new DateTime(1903, 06, 25),
                    "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."
                )
            );

            var douglas = await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "Douglas Adams",
                    new DateTime(1952, 03, 11),
                    "Douglas Adams was an English author, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
                )
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    AuthorId = orwell.Id,
                    Name = "1984",
                    Type = BookType.Dystopia,
                    PublishDate = new DateTime(1949, 6, 8),
                    Price = 19.84f
                },
                autoSave: true
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    AuthorId = douglas.Id,
                    Name = "The Hitchhiker's Guide to the Galaxy",
                    Type = BookType.ScienceFiction,
                    PublishDate = new DateTime(1995, 9, 27),
                    Price = 42.0f
                },
                autoSave: true
            );
        }

        // --- Publications seeding ---
        if (await _publicationRepository.GetCountAsync() == 0)
        {
            await _publicationRepository.InsertAsync(
                await _publicationManager.CreateAsync(
                    "Amin Publication",
                    "Dinajpur, Bangladesh",
                    "https://www.aminpublication.com"
                ),
                autoSave: true
            );

            await _publicationRepository.InsertAsync(
                await _publicationManager.CreateAsync(
                    "Sunrise Publications",
                    "Dhaka, Bangladesh",
                    "https://www.sunrisepub.com"
                ),
                autoSave: true
            );
        }
    }
}
