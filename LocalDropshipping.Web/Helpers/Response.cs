namespace LocalDropshipping.Web.Helpers
{
    public class Response<T>
    {
        public Response() { }
        public Response(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
        public T Data { get; set; }
    }
}
