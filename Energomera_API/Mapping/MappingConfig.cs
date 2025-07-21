using AutoMapper;
using Energomera_API.DTO.Responses;
using Energomera_API.Models;

namespace Energomera_API.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Fields, FieldsResponse>();

            CreateMap<Locations, LocationsResponse>();

            CreateMap<Coordinates, CoordinatesResponse>();
        }
    }
}
