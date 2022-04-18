using Rocky.Domain;

namespace Rocky.DAL.Repository.Interfaces
{
    public interface IInquiryHeaderRepository : IRepository<InquiryHeader>
    {
        void Update(InquiryHeader entity);
    }
}