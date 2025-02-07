using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace C3.Blocks.Domain;

/// <summary>
/// Represents a base entity with an identifier.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public abstract class EntityBase<TId> : IEntity<TId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityBase{TId}"/> class.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <exception cref="ArgumentNullException">Thrown when the id is null.</exception>
    protected EntityBase(TId id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        this.Id = id;
    }

    /// <summary>
    /// Gets the identifier of the entity.
    /// </summary>
    [NotNull]
    [JsonInclude]
    public virtual TId Id { get; protected set; }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.Id.GetHashCode() + 17;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != this.GetType())
        {
            return false;
        }

        var e = (EntityBase<TId>)obj;

        return e.Id.Equals(this.Id);
    }
}
