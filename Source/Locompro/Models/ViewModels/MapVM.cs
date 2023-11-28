using NetTopologySuite.Geometries;

namespace Locompro.Models.ViewModels;

public class MapVm
{
    public const int Ratio = 100;

    private const int DistanceRatio = 1;
    public Point Location { get; set; }
    public long Distance { get; set; }
    
    public MapVm(double latitude, double longitude, long distance)
    {
        if (latitude == 0 && longitude == 0)
        {
            Location = null;
            Distance = 0;
            return;
        }
        
        Location = new Point(latitude, longitude) { SRID = 4326 };
        Distance = distance * DistanceRatio;
    }
}