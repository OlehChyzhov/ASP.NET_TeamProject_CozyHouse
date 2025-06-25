using CozyHouse.Core.Domain.Entities;

namespace CozyHouse.Core.DataAccess
{
    /// <summary>
    /// Defines a unit of work that coordinates changes across multiple repositories,
    /// ensuring that all changes are saved together in a single transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for managing <see cref="Publication"/> entities.
        /// </summary>
        IRepository<Publication> Publications { get; }

        /// <summary>
        /// Gets the repository for managing <see cref="Pet"/> entities.
        /// </summary>
        IRepository<Pet> Pets { get; }

        /// <summary>
        /// Gets the repository for managing <see cref="AdoptionRequest"/> entities.
        /// </summary>
        IRepository<AdoptionRequest> AdoptionRequests { get; }

        /// <summary>
        /// Gets the repository for managing <see cref="Image"/> entities.
        /// </summary>
        IRepository<Image> Images { get; }

        /// <summary>
        /// Commits all pending changes to the database in a single transaction.
        /// </summary>
        void SaveChanges();
    }
}
