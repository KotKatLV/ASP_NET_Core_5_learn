using Rocky.Domain;

namespace Rocky.DAL.Repository.Interfaces
{
    public interface IApplicationTypeRepository : IRepository<ApplicationType>
    {
        void Update(ApplicationType applicationType);
    }
}