using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Publications
{
    public class Publication : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Location { get; private set; }
        public string Website { get; private set; }

        private Publication() { } // For ORM

        internal Publication(Guid id, string name, string location = null, string website = null)
            : base(id)
        {
            SetName(name);
            Location = location;
            Website = website;
        }

        internal Publication ChangeName(string name)
        {
            SetName(name);
            return this;
        }

        // Make these public so AppService can access
        public void ChangeLocation(string location)
        {
            Location = location;
        }

        public void ChangeWebsite(string website)
        {
            Website = website;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), PublicationConsts.MaxNameLength);
        }
    }
}
