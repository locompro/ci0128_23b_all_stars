namespace Locompro.Models.Factories;

/// <summary>
/// Defines a factory interface for converting between data transfer objects (DTOs) and entity models.
/// </summary>
/// <typeparam name="TD">The type of the data transfer object.</typeparam>
/// <typeparam name="TE">The type of the entity model.</typeparam>
public interface IEntityFactory<TD, TE>
    where TD : class
    where TE : class
{
    /// <summary>
    /// Converts a DTO to its corresponding entity model.
    /// </summary>
    /// <param name="dto">The DTO to convert to an entity model.</param>
    /// <returns>An entity model that corresponds to the provided DTO.</returns>
    TE FromDto(TD dto);

    /// <summary>
    /// Converts an entity model to its corresponding DTO.
    /// </summary>
    /// <param name="entity">The entity model to convert to a DTO.</param>
    /// <returns>A DTO that corresponds to the provided entity model.</returns>
    TD ToDto(TE entity);
}
