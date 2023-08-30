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


		public WishList Add(int UserId, int ProductId)
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
				var Wish = context.WishList.Find(WishListId);
				if (Wish != null)
				{
					Wish.IsActive = false;
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
