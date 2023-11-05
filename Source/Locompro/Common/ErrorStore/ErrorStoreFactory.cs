namespace Locompro.Common.ErrorStore;
/// <summary>
///   Implementation of <see cref="IErrorStoreFactory"/> for creating <see cref="IErrorStore"/> instances.
/// </summary>
public class ErrorStoreFactory : IErrorStoreFactory
{
    /// <summary>
    ///  Creates a new instance of <see cref="IErrorStore"/>.
    /// </summary>
    /// <returns> a new instance of <see cref="IErrorStore"/>.</returns>
    public IErrorStore Create()
    {
        return new ErrorStore();
    }
}