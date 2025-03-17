namespace C3.Blocks.Repository.Abstractions;

/// <summary>
/// Represents a unit of work that can commit transactions and execute actions within a resilient transaction.
/// </summary>
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Commits the current transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that represents the asynchronous commit operation.</returns>
    Task<int> CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the specified action within a resilient transaction asynchronously.
    /// </summary>
    /// <param name="actionAsync">The action to execute within the transaction.</param>
    /// <param name="isolationLevel">The isolation level to use for the transaction. Unspecified is the default.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ExecuteInResilientTransactionAsync(
        Func<Task<bool>> actionAsync,
        IsolationLevel isolationLevel = IsolationLevel.Unspecified,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new transaction with the specified isolation level.
    /// </summary>
    /// <param name="isolationLevel">The isolation level for the transaction.</param>
    /// <returns>The transaction object.</returns>
    ITransaction BeginTransaction(IsolationLevel isolationLevel);

    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    /// <returns>The transaction object.</returns>
    ITransaction BeginTransaction();

    /// <summary>
    /// Begins a new transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the transaction object.</returns>
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new transaction with the specified isolation level asynchronously.
    /// </summary>
    /// <param name="isolationLevel">The isolation level for the transaction.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the transaction object.</returns>
    Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
}
