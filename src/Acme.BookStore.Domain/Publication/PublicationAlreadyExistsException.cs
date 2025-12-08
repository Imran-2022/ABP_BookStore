using Volo.Abp;

namespace Acme.BookStore.Publications
{
    public class PublicationAlreadyExistsException : BusinessException
    {
        public PublicationAlreadyExistsException(string name)
            : base(BookStoreDomainErrorCodes.PublicationAlreadyExists)
        {
            WithData("name", name);
        }
    }
}
