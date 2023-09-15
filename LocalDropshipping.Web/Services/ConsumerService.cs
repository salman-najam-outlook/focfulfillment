using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly LocalDropshippingContext _context;

        public ConsumerService(LocalDropshippingContext context)
        {
            _context = context;
        }

        public List<Consumer?> GetAllConsumer()
        {
            return _context.Consumers.ToList();
        }
        public bool? AddNewConsumer(Consumer consumer)
        {
            try
            {
                _context.Consumers.Add(consumer);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool BlockOrUnblockConsumer(int userId)
        {
            try
            {
                var consumer = _context.Consumers.Find(userId);
                if (consumer != null && consumer.IsBlocked == false)
                {
                    consumer.IsBlocked = true;
                    _context.SaveChanges();
                }
                else
                {
                    consumer.IsBlocked = false;
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Consumer? GetById(int id)
        {
            return _context.Consumers.FirstOrDefault(x => x.Id == id);
        }
    }
}
