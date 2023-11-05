namespace Locompro.Common.Mappers;

public interface IMapper<TD, TV> 
    where TD : class
    where TV : class
{
    TV ToVm(TD dto);
    
    TD ToDto(TV vm);
}