namespace LocalDropshipping.Web.Helpers
{
    public class PageResponse<T> : Response<T>
    {
        public PageResponse(T data, int pageNumber, int pageSize, int totalCount)
        {
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
            this.Data = data;
            this.totalCount = totalCount;
            //this.Search = search;

        }

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        //public string Search { get; set; }
    }
}
