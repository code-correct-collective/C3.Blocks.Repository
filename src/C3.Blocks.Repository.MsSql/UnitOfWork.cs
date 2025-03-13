using System.Diagnostics.CodeAnalysis;

namespace C3.Blocks.Repository.MsSql;

/// <summary>
/// Represents a unit of work that encapsulates a database context and a logger factory.
/// </summary>
/// <typeparam name="TDbContext">The type of the database context.</typeparam>
/// <param name="dbContext">The database context instance.</param>
/// <param name="loggerFactory">The logger factory instance.</param>
public class UnitOfWork<TDbContext>(TDbContext dbContext, ILoggerFactory loggerFactory) : IUnitOfWork
    where TDbContext : DbContext
{
    private readonly ILogger logger = loggerFactory.CreateLogger<UnitOfWork<TDbContext>>();
    private bool disposed;

    /// <summary>
    /// Gets the database context.
    /// </summary>
    protected TDbContext Context { get; private set; } = dbContext;

    /// <summary>
    /// Gets the logger factory.
    /// </summary>
    protected ILoggerFactory LoggerFactory { get; private set; } = loggerFactory;


    /// <inheritdoc/>
    [ExcludeFromCodeCoverage(Justification = "Unsure to to mock this with NSubstitute")]
    public ITransaction BeginTransaction(IsolationLevel isolationLevel)
    {
        ObjectDisposedException.ThrowIf(this.disposed, this);
        this.logger.LogTraceMethod(nameof(BeginTransaction), [isolationLevel]);
        return new TransactionWrapper(this.Context.Database.BeginTransaction(isolationLevel));
    }

    /// <inheritdoc/>
    public ITransaction BeginTransaction()
    {
        ObjectDisposedException.ThrowIf(this.disposed, this);
        this.logger.LogTraceMethod(nameof(BeginTransaction), []);
        return new TransactionWrapper(this.Context.Database.BeginTransaction());
    }

    /// <inheritdoc/>
    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(this.disposed, this);
        this.logger.LogTraceMethod(nameof(BeginTransactionAsync), []);
        var t = await this.Context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        return new TransactionWrapper(t);
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage(Justification = "Unsure to to mock this with NSubstitute")]
    public async Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(this.disposed, this);
        this.logger.LogTraceMethod(nameof(BeginTransaction), [isolationLevel]);
        var t = await this.Context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);
        return new TransactionWrapper(t);
    }

    /// <inheritdoc/>
    public async Task<int> CommitAsync()
    {
        ObjectDisposedException.ThrowIf(this.disposed, this);
        this.logger.LogTraceMethod(nameof(CommitAsync), []);
        return await this.Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage(Justification = "Cannot get the Mocking to work.")]
    public async Task ExecuteInResilientTransactionAsync(Func<Task<bool>> actionAsync, IsolationLevel isolationLevel = IsolationLevel.Unspecified, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(this.disposed, this);
        this.logger.LogTraceMethod(nameof(ExecuteInResilientTransactionAsync), []);
        var strategy = this.Context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await this.Context.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);
            try
            {
                var shouldCommit = await actionAsync().ConfigureAwait(false);
                if (shouldCommit)
                {
                    this.logger.LogDebugMethod(nameof(ExecuteInResilientTransactionAsync), "Committing transaction", []);
                    await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    this.logger.LogDebugMethod(nameof(ExecuteInResilientTransactionAsync), "Rolling back a transaction", []);
                    await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                }
            }
            catch (Exception x)
            {
                this.logger.LogErrorMethod(x, nameof(ExecuteInResilientTransactionAsync), "Failed running in transaction", []);
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                throw;
            }
        }).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await this.DisposeAsyncCore().ConfigureAwait(false);
        this.Dispose(disposing: false);

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the resources used by the UnitOfWork.
    /// </summary>
    /// <param name="disposing">A boolean value indicating whether the method is called from Dispose method.</param>
    protected virtual void Dispose(bool disposing)
    {
        this.logger.LogTraceMethod(nameof(Dispose), []);
        if (disposing)
        {
            this.Context?.Dispose();
        }

        this.Context = null!;
        this.disposed = true;
    }

    /// <summary>
    /// Disposes the resources used by the UnitOfWork asynchronously.
    /// </summary>
    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (this.Context is not null)
        {
            await this.Context.DisposeAsync().ConfigureAwait(false);
        }

        this.Context = null!;
        this.disposed = true;
    }
}
