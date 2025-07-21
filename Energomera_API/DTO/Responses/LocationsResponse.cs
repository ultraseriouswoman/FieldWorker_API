namespace Energomera_API.DTO.Responses
{
    public class LocationsResponse
    {
        public CoordinatesResponse Center { get; set; }
        public IEnumerable<CoordinatesResponse> Polygon { get; set; }
    }
}
