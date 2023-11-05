namespace Locompro.Common;
/// <summary>
///   Interface Factory for creating <see cref="IErrorStore"/> instances.
/// </summary>
public interface IErrorStoreFactory
{
    /// <summary>
    ///  Creates a new instance of <see cref="IErrorStore"/>.
    /// </summary>
    /// <returns> a new instance of <see cref="IErrorStore"/>.</returns>
    IErrorStore Create();
}