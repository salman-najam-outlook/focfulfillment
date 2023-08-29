using LocalDropshipping.Web.Enum;

namespace LocalDropshipping.Web.Data.Entities
{
	public class MemberShip
	{
		public int MembershipId { get; set; }
		public int UserId { get; set; }
		public DateTime DatetimeStartDate { get; set; }
		public DateTime DatetimeEndDate { get; set; }
		public PaymentStatus PaymentStatus { get; set; }
		public bool IsActive { get; set; }
		public int ApprovedBy { get; set; }
	}
}
