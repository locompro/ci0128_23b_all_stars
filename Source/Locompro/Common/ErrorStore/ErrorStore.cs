namespace Locompro.Common.ErrorStore;

/// <summary>
///     Implements the <see cref="IErrorStore" /> interface to provide a store for managing error messages.
/// </summary>
public class ErrorStore : IErrorStore
{
    /// <summary>
    ///     Backing field for storing error messages.
    /// </summary>
    private readonly IList<string> _errors = new List<string>();

    /// <summary>
    ///     Gets the count of error messages currently stored.
    /// </summary>
    public int Count => _errors.Count;

    /// <summary>
    ///     Gets a value indicating whether the store has any errors.
    /// </summary>
    public bool HasErrors => _errors.Count > 0;

    /// <summary>
    ///     Stores a single error message in the store.
    /// </summary>
    /// <param name="error">The error message to store.</param>
    public void StoreError(string error)
    {
        _errors.Add(error);
    }

    /// <summary>
    ///     Stores a list of error messages in the store.
    /// </summary>
    /// <param name="errors">The list of error messages to store.</param>
    public void StoreErrors(IEnumerable<string> errors)
    {
        foreach (var error in errors) _errors.Add(error);
    }

    /// <summary>
    ///     Clears all error messages from the store.
    /// </summary>
    public void ClearStore()
    {
        _errors.Clear();
    }

    /// <summary>
    ///     Retrieves all the stored error messages.
    /// </summary>
    /// <returns>The list of error messages.</returns>
    public IList<string> GetErrors()
    {
        var errors = new List<string>(_errors);
        return errors;
    }
}