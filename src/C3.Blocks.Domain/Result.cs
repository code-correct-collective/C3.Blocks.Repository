namespace C3.Blocks.Domain;

/// <summary>
/// Represents the result of an operation, including a value, success status, and a message.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <param name="Data">The data returned by the operation. Can be null if the operation was not successful.</param>
/// <param name="Success">Indicates whether the operation was successful. Defaults to true.</param>
/// <param name="Message">An optional message providing additional information about the result of the operation. Defaults to an empty string.</param>
public sealed record Result<T>(T? Data, bool Success = true, string Message = "")
{
}
