using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;

namespace LocalDropshipping.Web.Services
{
    public class SubscriptionsService : ISubscriptionsService
	{
		private readonly LocalDropshippingContext context;

		public SubscriptionsService(LocalDropshippingContext context) 
		{
			this.context = context;
		}
		public Subscription Add(Subscription Sub)
		{
			Sub.ActivationDate = DateTime.Now;
			Sub.ExpiryDate = DateTime.Now.AddDays(30);
			Sub.PaymentStatus = PaymentStatus.UnPaid;
			Sub.SubscriptionStatus = SubscriptionStatus.PendingForApproval;


			context.Subscriptions.Add(Sub);

			
			context.SaveChanges();
			return Sub;
		}
		public Subscription Delete(int SubscriptionId)
		{
			try
			{
				var Sub = context.Subscriptions.Find(SubscriptionId);
				if (Sub != null)
				{
					Sub.SubscriptionStatus = SubscriptionStatus.Expired;
					context.SaveChanges();
					return Sub;
				}
			}
			catch (Exception ex)
			{
			}
			return null;
		}
		public List<Subscription> GetAll()
		{
			return context.Subscriptions.Where(x => x.SubscriptionStatus == SubscriptionStatus.Approved).ToList();
		}
		public Subscription? GetById(int SubscriptionId)
		{
			return context.Subscriptions.FirstOrDefault(m => m.SubscriptionId == SubscriptionId);
		}
		
		public Subscription UpdatePaymentStatus(int SubscriptionId, PaymentStatus newPaymentStatus)
		{
			var Sub = context.Subscriptions.FirstOrDefault(x => x.SubscriptionId == SubscriptionId);
			if (Sub != null)
			{
				Sub.PaymentStatus = newPaymentStatus;
				context.SaveChanges();
			}
			return Sub;
		}


		public Subscription UpdateSubscriptionStatus(int SubscriptionId)
		{
			var Sub = context.Subscriptions.FirstOrDefault(x => x.SubscriptionId == SubscriptionId);
			if (Sub != null)
			{
				Sub.SubscriptionStatus = SubscriptionStatus.Approved;

				context.SaveChanges();
			}
			return Sub;
		}

	}
}
