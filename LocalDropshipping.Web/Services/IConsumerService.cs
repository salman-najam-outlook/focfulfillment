using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Models;

namespace LocalDropshipping.Web.Services
{
    public interface IConsumerService
    {
        List<Consumer?> GetAllConsumer();
        bool? AddNewConsumer(Consumer consumer);
        Consumer? GetById(int id);
        bool BlockOrUnblockConsumer(int userId);
        Consumer AddConsumer(CheckoutViewModel viewModel, int orderId, string sellerEmail);
        bool CheckConsumer(string primaryPhone, string secondaryPhone);

    }
}
