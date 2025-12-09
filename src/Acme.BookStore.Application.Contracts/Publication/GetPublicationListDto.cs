using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Publications
{
    public class GetPublicationListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
    }
}
