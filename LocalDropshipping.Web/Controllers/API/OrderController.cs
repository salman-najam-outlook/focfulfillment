using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(orderService.GetAll());
        }

        [HttpGet("id")]
        public IActionResult GetById(int id)
        {
            return Ok(orderService.GetById(id));
        }

        [HttpPost]
        public IActionResult Add(OrderDto orderDto)
        {
            Order order = orderDto.ToEntity();
            orderService.Add(order);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            orderService.Delete(id);
            return Ok();
        }

        [HttpPost("{id}")]
        public IActionResult Update(int id, OrderDto orderDto)
        {
            Order order = orderDto.ToEntity();
            orderService.Update(id, orderDto);
            return Ok();
        }
    }
}
