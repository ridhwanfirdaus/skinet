using System;

namespace API.Errors
{
    public class APIResponse
    {
        public APIResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageStatusCode(statusCode);
        }

        

        public int StatusCode {get; set;}

        public string Message{get; set;}

        private string GetDefaultMessageStatusCode(int statusCode)
        {
            return statusCode switch 
            {
                
                400 =>"Bad Request ",
                401 => "Not Authorized",
                404 => " No resource is found",
                500 => "Errors are found",

                _ => null
            };
        }
    }
}