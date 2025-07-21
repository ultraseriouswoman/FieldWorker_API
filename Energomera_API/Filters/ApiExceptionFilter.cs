using Energomera_API.DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Energomera_API.Filters
{
    public class ApiExceptionFilter: IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;
        protected ApiResponse _response;

        public ApiExceptionFilter(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _response = new ApiResponse();
        }

        public void OnException(ExceptionContext context)
        {
            // Не отображает исключения, пока приложение запущено в Debug
            if (_hostEnvironment.IsDevelopment())
            {
                _response.ErrorMessages.Add(context.Exception.ToString());
            }
            else
            {
                _response.ErrorMessages.Add("An Internal Error occurred");
            }
            _response.StatusCode = HttpStatusCode.InternalServerError;
            context.Result = new OkObjectResult(_response);
        }
    }
}
