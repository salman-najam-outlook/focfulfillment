using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Enums;

namespace LocalDropshipping.Web.Models
{
    public class WithdrawalModel
    {
        public decimal AmountInPkr { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string TransactionId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Reason { get; set; }
    }
}
