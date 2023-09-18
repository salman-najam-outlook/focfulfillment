using LocalDropshipping.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Data.Entities
{
    public class Withdrawals
    {
        [Key]
        public int WithdrawalId { get; set; }
        public string UserId { get; set; }
        public int AmountInPkr { get; set; }
        public string AccountTitle { get; set; }
        public string AccountNumber { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public string? ProcessedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? UpdateBy { get; set; }
        public string? AddReason { get; set; }  
    }
}
