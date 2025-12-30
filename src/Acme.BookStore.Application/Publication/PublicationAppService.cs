using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Publications
{
    [Authorize(BookStorePermissions.Publications.Default)]
    public class PublicationAppService : BookStoreAppService, IPublicationAppService
    {
        private readonly IPublicationRepository _publicationRepository;
        private readonly PublicationManager _publicationManager;

        public PublicationAppService(
            IPublicationRepository publicationRepository,
            PublicationManager publicationManager)
        {
            _publicationRepository = publicationRepository;
            _publicationManager = publicationManager;
        }

        public async Task<PublicationDto> GetAsync(Guid id)
        {   
            Logger.LogInformation("Getting publication with Id: {Id}", id);
            var publication = await _publicationRepository.GetAsync(id);
            Logger.LogInformation("Publication {Id} retrieved successfully", id);
            return ObjectMapper.Map<Publication, PublicationDto>(publication);
        }

        public async Task<PagedResultDto<PublicationDto>> GetListAsync(GetPublicationListDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                input.Sorting = nameof(Publication.Name);
            }

            var publications = await _publicationRepository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Filter
            );

            var totalCount = string.IsNullOrWhiteSpace(input.Filter)
                ? await _publicationRepository.CountAsync()
                : await _publicationRepository.CountAsync(p => p.Name.Contains(input.Filter));
            Logger.LogInformation("Returned {Count} publications", publications.Count);
            return new PagedResultDto<PublicationDto>(
                totalCount,
                ObjectMapper.Map<List<Publication>, List<PublicationDto>>(publications)
            );
        }

        [Authorize(BookStorePermissions.Publications.Create)]
        public async Task<PublicationDto> CreateAsync(CreatePublicationDto input)
        {
            var publication = await _publicationManager.CreateAsync(input.Name, input.Location, input.Website);
            await _publicationRepository.InsertAsync(publication);
            Logger.LogInformation(
                "Publication created successfully"
            );
            return ObjectMapper.Map<Publication, PublicationDto>(publication);
        }

        [Authorize(BookStorePermissions.Publications.Edit)]
        public async Task UpdateAsync(Guid id, UpdatePublicationDto input)
        {
            Logger.LogInformation("Updating publication with Id: {Id}", id);
            var publication = await _publicationRepository.GetAsync(id);

            if (publication.Name != input.Name)
            {
                Logger.LogInformation(
                    "Changing publication name from {OldName} to {NewName}",
                    publication.Name,
                    input.Name
                );
                await _publicationManager.ChangeNameAsync(publication, input.Name);
            }

            publication.ChangeLocation(input.Location);
            publication.ChangeWebsite(input.Website);

            await _publicationRepository.UpdateAsync(publication);
            Logger.LogInformation("Publication {Id} updated successfully", id);
        }

        [Authorize(BookStorePermissions.Publications.Delete)]
        public async Task DeleteAsync(Guid id)
        {   
            Logger.LogWarning("Deleting publication with Id: {Id}", id);
            await _publicationRepository.DeleteAsync(id);
            Logger.LogInformation("Publication {Id} deleted successfully", id);
        }
    }
}
