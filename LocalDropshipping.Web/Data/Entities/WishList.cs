using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Data.Entities
{
	public class WishList
	{
		public int WishListId { get; set; }
		public int UserId { get; set; }
		public int ProductId { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsActive { get; set; }

	}
}
