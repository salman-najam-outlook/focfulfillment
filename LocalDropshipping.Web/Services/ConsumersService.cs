using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LocalDropshipping.Web.Services
{
    public class ConsumersService : IConsumersService
    {
        private readonly LocalDropshippingContext _context;

        public ConsumersService(LocalDropshippingContext context)
        {
            _context = context;
        }
        //public async Task AddConsumerAsync(CheckoutViewModel viewModel, int orderId, string sellerEmail)
        //{
        //    // Map the properties from the view model to a new Consumer entity
        //    var consumer = new Consumer
        //    {
        //        FullName = viewModel.FullName,
        //        PrimaryPhoneNumber = viewModel.PrimaryPhoneNumber,
        //        SecondaryPhoneNumber = viewModel.SecondaryPhoneNumber,
        //        Address = viewModel.Address,
        //        City = viewModel.City,
        //        OrderId = orderId, // Assign the OrderId
        //        SellerEmail = sellerEmail   // Assign the Seller
        //    };

        //    // Add the new consumer to the context and save changes to the database
        //    _context.Consumers.Add(consumer);
        //    await _context.SaveChangesAsync();
        //}

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
            Consumer consumer=_context.Consumers.FirstOrDefault((c=>(c.PrimaryPhoneNumber == primaryPhone || c.SecondaryPhoneNumber==secondaryPhone) && c.IsBlocked==true ))!;
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
