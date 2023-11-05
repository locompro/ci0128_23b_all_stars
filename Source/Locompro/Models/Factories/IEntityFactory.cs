namespace Locompro.Models.Factories;

public interface IEntityFactory<TD,TE> 
    where TD : class
    where TE : class
{
    TE FromDto(TD dto);
    
    TD ToDto(TE entity);
}