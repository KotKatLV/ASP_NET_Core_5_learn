using Rocky.DAL.Repository.Interfaces;
using Rocky.Domain;

namespace Rocky.DAL.Repository
{
    public class InquiryHeaderRepository : Repository<InquiryHeader>, IInquiryHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public InquiryHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InquiryHeader entity)
        {
            _db.InquiryHeader.Update(entity);
        }
    }
}