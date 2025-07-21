using Aspose.Gis.Common;

namespace Energomera_API.Models
{
    public class Locations
    {
        public Coordinates Center { get; set; }
        public ICollection<Coordinates> Polygon { get; set; }
    }
}
