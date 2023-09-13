using LocalDropshipping.Web.Enums;

namespace LocalDropshipping.Web.Data.Entities
{
    public class Subscription
	{
		public int SubscriptionId { get; set; }
		public string UserId { get; set; }
        public string Email { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
		public SubscriptionStatus SubscriptionStatus { get; set; }
		public int ApprovedBy { get; set; }
		public DateTime ActivationDate { get; set; }
		public DateTime ExpiryDate { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}
