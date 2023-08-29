using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enum;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers.API
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class MemberShipController : ControllerBase
	{
		private readonly IMemberShipService memberShipService;

		public MemberShipController(IMemberShipService memberShipService) 
		{
			this.memberShipService = memberShipService;
		}
		[HttpGet]
		public IActionResult GetAll()

		{
			return Ok(memberShipService.GetAll());
		}
		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			return Ok(memberShipService.GetById(id));
		}
		[HttpPost]
		public IActionResult AddMemberShip(MemberShipDto memberShipDt)
		{
			MemberShip MShip = memberShipDt.ToEntity();
			memberShipService.Add(MShip);
			return Ok();
		}
		[HttpDelete("{id}")]
		public IActionResult DeleteMemberShip(int id)
		{
			memberShipService.Delete(id)
;
			return Ok();
		}
		[HttpPost("{id}" )]
		public IActionResult UpdatePaymentStatus(int id, PaymentStatus newPaymentStatus)
		{
			var updatedMembership = memberShipService.UpdatePaymentStatus(id, newPaymentStatus);

			if (updatedMembership != null)
			{
				return Ok();
			}
			else
			{
				return NotFound(); // Membership with the given ID was not found
			}
		}
		[HttpPost("{id}")]
		public IActionResult UpdateIsActive(int id, bool newIsActive)
		{
			var updatedMembership = memberShipService.UpdateIsActive(id, newIsActive);

			if (updatedMembership != null)
			{
				return Ok();
			}
			else
			{
				return NotFound(); // Membership with the given ID was not found
			}
		}
	}
}
