namespace C3.Blocks.Domain;

/// <summary>
/// Represents an entity with an identifier of type TId.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public interface IEntity<out TId>
{
    /// <summary>
    /// Gets the identifier of the entity.
    /// </summary>
    TId Id { get; }
}
