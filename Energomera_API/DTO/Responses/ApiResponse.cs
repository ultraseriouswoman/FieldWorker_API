using System.Net;

namespace Energomera_API.DTO.Responses
{
    public class ApiResponse : ApiResponse<object> // Объект по умолчанию
    {

    }
    public class ApiResponse<T> // Используется для документации результата
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success
        {
            get
            {
                return (int)StatusCode / 100 == 2;
            }
        }
        public List<string> ErrorMessages { get; set; } = [];
        public T? Result { get; set; }
    }
}
