using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Publications;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Acme.BookStore;

/*
 * You can add your own mappings here.
 * [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
 * public partial class BookStoreApplicationMappers : MapperBase<BookDto, CreateUpdateBookDto>
 * {
 *    public override partial CreateUpdateBookDto Map(BookDto source);
 * 
 *    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
 * }
 */
[Mapper]
public partial class BookToBookDtoMapper : MapperBase<Book, BookDto>
{
    public override partial BookDto Map(Book source);

    public override partial void Map(Book source, BookDto destination);
}

[Mapper]
public partial class CreateUpdateBookDtoToBookMapper : MapperBase<CreateUpdateBookDto, Book>
{
    public override partial Book Map(CreateUpdateBookDto source);

    public override partial void Map(CreateUpdateBookDto source, Book destination);
}

[Mapper]
public partial class AuthorToAuthorDtoMapper : MapperBase<Author, AuthorDto>
{
    public override partial AuthorDto Map(Author source);

    public override partial void Map(Author source, AuthorDto destination);
}

[Mapper]
public partial class AuthorToAuthorLookupDtoMapper : MapperBase<Author, AuthorLookupDto>
{
    public override partial AuthorLookupDto Map(Author source);

    public override partial void Map(Author source, AuthorLookupDto destination);
}

[Mapper]
public partial class PublicationToPublicationDtoMapper : MapperBase<Publication, PublicationDto>
{
    public override partial PublicationDto Map(Publication source);
    public override partial void Map(Publication source, PublicationDto destination);
}

[Mapper]
public partial class PublicationToPublicationLookupDtoMapper : MapperBase<Publication, PublicationLookupDto>
{
    public override partial PublicationLookupDto Map(Publication source);

    public override partial void Map(Publication source, PublicationLookupDto destination);
}