namespace Locompro.Models.Factories;

/// <summary>
/// Provides a generic factory for converting between data transfer objects (DTOs) and entity models.
/// </summary>
/// <typeparam name="TD">The type of the data transfer object.</typeparam>
/// <typeparam name="TE">The type of the entity model.</typeparam>
public abstract class GenericEntityFactory<TD, TE> : IEntityFactory<TD, TE>
    where TD : class
    where TE : class
{
    /// <summary>
    /// Creates an entity from a data transfer object.
    /// </summary>
    /// <param name="dto">The data transfer object to convert from.</param>
    /// <returns>An entity model corresponding to the DTO.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided DTO is null.</exception>
    public TE FromDto(TD dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        return BuildEntity(dto);
    }

    /// <summary>
    /// Creates a data transfer object from an entity.
    /// </summary>
    /// <param name="entity">The entity to convert from.</param>
    /// <returns>A DTO corresponding to the entity model.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
    public TD ToDto(TE entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return BuildDto(entity);
    }

    /// <summary>
    /// When implemented in a derived class, constructs an entity from the provided DTO.
    /// </summary>
    /// <param name="dto">The DTO to use for constructing the entity.</param>
    /// <returns>The constructed entity model.</returns>
    protected abstract TE BuildEntity(TD dto);

    /// <summary>
    /// When implemented in a derived class, constructs a DTO from the provided entity.
    /// </summary>
    /// <param name="entity">The entity to use for constructing the DTO.</param>
    /// <returns>The constructed data transfer object.</returns>
    protected abstract TD BuildDto(TE entity);
}
