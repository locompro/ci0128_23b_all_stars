namespace Locompro.Models.Factories;

public abstract class GenericEntityFactory<TD, TE> : IEntityFactory<TD, TE>
    where TD : class
    where TE : class
{
    public TE FromDto(TD dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        return BuildEntity(dto);
    }

    public TD ToDto(TE entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return BuildDto(entity);
    }

    protected abstract TE BuildEntity(TD dto);

    protected abstract TD BuildDto(TE entity);
}