using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public interface IConsumerService
    {
        List<Consumer?> GetAllConsumer();
        bool? AddNewConsumer(Consumer consumer);
        Consumer? GetById(int id);
        bool BlockOrUnblockConsumer(int userId);

    }
}
