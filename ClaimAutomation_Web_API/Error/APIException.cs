namespace AppoinmentManagementPortal.API.Error
{
    public class APIException
    {
        public APIException(int statusCode,string message,string details)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; }
        public string Message { get; }
        public string Details { get; }
    }
}
