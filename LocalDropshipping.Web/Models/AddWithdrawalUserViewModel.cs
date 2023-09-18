using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Enums;

namespace LocalDropshipping.Web.Models
{
    public class AddWithdrawalUserViewModel
    {


        public string? Email { get; set; }
        public int AmountInPkr { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public List<Withdrawals> Withdrawals { get; set; }
        public List<User> User { get; set; }
        public List<Profiles> Profiles { get; set; }
        public string? ProcessedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? AccountTitle { get; set; }
        public string? BankAccountNumberOrIBAN { get; set; }
        public string? BankName { get; set; }
    }
}
