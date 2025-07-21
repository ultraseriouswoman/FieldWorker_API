using Aspose.Gis.Common;
using Aspose.Gis.Geometries;
using AutoMapper;
using Energomera_API.DTO.Requests;
using Energomera_API.DTO.Responses;
using Energomera_API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Energomera_API.Controllers
{
    [Route("api/fields")]
    public class FieldsController(IMapper mapper): ApiController
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<FieldsResponse>>>> GetAllFields()
        {
            var result = mapper.Map<IEnumerable<FieldsResponse>>(KmlReader.GetDataToList(Paths.FieldPath, Paths.CentroidPath));
            return new ApiResponse<IEnumerable<FieldsResponse>>()
            {
                Result = result,
                StatusCode = HttpStatusCode.OK,
            };
        }

        [HttpGet("{field_id:int}")]
        public async Task<ActionResult<ApiResponse<SizeFromIDResponse>>> GetSizeFromID(int field_id)
        {
            var response = new ApiResponse<SizeFromIDResponse>();
            if (field_id == 0)
            {
                response.ErrorMessages.Add("Необходимо ввести номер участка");
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            var entity = KmlReader.GetDataFromID(field_id, Paths.FieldPath, Paths.CentroidPath) ?? null;
            if (entity == null || entity.Id == 0)
            {
                response.ErrorMessages.Add("Указанный номер участка не существует в реестре");
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }
            var result = new SizeFromIDResponse()
            {
                Size = entity.Size
            };

            response.Result = result;
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }

        [HttpPost("/{field_id:int}/getdistance")]
        public async Task<ActionResult<ApiResponse<DistanceFromCenterToPointResponse>>> GetDistance(int field_id, 
            [FromBody] CustomCoordinateRequest request)
        {
            var response = new ApiResponse<DistanceFromCenterToPointResponse>();
            if (field_id != 0)
            {
                var point = new Coordinate(request.Lat, request.Lng);
                var center = new Coordinate(
                    KmlReader.GetDataFromID(field_id, Paths.FieldPath, Paths.CentroidPath).Locations.Center.Lat,
                    KmlReader.GetDataFromID(field_id, Paths.FieldPath, Paths.CentroidPath).Locations.Center.Lng);

                var result = new DistanceFromCenterToPointResponse()
                {
                    Distance = center.DistanceTo(point) * Math.Pow(10, 2)
                };

                response.Result = result;
                response.StatusCode= HttpStatusCode.OK;

                return response;
            }

            response.ErrorMessages.Add("Указанный номер участка не существует в реестре");
            response.StatusCode = HttpStatusCode.NotFound;

            return NotFound(response);
        }

        [HttpPost("/checkispointinanypolygon")]
        public async Task<ActionResult<ApiResponse<CheckIsPointInAnyPolygonReponse>>> CheckIsPointInAnyPolygon(
            [FromBody] CustomCoordinateRequest request)
        {
            var result = new CheckIsPointInAnyPolygonReponse();
            result.Result = false;

            var point = new Point(request.Lat, request.Lng);

            var polygons = KmlReader.GetAllPolygons(Paths.FieldPath);

            foreach (var polygon in polygons)
            {
                if (polygon.ToLinearGeometry().ToLinearGeometry().Touches(point))
                {
                    result.Field_Id = KmlReader.GetFieldFromPolygon(polygon).Id;
                    result.Field_Name = KmlReader.GetFieldFromPolygon(polygon).Name;
                    result.Result = true;
                    break;
                }
            }

            return new ApiResponse<CheckIsPointInAnyPolygonReponse>()
            { 
                Result = result,
                StatusCode = HttpStatusCode.OK
            };

        }
    }
}
