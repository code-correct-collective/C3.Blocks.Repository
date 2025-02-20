namespace C3.Blocks.Domain;

/// <summary>
/// Provides factory methods for creating result objects.
/// </summary>
public static class ResultFactories
{
    /// <summary>
    /// Creates an unsuccessful result with the specified value and message.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value of the result.</param>
    /// <param name="message">The message describing the result.</param>
    /// <returns>A new unsuccessful <see cref="Result{T}"/>.</returns>
    public static Result<T> CreateUnsuccessfulResult<T>(this T value, string? message = null)
    {
        return new Result<T>(value, Success: false, message ?? string.Empty);
    }

    /// <summary>
    /// Creates an unsuccessful result with the specified message.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="message">The message describing the result.</param>
    /// <returns>A new unsuccessful <see cref="Result{T}"/>.</returns>
    public static Result<T> CreateUnsuccessfulResult<T>(string message)
    {
        return new Result<T>(default, Success: false, message);
    }

    /// <summary>
    /// Creates a successful result with the specified value and message.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value of the result.</param>
    /// <param name="message">The message describing the result.</param>
    /// <returns>A new successful <see cref="Result{T}"/>.</returns>
    public static Result<T> CreateSuccessfulResult<T>(this T value, string? message = null)
    {
        return new Result<T>(value, Message: message ?? string.Empty);
    }
}
