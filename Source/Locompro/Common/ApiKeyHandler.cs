namespace Locompro.Common;

public class ApiKeyHandler: IApiKeyHandler
{
    private readonly string _apiKey;

    public ApiKeyHandler (string apiKey)
    {
        _apiKey = apiKey;
    }
    
    public string GetApiKey()
    {
        return _apiKey;
    }
}