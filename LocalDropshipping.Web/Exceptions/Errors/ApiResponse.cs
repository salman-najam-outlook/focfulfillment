using LocalDropshipping.Web.Helpers.Constants;

namespace LocalDropshipping.Web.Exceptions.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? DefaultApiResponse(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string DefaultApiResponse(int statusCode)
        {
            return statusCode switch
            {
                400 => Constants.ErrorMessage._400,
                401 => Constants.ErrorMessage._401,
                404 => Constants.ErrorMessage._404,
                500 => Constants.ErrorMessage._500,
                _ => null
            };
        }

    }
}
