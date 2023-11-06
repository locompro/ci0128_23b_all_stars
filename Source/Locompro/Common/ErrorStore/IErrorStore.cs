namespace Locompro.Common.ErrorStore;

/// <summary>
///     Defines a contract for a store that can hold and manage error messages.
/// </summary>
public interface IErrorStore
{
    /// <summary>
    ///     Gets a value indicating whether the store has any errors.
    /// </summary>
    bool HasErrors { get; }

    /// <summary>
    ///     Gets the count of error messages currently stored.
    /// </summary>
    int Count { get; }

    /// <summary>
    ///     Stores a single error message in the store.
    /// </summary>
    /// <param name="error">The error message to store.</param>
    void StoreError(string error);

    /// <summary>
    ///     Stores a list of error messages in the store.
    /// </summary>
    /// <param name="errors">The list of error messages to store.</param>
    void StoreErrors(IEnumerable<string> errors);

    /// <summary>
    ///     Retrieves all the stored error messages and clears the store.
    /// </summary>
    /// <returns>The list of error messages.</returns>
    IList<string> GetErrors();

    /// <summary>
    ///     Clears all error messages from the store.
    /// </summary>
    void ClearStore();
}