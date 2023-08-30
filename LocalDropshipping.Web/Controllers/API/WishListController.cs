using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers.API
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class WishListController : ControllerBase
	{
		private readonly IWishListService wishListService;

		public WishListController(IWishListService wishListService) 
		{
			this.wishListService = wishListService;
		}

		[HttpGet]
		public IActionResult GetAll()

		{
			return Ok(wishListService.GetAll());
		}


		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			return Ok(wishListService.GetById(id));
		}



		[HttpPost("{UserId}/{ProductId}")]
		public IActionResult AddToWishList(int UserId, int ProductId)
		{
			var wishList = wishListService.Add(UserId, ProductId);

			return Ok();
		}
		[HttpDelete("{id}")]
		public IActionResult DeleteFromWishlist(int id)
		{
			wishListService.Delete(id)
;
			return Ok();
		}

	}
}
