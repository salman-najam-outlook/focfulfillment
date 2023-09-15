using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Models;

namespace LocalDropshipping.Web.Services
{
    public interface IConsumersService
    {
        Consumer AddConsumer(CheckoutViewModel viewModel, int orderId, string sellerEmail);
		bool CheckConsumer(string primaryPhone, string secondaryPhone);
	}
}
