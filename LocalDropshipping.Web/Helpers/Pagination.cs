using LocalDropshipping.Web.Enums;

namespace LocalDropshipping.Web.Helpers
{
    public class Pagination
    {
        const int maxPageSize = 30;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string search { get; set; }
        public string PaymentStatus { get; set; } = "All";
        public DateTime? From { get; set; } = DateTime.MinValue;
        public DateTime? To { get; set; } =DateTime.MinValue;
    }
}
