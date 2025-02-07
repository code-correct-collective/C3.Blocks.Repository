using System.Collections.ObjectModel;
using System.Diagnostics;

namespace C3.Blocks.Domain;

/// <summary>
/// Represents a paginated list of items with keyset pagination.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
/// <typeparam name="TOrderKey">The type of the key used for ordering the pagination items.</typeparam>
[DebuggerDisplay("MinValue: {" + nameof(MinValue) + "}; MaxValue: {" + nameof(MaxValue) + "}; Size: {" + nameof(Size) + "}")]
public sealed record KeysetPaginatedList<T, TOrderKey>
    where TOrderKey : IComparable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KeysetPaginatedList{T, TOrderKey}"/> class.
    /// </summary>
    /// <param name="items">The list of items.</param>
    /// <param name="minValue">The minimum value of the order key.</param>
    /// <param name="maxValue">The maximum value of the order key.</param>
    /// <param name="size">The size of the paginated list.</param>
    public KeysetPaginatedList(IList<T> items, TOrderKey minValue, TOrderKey maxValue, int size)
    {
        this.Items = new ReadOnlyCollection<T>(items);
        this.MinValue = minValue;
        this.MaxValue = maxValue;
        this.Size = size;
    }

    /// <summary>
    /// Gets the list of items in the paginated list.
    /// </summary>
    public IReadOnlyList<T> Items { get; }

    /// <summary>
    /// Gets the minimum value of the order key.
    /// </summary>
    public TOrderKey MinValue { get; }

    /// <summary>
    /// Gets the maximum value of the order key.
    /// </summary>
    public TOrderKey MaxValue { get; }

    /// <summary>
    /// Gets the size of the paginated list.
    /// </summary>
    public int Size { get; }
}
