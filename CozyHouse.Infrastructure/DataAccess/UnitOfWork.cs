using CozyHouse.Core.DataAccess;
using CozyHouse.Core.Domain.Entities;
using CozyHouse.Infrastructure.Database;

namespace CozyHouse.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public IRepository<Publication> Publications { get; }
        public IRepository<Pet> Pets { get; }
        public IRepository<AdoptionRequest> AdoptionRequests { get; }
        public IRepository<Image> Images { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Publications = new Repository<Publication>(context);
            Pets = new Repository<Pet>(context);
            AdoptionRequests = new Repository<AdoptionRequest>(context);
            Images = new Repository<Image>(context);
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
