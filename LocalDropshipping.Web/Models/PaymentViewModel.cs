using LocalDropshipping.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class PaymentViewModel
    {
        public int? WithdrawalId { get; set; }
        public string? CreatedBy { get; set; }
        public string? ProcessedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public string? Reason { get; set; }
    }
}
