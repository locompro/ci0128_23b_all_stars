using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Locompro.Pages.Shared;

/// <summary>
///     Base class for page models that depend on communication with the client and caching on local sessions
/// </summary>
public class BasePageModel : PageModel
{
    protected readonly IHttpContextAccessor HttpContextAccessor;
    protected ILogger Logger;

    public BasePageModel(ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
    {
        Logger = loggerFactory.CreateLogger(GetType());
        HttpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    ///     Gets data sent from browser to server
    ///     Data gets sent from browser in json format, it is then here deserialized
    /// </summary>
    /// <typeparam name="T"> type of class where it is expected for the data to be stored </typeparam>
    /// <returns> Object of given type </returns>
    protected async Task<T> GetDataSentByClient<T>()
    {
        try
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    ///     Serializes the given object to json
    /// </summary>
    /// <param name="objectToSerialize"> object that will be serialized </param>
    /// <typeparam name="T"></typeparam>
    /// <returns> serialized string in json format </returns>
    protected string GetJsonFrom<T>(T objectToSerialize)
    {
        // prevent the json serializer from looping infinitely
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        var json = JsonConvert.SerializeObject(objectToSerialize, settings);

        Response.ContentType = "application/json";

        return json;
    }

    /// <summary>
    ///     Stores the given data in a local user session specific instance
    /// </summary>
    /// <param name="objectToCache"> what is to be stored </param>
    /// <param name="key"> identifier to retrieve cached data </param>
    /// <remarks> if for some reason there is no session, one will be instantiated</remarks>
    protected void CacheDataInSession<T>(T objectToCache, string key)
    {
        if (HttpContextAccessor.HttpContext?.Session == null)
            HttpContextAccessor.HttpContext?.RequestServices.GetService(typeof(ISession));

        HttpContext.Session.SetString(key, JsonConvert.SerializeObject(objectToCache));
    }

    /// <summary>
    ///     Retrieves the cached data from the local user session specific instance
    /// </summary>
    /// <param name="key"> identifier to retrieve cached data </param>
    /// <param name="deletesData"> if the data and session are to be deleted </param>
    /// <typeparam name="T"> the type of the data to be retrieved</typeparam>
    /// <remarks>
    ///     if deletesData is true, the session is removed, but one is produced
    ///     by framework, or on a caching action if not automatic
    /// </remarks>
    /// <returns> cached data according to key</returns>
    protected T GetCachedDataFromSession<T>(string key, bool deletesData = true)
    {
        if (key == null || HttpContext.Session.GetString(key) == null) return default;

        var cachedObjectJson = HttpContext.Session.GetString(key);

        if (cachedObjectJson == null) return default;

        var cachedObject = JsonConvert.DeserializeObject<T>(cachedObjectJson);

        if (deletesData)
        {
            HttpContext.Session.Remove(key);
            HttpContextAccessor.HttpContext?.Session.Clear();
        }

        return cachedObject;
    }
}