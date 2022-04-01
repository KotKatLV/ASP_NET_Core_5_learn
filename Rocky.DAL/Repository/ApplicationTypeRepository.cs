using Rocky.Domain;

namespace Rocky.DAL.Repository.Interfaces
{
    public class ApplicationTypeRepository : Repository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationType applicationType)
        {
            var appTypeFromDb = FirstOrDefault(aT => aT.Id == applicationType.Id);

            if (appTypeFromDb != null)
            {
                appTypeFromDb.Name = applicationType.Name;
            }
        }
    }
}