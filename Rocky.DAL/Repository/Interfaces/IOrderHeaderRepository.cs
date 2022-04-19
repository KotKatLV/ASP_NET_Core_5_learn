using Rocky.Domain;

namespace Rocky.DAL.Repository.Interfaces
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader entity);
    }
}