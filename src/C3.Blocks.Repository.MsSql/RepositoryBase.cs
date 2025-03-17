using System.ComponentModel.DataAnnotations;

namespace C3.Blocks.Repository.MsSql;

/// <summary>
/// Base repository class for handling database operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the entity identifier.</typeparam>
/// <typeparam name="TDbContext">The type of the database context.</typeparam>
/// <param name="context">The database context.</param>
/// <param name="logger">The logger instance.</param>
public class RepositoryBase<TEntity, TId, TDbContext>([Required] TDbContext context, [Required] ILogger<RepositoryBase<TEntity, TId, TDbContext>> logger) : IRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TDbContext : DbContext
{
    /// <summary>
    /// Gets the logger instance.
    /// </summary>
    protected ILogger Logger { get; } = logger;

    /// <summary>
    /// Gets the database context.
    /// </summary>
    protected TDbContext Context { get; private set; } = context;

    /// <summary>
    /// Gets the DbSet of the entity.
    /// </summary>
    protected DbSet<TEntity> Entities { get; } = context.Set<TEntity>();

    /// <inheritdoc/>
    public async ValueTask<TEntity?> FindAsync([Required] TId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        return await this.FindAsync([id], cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask<TEntity?> FindAsync(TId?[] ids, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTraceMethod(nameof(FindAsync), ids.Cast<object>().ToArray());
        this.Logger.LogFindEntity(LogLevel.Information, ids.Cast<object>().ToArray());
        return await this.Entities.FindAsync(keyValues: ids.Cast<object>().ToArray(), cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTraceMethod(nameof(AddAsync), [entity]);
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        if (entity.Id == null)
        {
            this.Logger.LogAddEntityNull();
            throw new ArgumentNullException(nameof(entity), "Entity is required.");
        }

        this.Logger.LogAddEntityAttempt(entity.Id);

        var result = await this.Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        return result.Entity;
    }

    /// <inheritdoc/>
    public Task AddRangeAsync(TEntity[] entities, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTraceMethod(nameof(AddRangeAsync), entities);
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        cancellationToken.ThrowIfCancellationRequested();
        return this.AddRangeAsync(entities.AsEnumerable(), cancellationToken);
    }

    /// <inheritdoc/>
    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTraceMethod(nameof(AddRangeAsync), [entities]);
        this.Logger.LogAddRangeEntity(entities.Count());
        cancellationToken.ThrowIfCancellationRequested();
        return this.Entities.AddRangeAsync(entities, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTraceMethod(nameof(UpdateAsync), [entity]);
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        if (entity.Id == null)
        {
            this.Logger.LogUpdateEntityNull();
            throw new ArgumentNullException(nameof(entity), "Entity is required.");
        }

        this.Logger.LogUpdateEntityAttempt(entity.Id);
        cancellationToken.ThrowIfCancellationRequested();
        this.Entities.Update(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task UpdateRangeAsync(TEntity[] entities, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTraceMethod(nameof(UpdateRangeAsync), entities);
        return this.UpdateRangeAsync(entities.AsEnumerable(), cancellationToken);
    }

    /// <inheritdoc/>
    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        this.Logger.LogTraceMethod(nameof(UpdateRangeAsync), [entities]);
        this.Logger.LogUpdateRangeEntity(entities.Count());
        cancellationToken.ThrowIfCancellationRequested();
        this.Entities.UpdateRange(entities);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTraceMethod(nameof(RemoveAsync), [entity]);
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        if (entity.Id == null)
        {
            this.Logger.LogRemoveEntityNull();
            throw new ArgumentNullException(nameof(entity), "Entity is required.");
        }

        this.Logger.LogRemoveEntityAttempt(entity.Id);
        cancellationToken.ThrowIfCancellationRequested();
        this.Entities.Remove(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task RemoveRangeAsync(TEntity[] entities, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTraceMethod(nameof(RemoveAsync), entities);
        cancellationToken.ThrowIfCancellationRequested();
        return this.RemoveRangeAsync(entities.AsEnumerable(), cancellationToken);
    }

    /// <inheritdoc/>
    public Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var items = entities.ToArray();
        this.Logger.LogTraceMethod(nameof(RemoveAsync), items);
        this.Logger.LogRemoveRangeEntity(items.Length);
        cancellationToken.ThrowIfCancellationRequested();
        this.Entities.RemoveRange(entities);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Disposes the repository.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the repository asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        this.Logger.LogTraceMethod(nameof(DisposeAsync), []);
        await this.DisposeAsyncCore().ConfigureAwait(false);
        this.Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the repository.
    /// </summary>
    /// <param name="disposing">A boolean value indicating whether the method is called from the Dispose method.</param>
    protected virtual void Dispose(bool disposing)
    {
        this.Logger.LogTraceMethod(nameof(Dispose), [disposing]);
        if (disposing)
        {
            this.Context?.Dispose();
        }

        this.Context = null!;
    }

    /// <summary>
    /// Disposes the repository asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    protected async virtual ValueTask DisposeAsyncCore()
    {
        this.Logger.LogTraceMethod(nameof(DisposeAsyncCore), []);
        if (this.Context is not null)
        {
            await this.Context.DisposeAsync().ConfigureAwait(false);
        }

        this.Context = null!;
    }
}
