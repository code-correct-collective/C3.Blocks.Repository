using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;

namespace C3.Blocks.Repository.MsSql;

/// <summary>
/// Initializes a new instance of the <see cref="TransactionWrapper"/> class.
/// </summary>
/// <param name="dbContextTransaction">The database context transaction.</param>
public sealed class TransactionWrapper([Required] IDbContextTransaction dbContextTransaction) : ITransaction
{
    private bool disposed;

    private IDbContextTransaction dbContextTransaction = dbContextTransaction;

    /// <summary>
    /// Gets the transaction identifier.
    /// </summary>
    public Guid TransactionId => this.dbContextTransaction.TransactionId;

    /// <summary>
    /// Commits the transaction asynchronously.
    /// </summary>
    public async Task CommitAsync()
    {
        ObjectDisposedException.ThrowIf(this.disposed, this);
        await this.dbContextTransaction!.CommitAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Commits the transaction.
    /// </summary>
    public void Commit()
    {
        ObjectDisposedException.ThrowIf(this.disposed, this);
        this.dbContextTransaction!.Commit();
    }

    /// <summary>
    /// Rolls back the transaction asynchronously.
    /// </summary>
    public async Task RollbackAsync()
    {
        ObjectDisposedException.ThrowIf(this.disposed, this);
        await this.dbContextTransaction!.RollbackAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Rolls back the transaction.
    /// </summary>
    public void Rollback()
    {
        ObjectDisposedException.ThrowIf(this.disposed, this);
        this.dbContextTransaction!.Rollback();
    }
    /// <summary>
    /// Disposes the transaction.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the transaction asynchronously.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await this.DisposeAsyncCore().ConfigureAwait(false);

        this.Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.dbContextTransaction?.Dispose();
        }

        this.dbContextTransaction = null!;
        this.disposed = true;
    }

    private async ValueTask DisposeAsyncCore()
    {
        if (this.dbContextTransaction is not null)
        {
            await this.dbContextTransaction.DisposeAsync().ConfigureAwait(false);
        }

        this.dbContextTransaction = null!;
        this.disposed = true;
    }
}
