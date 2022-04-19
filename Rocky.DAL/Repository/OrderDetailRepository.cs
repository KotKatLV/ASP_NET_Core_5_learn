using Rocky.DAL.Repository.Interfaces;
using Rocky.Domain;

namespace Rocky.DAL.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;   
        }

        public void Update(OrderDetail entity)
        {
            _db.OrderDetail.Update(entity);
        }
    }
}