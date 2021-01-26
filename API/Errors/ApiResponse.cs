using System;

namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode,string message=null)
        {
            this.StatusCode = statusCode;
            this.Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch{
            400 => "Bad Request you made",
            401 => "Authorized, you not",
            404 => "Not found what you want",
            500 => "Server made an error"
            };
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}