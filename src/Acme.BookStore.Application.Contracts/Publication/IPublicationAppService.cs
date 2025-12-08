using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Publications
{
    public interface IPublicationAppService : IApplicationService
    {
        Task<PublicationDto> GetAsync(Guid id);
        Task<PagedResultDto<PublicationDto>> GetListAsync(GetPublicationListDto input);
        Task<PublicationDto> CreateAsync(CreatePublicationDto input);
        Task UpdateAsync(Guid id, UpdatePublicationDto input);
        Task DeleteAsync(Guid id);
    }
}
