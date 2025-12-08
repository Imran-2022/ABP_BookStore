using System.ComponentModel.DataAnnotations;

namespace Acme.BookStore.Publications
{
    public class UpdatePublicationDto
    {
        [Required]
        [StringLength(PublicationConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(PublicationConsts.MaxLocationLength)]
        public string Location { get; set; }

        [StringLength(PublicationConsts.MaxWebsiteLength)]
        public string Website { get; set; }
    }
}
