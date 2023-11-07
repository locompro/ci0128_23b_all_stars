using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Pages.Util;
using Microsoft.AspNetCore.Http;

namespace Locompro.Tests.Utilities;

[TestFixture]
public class PictureParserTest
{
    [Test]
    public void Parse_ReturnsListOfPictures()
    {
        // Arrange
        IFormFileCollection files = new FormFileCollection();

        // Act
        List<PictureVm> pictures = PictureParser.Parse(files);

        // Assert
        Assert.That(pictures, Is.Not.Null);
    }

    [Test]
    public void ParseSingleImage()
    {
        var ms = new MemoryStream();
        byte[] data = { 0, 1, 2, 3, 4 };
        ms.Write(data, 0, data.Length);

        IFormFile file = new FormFile(ms, 0, 5, "Test", "Test.jpg");

        var picture = PictureParser.ParseSinglePicture(file);

        Assert.Multiple(() =>
        {
            Assert.That(picture.Name, Is.EqualTo("Test.jpg"));
            Assert.That(picture.PictureData, Is.EqualTo(data));
        });
    }

    [Test]
    public void ParserAcceptsValidFormats()
    {
        IFormFile file = new FormFile(new MemoryStream(), 0, 0, "Test", "Test.jpg");
        IFormFile file2 = new FormFile(new MemoryStream(), 0, 0, "Test", "Test.jpeg");
        IFormFile file3 = new FormFile(new MemoryStream(), 0, 0, "Test", "Test.png");

        List<PictureVm> pictures = PictureParser.Parse(new FormFileCollection { file, file2, file3 });

        Assert.That(pictures, Has.Count.EqualTo(3));
    }

    [Test]
    public void ParserRejectsInvalidFormat()
    {
        IFormFile file = new FormFile(new MemoryStream(), 0, 0, "Test", "Test.txt");

        List<PictureVm> pictures = PictureParser.Parse(new FormFileCollection { file });

        Assert.That(pictures, Is.Empty);
    }

    [Test]
    public void ParserSerializes()
    {
        byte[] data = { 0, 1, 2, 3, 4 };
        byte[] data2 = { 5, 6, 7, 8, 9 };

        var pictures = new List<Picture>
        {
            new()
            {
                PictureTitle = "title",
                PictureData = data
            },
            new()
            {
                PictureTitle = "title2",
                PictureData = data2
            }
        };

        List<string> serializedPictures = PictureParser.Serialize(pictures);

        Assert.Multiple(() =>
        {
            Assert.That(serializedPictures, Has.Count.EqualTo(2));
            Assert.That(serializedPictures[0], Is.EqualTo($"data:image/png;base64,{Convert.ToBase64String(data)}"));
            Assert.That(serializedPictures[1], Is.EqualTo($"data:image/png;base64,{Convert.ToBase64String(data2)}"));
        });
    }

    [Test]
    public void ParserSerializesRawData()
    {
        byte[] rawData = { 0, 1, 2, 3, 4 };

        var serializedData = PictureParser.SerializeData(rawData);

        Assert.That(serializedData, Is.EqualTo($"data:image/png;base64,{Convert.ToBase64String(rawData)}"));
    }
}