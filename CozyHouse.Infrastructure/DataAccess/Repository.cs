using CozyHouse.Core.DataAccess;
using CozyHouse.Core.Domain.Entities.Interfaces;
using CozyHouse.Infrastructure.Database;

namespace CozyHouse.Infrastructure.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly ApplicationDbContext _context;
        
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public T Get(Guid id)
        {
            return _context.Set<T>().First(entity => entity.Id == id);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }
    }
}
