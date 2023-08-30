using LocalDropshipping.Web.Data.Entities;
using Newtonsoft.Json;

namespace LocalDropshipping.Web.Dtos
{
	public class SubscriptionsDto
	{
		public int UserId { get; set; }

		public int ApprovedBy { get; set; }

		//public DateTime CreatedDate { get; set; }

		//public int CreatedBy { get; set; }

		//public DateTime UpdatedDate { get; set; }
		//public int UpdatedBy { get; set; }


		internal Subscription ToEntity()
		{
			return JsonConvert.DeserializeObject<Subscription>(JsonConvert.SerializeObject(this))!;
		}

	}
}
