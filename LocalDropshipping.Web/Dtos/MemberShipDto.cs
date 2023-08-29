using LocalDropshipping.Web.Data.Entities;
using Newtonsoft.Json;

namespace LocalDropshipping.Web.Dtos
{
	public class MemberShipDto
	{
		public int UserId { get; set; }

		public int ApprovedBy { get; set; }
		internal MemberShip ToEntity()
		{
			return JsonConvert.DeserializeObject<MemberShip>(JsonConvert.SerializeObject(this))!;
		}

	}
}
