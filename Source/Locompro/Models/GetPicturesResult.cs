namespace Locompro.Models;

/// <summary>
/// The result of a search for pictures.
/// </summary>
public class GetPicturesResult
{
    public string PictureTitle { get; init; }
    /// <summary>
    ///  The picture represented in bytes
    /// </summary>
    public byte[] PictureData { get; init; }
}