using NetTopologySuite.Geometries;

namespace Locompro.Models.ViewModels;

public class MapVm
{
    public Point Location { get; set; }
    public long Distance { get; set; }
}