using Locompro.Models.Results;

namespace Locompro.Models.Dtos;

public class PictureDto
{
    public string Name { get; init; }
    public byte[] PictureData { get; init; }
    
    public PictureDto()
    {
    }

    public PictureDto(GetPicturesResult result)
    {
        Name = result.PictureTitle;
        PictureData = result.PictureData;
    }
}