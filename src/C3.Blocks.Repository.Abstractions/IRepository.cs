namespace C3.Blocks.Repository.Abstractions;

/// <summary>
/// Represents a generic repository interface.
/// </summary>
public interface IRepository
{
}

/// <summary>
/// Represents a generic repository interface.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public interface IRepository<TEntity, in TId> : IRepository, IDisposable, IAsyncDisposable
    where TEntity : class
{
    /// <summary>
    /// Finds the entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns a single <typeparamref name="TEntity" />.</returns>
    ValueTask<TEntity?> FindAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds the asynchronous.
    /// </summary>
    /// <param name="ids">The ids.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;TEntity&gt;.</returns>
    ValueTask<TEntity?> FindAsync(TId[] ids, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>ValueTask&lt;TEntity&gt;.</returns>
    ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds the range asynchronous.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns>Task&lt;TEntity&gt;.</returns>
    /// <returns>Task.</returns>
    Task AddRangeAsync(params TEntity[] entities);

    /// <summary>
    /// Adds the range asynchronous.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns>Task.</returns>
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>A void <see cref="Task"/>.</returns>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Updates the range asynchronous.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns>Task.</returns>
    Task UpdateRangeAsync(params TEntity[] entities);

    /// <summary>
    /// Updates the range asynchronous.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns>Task.</returns>
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Deletes the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>A void <see cref="Task"/>.</returns>
    Task RemoveAsync(TEntity entity);

    /// <summary>
    /// Removes the range asynchronous.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns>Task.</returns>
    Task RemoveRangeAsync(params TEntity[] entities);

    /// <summary>
    /// Removes the range asynchronous.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns>Task.</returns>
    Task RemoveRangeAsync(IEnumerable<TEntity> entities);
}
