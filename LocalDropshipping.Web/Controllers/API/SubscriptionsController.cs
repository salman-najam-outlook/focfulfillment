using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers.API
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SubscriptionsController : ControllerBase
	{
		private readonly ISubscriptionsService sub;

		public SubscriptionsController(ISubscriptionsService Sub) 
		{
			sub = Sub;
		}
		[HttpGet]
		public IActionResult GetAll()

		{
			return Ok(sub.GetAll());
		}
		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			return Ok(sub.GetById(id));
		}
		[HttpPost]
		public IActionResult AddSubscription(SubscriptionsDto subscriptionsDt)
		{
			Subscription Subb = subscriptionsDt.ToEntity();
			sub.Add(Subb);
			return Ok();
		}
		[HttpDelete("{id}")]
		public IActionResult DeleteSubscription(int id)
		{
			sub.Delete(id)
;
			return Ok();
		}
		[HttpPost("{id}" )]
		public IActionResult UpdatePaymentStatus(int id, PaymentStatus newPaymentStatus)
		{
			var updatedPStatus = sub.UpdatePaymentStatus(id, newPaymentStatus);

			if (updatedPStatus != null)
			{
				return Ok();
			}
			else
			{
				return NotFound();
			}
		}

		[HttpPost("{id}")]
		public IActionResult UpdateSubscriptionStatus(int id)
		{
			var updatedMembership = sub.UpdateSubscriptionStatus(id);

			if (updatedMembership != null)
			{
				return Ok();
			}
			else
			{
				return NotFound(); 
			}
		}

	}
}
