namespace Energomera_API.DTO.Responses
{
    public class FieldsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
        public LocationsResponse Locations { get; set; }
    }
}
