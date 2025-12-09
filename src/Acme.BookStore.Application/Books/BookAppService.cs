using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Acme.BookStore.Permissions;
using Acme.BookStore.Publications;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Books;

[Authorize(BookStorePermissions.Books.Default)]
public class BookAppService :
    CrudAppService<
        Book, //The Book entity
        BookDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBookDto>, //Used to create/update a book
    IBookAppService //implement the IBookAppService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IPublicationRepository _publicationRepository;

    public BookAppService(
        IRepository<Book, Guid> repository,
        IAuthorRepository authorRepository,
        IPublicationRepository publicationRepository)
        : base(repository)
    {
        _authorRepository = authorRepository;
        _publicationRepository = publicationRepository;
        GetPolicyName = BookStorePermissions.Books.Default;
        GetListPolicyName = BookStorePermissions.Books.Default;
        CreatePolicyName = BookStorePermissions.Books.Create;
        UpdatePolicyName = BookStorePermissions.Books.Edit;
        DeletePolicyName = BookStorePermissions.Books.Delete;
    }

    public override async Task<BookDto> GetAsync(Guid id)
    {
        var queryable = await Repository.GetQueryableAsync();

        var query = from book in queryable
                    join author in await _authorRepository.GetQueryableAsync()
                        on book.AuthorId equals author.Id
                    join publication in await _publicationRepository.GetQueryableAsync()
                        on book.PublicationId equals publication.Id
                    where book.Id == id
                    select new { book, author, publication };

        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);

        if (queryResult == null)
            throw new EntityNotFoundException(typeof(Book), id);

        var bookDto = ObjectMapper.Map<Book, BookDto>(queryResult.book);

        bookDto.AuthorName = queryResult.author.Name;
        bookDto.PublicationName = queryResult.publication.Name;

        return bookDto;
    }


    public override async Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Book> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join books and authors
        var query = from book in queryable
                    join author in await _authorRepository.GetQueryableAsync()
                        on book.AuthorId equals author.Id
                    join publication in await _publicationRepository.GetQueryableAsync()
                        on book.PublicationId equals publication.Id
                    select new { book, author, publication };

        var queryResult = await AsyncExecuter.ToListAsync(query);

        var bookDtos = queryResult.Select(x =>
        {
            var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
            bookDto.AuthorName = x.author.Name;           // Author name
            bookDto.PublicationName = x.publication.Name; // Publication name
            bookDto.PublicationId = x.publication.Id;     // Publication ID
            return bookDto;
        }).ToList();


        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<BookDto>(
            totalCount,
            bookDtos
        );
    }

    public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
    {
        var authors = await _authorRepository.GetListAsync();

        return new ListResultDto<AuthorLookupDto>(
            ObjectMapper.Map<List<Author>, List<AuthorLookupDto>>(authors)
        );
    }

    // --- Publication Lookup (new)
    // public async Task<ListResultDto<PublicationLookupDto>> GetPublicationLookupAsync()
    // {
    //     var publications = await _publicationRepository.GetListAsync();

    //     var lookup = publications.Select(p => new PublicationLookupDto
    //     {
    //         Id = p.Id,
    //         Name = p.Name
    //     }).ToList();

    //     return new ListResultDto<PublicationLookupDto>(lookup);
    // }
    // --- Publication Lookup
    public async Task<ListResultDto<PublicationLookupDto>> GetPublicationLookupAsync()
    {
        var publications = await _publicationRepository.GetListAsync();

        var lookup = ObjectMapper.Map<List<Publication>, List<PublicationLookupDto>>(publications);

        return new ListResultDto<PublicationLookupDto>(lookup);
    }
    private static string NormalizeSorting(string sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"book.{nameof(Book.Name)}";
        }

        if (sorting.Contains("authorName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "authorName",
                "author.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        if (sorting.Contains("publicationName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "publicationName",
                "publication.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"book.{sorting}";
    }

}
