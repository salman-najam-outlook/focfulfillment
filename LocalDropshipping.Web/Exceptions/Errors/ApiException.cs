namespace LocalDropshipping.Web.Exceptions.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, bool succeeded = false, string message = null, string details = null)
            : base(statusCode, message)
        {
            Succeeded = succeeded;
            Details = details;
        }

        public bool Succeeded { get; set; }
        public string Details { get; set; }
    }
}
