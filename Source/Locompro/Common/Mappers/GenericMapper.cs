namespace Locompro.Common.Mappers;

public abstract class GenericMapper<TD, TV> : IMapper<TD, TV>
    where TD : class
    where TV : class
{
    public TV ToVm(TD dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return BuildVm(dto);
    }

    public TD ToDto(TV vm)
    {
        if (vm == null)
        {
            throw new ArgumentNullException(nameof(vm));
        }

        return BuildDto(vm);
    }
    
    protected abstract TV BuildVm(TD dto);

    protected abstract TD BuildDto(TV vm);
}