using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IWithdrawlsService withdrawalsService;

        public SellerController(IOrderService orderService, IWithdrawlsService withdrawalsService)
        {
            this.orderService = orderService;
            this.withdrawalsService = withdrawalsService;

        }

        #region Order


        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok(orderService.GetAll());
        }

        [HttpGet("id")]
        public IActionResult GetOrderById(int id)
        {
            return Ok(orderService.GetById(id));
        }

        [HttpPost]
        public IActionResult AddOrder(OrderDto orderDto)
        {
            Order order = orderDto.ToEntity();
            orderService.Add(order);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            orderService.Delete(id);
            return Ok();
        }

        [HttpPost("{id}")]
        public IActionResult UpdateOrder(int id, OrderDto orderDto)
        {
            Order order = orderDto.ToEntity();
            orderService.Update(id, orderDto);
            return Ok();
        }
        #endregion

        #region Withdrawal
        [HttpPost]
        public IActionResult RequestWithdrawal(WithdrawlsDto withdrawlsDto)
        {
            Withdrawals withdrawal = withdrawlsDto.ToEntity();
            withdrawalsService.RequestWithdrawal(withdrawal);
            return Ok(withdrawlsDto);
        }

        [HttpGet("{id}")]
        public IActionResult getWithdrawalById(int id)
        {
            return Ok(withdrawalsService.GetWithdrawalRequestsById(id));
        }

        [HttpGet("{email}")]
        public IActionResult getWithrawalByUserId(string email)
        {
            return Ok(withdrawalsService.GetWithdrawalRequestsByUserEmail(email));
        }

        [HttpPost]
        public IActionResult ProcessWithdrawal(ProcessWidrawalDto processDto)
        {
            var withdrawal = withdrawalsService.ProcessWithdrawal(processDto);
            if (withdrawal != null)
            {
                return Ok(withdrawal);
            }
            return NotFound();
        }
        #endregion


    }
}
