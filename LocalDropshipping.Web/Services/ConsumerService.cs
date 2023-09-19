using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Models;

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

        public Consumer AddConsumer(CheckoutViewModel viewModel, int orderId, string sellerEmail)
        {
            var consumer = new Consumer
            {
                FullName = viewModel.FullName,
                PrimaryPhoneNumber = viewModel.PrimaryPhoneNumber,
                SecondaryPhoneNumber = viewModel.SecondaryPhoneNumber,
                Address = viewModel.Address,
                City = viewModel.City,
                OrderId = orderId, // Assign the OrderId
                SellerEmail = sellerEmail   // Assign the Seller
            };

            // Add the new consumer to the context and save changes to the database
            _context.Consumers.Add(consumer);
            _context.SaveChanges();
            return consumer;
        }

        public bool CheckConsumer(string primaryPhone, string secondaryPhone)
        {
            Consumer consumer = _context.Consumers.FirstOrDefault((c => (c.PrimaryPhoneNumber == primaryPhone || c.SecondaryPhoneNumber == secondaryPhone) && c.IsBlocked == true))!;
            if (consumer != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
