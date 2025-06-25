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
        /// Gets the repository for managing <see cref="ShelterPetPublication"/> entities.
        /// </summary>
        IRepository<ShelterPetPublication> ShelterPetPublications { get; }

        /// <summary>
        /// Gets the repository for managing <see cref="UserPetPublication"/> entities.
        /// </summary>
        IRepository<UserPetPublication> UserPetPublications { get; }

        /// <summary>
        /// Gets the repository for managing <see cref="ShelterAdoptionRequest"/> entities.
        /// </summary>
        IRepository<ShelterAdoptionRequest> ShelterAdoptionRequests { get; }

        /// <summary>
        /// Gets the repository for managing <see cref="UserAdoptionRequest"/> entities.
        /// </summary>
        IRepository<UserAdoptionRequest> UserAdoptionRequests { get; }

        /// <summary>
        /// Gets the repository for managing <see cref="PetImage"/> entities.
        /// </summary>
        IRepository<PetImage> PetImages { get; }

        /// <summary>
        /// Commits all pending changes to the database in a single transaction.
        /// </summary>
        void SaveChanges();
    }
}
