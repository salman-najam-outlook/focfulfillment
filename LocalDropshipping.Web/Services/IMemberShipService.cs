using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enum;

namespace LocalDropshipping.Web.Services
{
	public interface IMemberShipService
	{
		MemberShip Add(MemberShip memberShip);

		List<MemberShip> GetAll();

		MemberShip? GetById(int MembershipId);

		MemberShip Delete(int MembershipId);
		MemberShip UpdatePaymentStatus(int membershipId, PaymentStatus newPaymentStatus);
		MemberShip UpdateIsActive(int membershipId, bool newIsActive);



	}
}
