namespace Locompro.Models;

/// <summary>
/// Users qualified to be a moderator.
/// Result of a procedure call.
/// </summary>
public class GetQualifiedUserIDsResult
{
    /// <summary>
    /// The ID of one of the qualified users.
    /// </summary>
    public string Id { get; set; }
}