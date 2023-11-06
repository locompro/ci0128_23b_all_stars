using Locompro.Models.Entities;

namespace Locompro.Models.Dtos;
/// <summary>
///  Minimal json object to represent a canton
/// </summary>
public class CantonDto
{
    /// <summary>
    ///   Name of the canton
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    ///   Constructor based on a canton entity
    /// </summary>
    public CantonDto(Canton canton)
    {
        Name = canton.Name;
    }
}