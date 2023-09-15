using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
	public interface IWishListService
	{
		WishList Add(string UserId ,int ProductId);

		List<WishList> GetAll();
		List<WishList> GetAllbyUserId(string UserId);

        WishList? GetById(int WishListId);

		WishList Delete(int WishListId);
		bool ValidateWishlistProduct(string userId, int productId);

    }
}
