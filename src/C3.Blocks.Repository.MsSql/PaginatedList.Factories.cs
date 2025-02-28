using System.Linq.Expressions;

namespace C3.Blocks.Repository.MsSql;

/// <summary>
/// Provides methods to create a paginated list from a query.
/// </summary>
public static class PaginatedListFactories
{
    /// <summary>
    /// Creates a paginated list from a query.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <param name="query">The query to paginate.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list.</returns>
    public static async Task<PaginatedList<T>> CreatePaginatedListAsync<T>(this IQueryable<T> query, int page, int size, CancellationToken cancellationToken = default)
    {
        var count = await query.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await query
            .Take(size)
            .Skip(size * (page - 1))
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        return items.CreatePaginatedList(count, page, size);
    }

    /// <summary>
    /// Creates a keyset paginated list from a query.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <typeparam name="TOrderKey">The type of the key used for ordering.</typeparam>
    /// <param name="query">The query to paginate.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <param name="before">The starting point for pagination.</param>
    /// <param name="after">The starting point for pagination.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the keyset paginated list.</returns>
    /// <exception cref="ArgumentNullException" />
    public static async Task<KeysetPaginatedList<T, TOrderKey>> CreateKeysetPaginatedListAsync<T, TOrderKey>(
        this IQueryable<T> query,
        Expression<Func<T, TOrderKey>> keySelector,
        int size,
        TOrderKey? before = default,
        TOrderKey? after = default,
        CancellationToken cancellationToken = default)
        where TOrderKey : IComparable
    {
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        if (!EqualityComparer<TOrderKey>.Default.Equals(before, default) && !EqualityComparer<TOrderKey>.Default.Equals(after, default))
        {
            throw new ArgumentException("Optionally supply either a before or after timestamp, but not both");
        }

        query = query
            .OrderBy(keySelector);

        if (!EqualityComparer<TOrderKey>.Default.Equals(before, default))
        {
            var parameter = keySelector.Parameters[0];
            var condition = Expression.GreaterThan(Expression.Constant(before), keySelector.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
            query = query.Where(lambda);
        }

        if (!EqualityComparer<TOrderKey>.Default.Equals(after, default))
        {
            var parameter = keySelector.Parameters[0];
            var condition = Expression.LessThan(Expression.Constant(after), keySelector.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
            query = query.Where(lambda);
        }

        var items = await query
            .Take(size)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        if (items.Count == 0)
        {
            return items.CreateKeysetPaginatedList<T, TOrderKey>(default!, default!, size);
        }

        var minValue = await query.OrderBy(keySelector).MinAsync(keySelector, cancellationToken).ConfigureAwait(false);
        var maxValue = await query.OrderBy(keySelector).MaxAsync(keySelector, cancellationToken).ConfigureAwait(false);

        return items.CreateKeysetPaginatedList(minValue, maxValue, size);
    }

    /// <summary>
    /// Creates a keyset paginated list in descending order from a query.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <typeparam name="TOrderKey">The type of the key used for ordering.</typeparam>
    /// <param name="query">The query to paginate.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <param name="before">The starting point for pagination.</param>
    /// <param name="after">The starting point for pagination.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the keyset paginated list in descending order.</returns>
    /// <exception cref="ArgumentNullException" />
    public static async Task<KeysetPaginatedList<T, TOrderKey>> CreateKeysetPaginatedListDescendingAsync<T, TOrderKey>(
        this IQueryable<T> query,
        Expression<Func<T, TOrderKey>> keySelector,
        int size,
        TOrderKey? before = default,
        TOrderKey? after = default,
        CancellationToken cancellationToken = default)
            where TOrderKey : IComparable
    {
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        if (!EqualityComparer<TOrderKey>.Default.Equals(before, default) && !EqualityComparer<TOrderKey>.Default.Equals(after, default))
        {
            throw new ArgumentException("Optionally supply either a before or after timestamp, but not both");
        }

        query = query
            .OrderByDescending(keySelector);

        if (!EqualityComparer<TOrderKey>.Default.Equals(before, default))
        {
            var parameter = keySelector.Parameters[0];
            var condition = Expression.GreaterThan(Expression.Constant(before), keySelector.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
            query = query.Where(lambda);
        }

        if (!EqualityComparer<TOrderKey>.Default.Equals(after, default))
        {
            var parameter = keySelector.Parameters[0];
            var condition = Expression.LessThan(Expression.Constant(after), keySelector.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
            query = query.Where(lambda);
        }

        var items = await query
            .Take(size)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        if (items.Count == 0)
        {
            return items.CreateKeysetPaginatedList<T, TOrderKey>(default!, default!, size);
        }

        var minValue = await query.OrderBy(keySelector).MinAsync(keySelector, cancellationToken).ConfigureAwait(false);
        var maxValue = await query.OrderBy(keySelector).MaxAsync(keySelector, cancellationToken).ConfigureAwait(false);

        return items.CreateKeysetPaginatedList(minValue, maxValue, size);
    }
}
