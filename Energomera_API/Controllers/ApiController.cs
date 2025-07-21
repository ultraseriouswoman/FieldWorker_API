using Energomera_API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Energomera_API.Controllers
{
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class ApiController: ControllerBase
    {
    }
}
