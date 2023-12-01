using Locompro.Common;

namespace Locompro.Tests.Common;

[TestFixture]
public class ApiKeyHandlerTests
{
    [Test]
    public void Constructor_WhenCalled_SetsApiKey()
    {
        // Arrange
        var expectedApiKey = "testApiKey";

        // Act
        var apiKeyHandler = new ApiKeyHandler(expectedApiKey);

        // Use reflection to get the private field _apiKey
        var apiKeyField = typeof(ApiKeyHandler).GetField("_apiKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var actualApiKey = (string)apiKeyField.GetValue(apiKeyHandler);

        // Assert
        Assert.That(actualApiKey, Is.EqualTo(expectedApiKey));
    }

    [Test]
    public void GetApiKey_WhenCalled_ReturnsCorrectApiKey()
    {
        // Arrange
        var expectedApiKey = "testApiKey123";
        var apiKeyHandler = new ApiKeyHandler(expectedApiKey);

        // Act
        var actualApiKey = apiKeyHandler.GetApiKey();

        // Assert
        Assert.That(actualApiKey, Is.EqualTo(expectedApiKey));
    }
}