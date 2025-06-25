namespace CozyHouse.Core.DataAccess
{
    /// <summary>
    /// Provides methods for retrieving and submitting data to the data storage.
    /// </summary>
    /// <typeparam name="T">Type of the entity the repository works with.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Adds a new entity to the data source.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void Add(T entity);

        /// <summary>
        /// Retrieves an entity from the data source by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <returns>The entity with the specified identifier.</returns>
        T Get(Guid id);

        /// <summary>
        /// Updates the given entity in the data source.
        /// </summary>
        /// <param name="entity">The entity with updated values.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes the entity with the specified identifier from the data source.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        /// Retrieves all entities from the data source.
        /// </summary>
        /// <returns>An <see cref="IQueryable{T}"/> collection of all entities.</returns>
        IQueryable<T> GetAll();
    }

}
