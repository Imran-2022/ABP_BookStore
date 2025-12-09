using System;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books;

public class PublicationLookupDto: EntityDto<Guid>
{
    public string Name { get; set; }
}
