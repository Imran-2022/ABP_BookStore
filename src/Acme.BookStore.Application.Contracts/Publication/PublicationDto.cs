using System;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Publications
{
    public class PublicationDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
    }
}
