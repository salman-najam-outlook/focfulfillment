using LocalDropshipping.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Data.Entities
{
    public class Withdrawals
    {
        [Key]
        public int WithdrawalId { get; set; }
        public string? UserEmail { get; set; }
        public int AmountInPkr { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public string? ProcessedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdateBy { get; set; }
        public string? Reason { get; set; }
    }
}
