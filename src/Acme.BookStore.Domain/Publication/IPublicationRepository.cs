using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Publications
{
    public interface IPublicationRepository : IRepository<Publication, Guid>
    {
        Task<Publication> FindByNameAsync(string name);

        Task<List<Publication>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null);
    }
}
