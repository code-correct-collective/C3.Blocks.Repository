namespace C3.Blocks.Domain;

/// <summary>
/// Provides methods to create paginated lists.
/// </summary>
public static class PaginatedListFactories
{
    /// <summary>
    /// Creates a paginated list from the given items.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="items">The list of items to paginate.</param>
    /// <param name="total">The total number of items.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="size">The number of items per page.</param>
    /// <returns>A paginated list containing the items.</returns>
    public static PaginatedList<T> CreatePaginatedList<T>(this IList<T> items, int total, int page, int size)
    {
        return new PaginatedList<T>(items, total, size, page);
    }

    /// <summary>
    /// Creates a keyset paginated list from the given items.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <typeparam name="TOrderKey">The type of the order key.</typeparam>
    /// <param name="items">The list of items to paginate.</param>
    /// <param name="minValue">The minimum value of the order key.</param>
    /// <param name="maxValue">The maximum value of the order key.</param>
    /// <param name="size">The number of items per page.</param>
    /// <returns>A keyset paginated list containing the items.</returns>
    public static KeysetPaginatedList<T, TOrderKey> CreateKeysetPaginatedList<T, TOrderKey>(this IList<T> items, TOrderKey minValue, TOrderKey maxValue, int size)
        where TOrderKey : IComparable
    {
        return new KeysetPaginatedList<T, TOrderKey>(items, minValue, maxValue, size);
    }
}
