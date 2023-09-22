namespace LocalDropshipping.Web.Models
{
    public class SellerOrdersCounterViewModel
    {
        public int TotalDeliveredOrders { get; set; }
        public int PendingOrders { get; set; }
        public decimal PendingProfit { get; set; }
        public decimal TotalSales { get; set;}
    }
}
