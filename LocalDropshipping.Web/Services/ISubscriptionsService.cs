using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;

namespace LocalDropshipping.Web.Services
{
	public interface ISubscriptionsService
	{
		Subscription Add(Subscription memberShip);
		List<Subscription> GetAll();
		Subscription? GetById(int SubscriptionId);
		Subscription Delete(int SubscriptionId);
		Subscription UpdatePaymentStatus(int membershipId, PaymentStatus newPaymentStatus);
		Subscription UpdateSubscriptionStatus(int SubscriptionId);
	}
}
