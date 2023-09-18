using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace LocalDropshipping.Web.Services
{
	public class WishListService : IWishListService
	{
		private readonly LocalDropshippingContext context;

		public WishListService(LocalDropshippingContext context) 
		{
			this.context = context;
		}

		public List<WishList> GetAll()
		{
			return context.WishList.Where(x => x.IsActive == true).ToList();
		}

        public List<WishList> GetAllbyUserId(string UserId)
        {
            return context.WishList.Where(x => x.IsActive == true && x.UserId==UserId).ToList();
        }


        public bool ValidateWishlistProduct(string userId, int productId)
        {
            bool isProductInWishlist = context.WishList.Any(x => x.ProductId == productId && x.UserId == userId);
            if (isProductInWishlist)
            {
                return false;
            }

            return true;
        }

        public WishList Add(string UserId, int ProductId)
		{
			var wishList = new WishList
			{
				UserId = UserId,
				ProductId = ProductId,
				CreatedDate = DateTime.Now,
				IsActive = true
			};
			context.WishList.Add(wishList);
			context.SaveChanges();
			return wishList;
		}

		public WishList? GetById(int WishListId)
		{
			return context.WishList.FirstOrDefault(c => c.WishListId == WishListId && c.IsActive );
		}

		public WishList? Delete(int WishListId)
		{
			try
			{
				var Wish = context.WishList.FirstOrDefault(c => c.ProductId == WishListId);
				if (Wish != null)
				{
					//Wish.IsActive = false;
                    context.WishList.Remove(Wish);
                    context.SaveChanges();
					return Wish;
				}
			}
			catch (Exception ex)
			{
			}
			return null;

		}
	}
}
