using Rocky.DAL.Repository.Interfaces;
using Rocky.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky.DAL.Repository
{
    public class InquiryDetailRepository : Repository<InquiryDetail>, IInquiryDetailRepository
    {
        private readonly ApplicationDbContext _db;

        public InquiryDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;   
        }

        public void Update(InquiryDetail entity)
        {
            _db.InquiryDetail.Update(entity);   
        }
    }
}