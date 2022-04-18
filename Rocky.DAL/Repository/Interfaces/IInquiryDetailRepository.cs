using Rocky.Domain;

namespace Rocky.DAL.Repository.Interfaces
{
    public interface IInquiryDetailRepository : IRepository<InquiryDetail>
    {
        void Update(InquiryDetail entity);
    }
}
