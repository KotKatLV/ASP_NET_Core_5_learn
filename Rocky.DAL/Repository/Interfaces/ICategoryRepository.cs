using Rocky.Domain;

namespace Rocky.DAL.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}