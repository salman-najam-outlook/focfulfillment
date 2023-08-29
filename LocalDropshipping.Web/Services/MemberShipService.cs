using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enum;

namespace LocalDropshipping.Web.Services
{
	public class MemberShipService : IMemberShipService
	{
		private readonly LocalDropshippingContext context;

		public MemberShipService(LocalDropshippingContext context) 
		{
			this.context = context;
		}
		public MemberShip Add(MemberShip memberShip)
		{
			memberShip.IsActive = true;
			memberShip.DatetimeStartDate = DateTime.Now;
			memberShip.DatetimeEndDate = DateTime.Now.AddDays(30);
			memberShip.PaymentStatus = PaymentStatus.UnPaid;


			context.MemberShip.Add(memberShip);

			context.SaveChanges();
			return memberShip;
		}
		public MemberShip Delete(int MembershipId)
		{
			try
			{
				var MShip = context.MemberShip.Find(MembershipId);
				if (MShip != null)
				{
					MShip.IsActive = false;
					context.SaveChanges();
					return MShip;
				}
			}
			catch (Exception ex)
			{
			}
			return null;
		}
		public List<MemberShip> GetAll()
		{
			return context.MemberShip.Where(x => x.IsActive == true).ToList();
		}
		public MemberShip? GetById(int MembershipId)
		{
			return context.MemberShip.FirstOrDefault(m => m.MembershipId == MembershipId);
		}
		public MemberShip Update(int MembershipId, MemberShipDto memberShipDto)
		{
			var MShip = context.MemberShip.FirstOrDefault(x => x.MembershipId == MembershipId);
			if (MShip != null)
			{

				context.SaveChanges();
			}
			return MShip;
		}
		public MemberShip UpdatePaymentStatus(int MembershipId, PaymentStatus newPaymentStatus)
		{
			var MShip = context.MemberShip.FirstOrDefault(x => x.MembershipId == MembershipId);
			if (MShip != null)
			{
				MShip.PaymentStatus = newPaymentStatus;
				context.SaveChanges();
			}
			return MShip;
		}
		public MemberShip UpdateIsActive(int membershipId, bool newIsActive)
		{
			var MShip = context.MemberShip.FirstOrDefault(x => x.MembershipId == membershipId);
			if (MShip != null)
			{
				MShip.IsActive = newIsActive;
				context.SaveChanges();
			}
			return MShip;
		}
	}
}
