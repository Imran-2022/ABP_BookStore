using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Acme.BookStore.Publications
{
    public class PublicationManager : DomainService
    {
        private readonly IPublicationRepository _publicationRepository;

        public PublicationManager(IPublicationRepository publicationRepository)
        {
            _publicationRepository = publicationRepository;
        }

        public async Task<Publication> CreateAsync(string name, string location = null, string website = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _publicationRepository.FindByNameAsync(name);
            if (existing != null)
            {
                throw new PublicationAlreadyExistsException(name);
            }

            return new Publication(GuidGenerator.Create(), name, location, website);
        }

        public async Task ChangeNameAsync(Publication publication, string newName)
        {
            Check.NotNull(publication, nameof(publication));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _publicationRepository.FindByNameAsync(newName);
            if (existing != null && existing.Id != publication.Id)
            {
                throw new PublicationAlreadyExistsException(newName);
            }

            publication.ChangeName(newName);
        }
    }
}
