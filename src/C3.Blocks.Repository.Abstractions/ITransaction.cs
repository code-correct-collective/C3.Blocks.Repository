namespace C3.Blocks.Repository.Abstractions;

/// <summary>
/// Represents a transaction with commit and rollback capabilities.
/// </summary>
public interface ITransaction : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the unique identifier for the transaction.
    /// </summary>
    Guid TransactionId { get; }

    /// <summary>
    /// Commits the transaction.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rolls back the transaction.
    /// </summary>
    void Rollback();
}
