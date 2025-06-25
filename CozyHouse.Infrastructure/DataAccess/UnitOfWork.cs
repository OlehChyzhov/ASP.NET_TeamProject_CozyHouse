using CozyHouse.Core.DataAccess;
using CozyHouse.Core.Domain.Entities;
using CozyHouse.Infrastructure.Database;

namespace CozyHouse.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public IRepository<ShelterPetPublication> ShelterPetPublications { get; }
        public IRepository<UserPetPublication> UserPetPublications { get; }
        public IRepository<ShelterAdoptionRequest> ShelterAdoptionRequests { get; }
        public IRepository<UserAdoptionRequest> UserAdoptionRequests { get; }
        public IRepository<PetImage> PetImages { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ShelterPetPublications = new Repository<ShelterPetPublication>(context);
            UserPetPublications = new Repository<UserPetPublication>(context);
            ShelterAdoptionRequests = new Repository<ShelterAdoptionRequest>(context);
            UserAdoptionRequests = new Repository<UserAdoptionRequest>(context);
            PetImages = new Repository<PetImage>(context);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
