using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
	public interface IWishListService
	{
		WishList Add(int UserId ,int ProductId);

		List<WishList> GetAll();

		WishList? GetById(int WishListId);

		WishList Delete(int WishListId);
	}
}
