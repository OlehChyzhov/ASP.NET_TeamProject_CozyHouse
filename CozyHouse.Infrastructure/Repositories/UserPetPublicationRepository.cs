using CozyHouse.Core.Domain.Entities;
using CozyHouse.Core.RepositoryInterfaces;
using CozyHouse.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CozyHouse.Infrastructure.Repositories
{
    public class UserPetPublicationRepository : IUserPetPublicationRepository
    {
        private ApplicationDbContext _db;
        public UserPetPublicationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Create(UserPetPublication publication)
        {
            _db.UserPetPublications.Add(publication);
            _db.SaveChanges();
        }

        public UserPetPublication Read(Guid publicationId)
        {
            return _db.UserPetPublications.Include(pub => pub.Images).Include(o => o.Owner).First(publication => publication.Id == publicationId);
        }

        public void Update(UserPetPublication publication)
        {
            _db.UserPetPublications.Update(publication);
            _db.SaveChanges();
        }

        public void Delete(Guid id)
        {
            UserPetPublication publication = _db.UserPetPublications.First(pub => pub.Id == id);
            _db.UserPetPublications.Remove(publication);
            _db.SaveChanges();
        }

        public bool IsPublicationExists(Guid id)
        {
            UserPetPublication? publication = _db.UserPetPublications.FirstOrDefault(publication => publication.Id == id);
            return publication == null ? false : true;
        }

        public IEnumerable<UserPetPublication> GetAll()
        {
            return _db.UserPetPublications.Include(pub => pub.Images);
        }
    }
}
