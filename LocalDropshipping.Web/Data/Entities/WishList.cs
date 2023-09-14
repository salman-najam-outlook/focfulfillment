using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalDropshipping.Web.Data.Entities
{
	public class WishList
	{
		public int WishListId { get; set; }
		public string UserId { get; set; }
		public int ProductId { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsActive { get; set; }
    }
}
