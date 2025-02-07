using System.Collections.ObjectModel;
using System.Diagnostics;

namespace C3.Blocks.Domain;

/// <summary>
/// Represents a paginated list of items.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
[DebuggerDisplay("Total: {" + nameof(Total) + "}; Page: {" + nameof(Page) + "}; Size: {" + nameof(Size) + "}")]
public sealed record PaginatedList<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedList{T}"/> class.
    /// </summary>
    /// <param name="items">The list of items.</param>
    /// <param name="total">The total number of items.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="page">The current page number.</param>
    /// <exception cref="ArgumentNullException" />
    public PaginatedList(IList<T> items, int total, int size, int page)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));
        this.Items = new ReadOnlyCollection<T>(items);
        this.Total = total;
        this.Size = size;
        this.Page = page;
    }
    /// <summary>
    /// Gets the list of items.
    /// </summary>
    public IReadOnlyList<T> Items { get; }

    /// <summary>
    /// Gets the total number of items.
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int Size { get; }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(Total / (double)Size);
}
