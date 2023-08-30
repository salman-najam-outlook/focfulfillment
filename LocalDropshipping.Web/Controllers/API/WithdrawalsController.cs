using Azure.Core;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WithdrawalsController : ControllerBase
    {
        private readonly IWithdrawlsService withdrawlsService;

        public WithdrawalsController(IWithdrawlsService withdrawlsService)
        {
            this.withdrawlsService = withdrawlsService;
        }

        [HttpPost]
        public IActionResult RequestWithdrawal(WithdrawlsDto withdrawlsDto)
        {
            Withdrawals withdrawal = withdrawlsDto.ToEntity();
            withdrawlsService.RequestWithdrawal(withdrawal);
            return Ok(withdrawlsDto);
        }

        [HttpGet("{id}")]
        public IActionResult getById(int id)
        {
            return Ok(withdrawlsService.GetWithdrawalRequestsById(id));
        }
        [HttpGet("{id}")]
        public IActionResult getWithrawalByUserId(int id)
        {
            return Ok(withdrawlsService.GetWithdrawalRequestsByUserId(id));
        }

        [HttpPost]
        public IActionResult ProcessWidrawal(ProcessWidrawalDto processDto)
        {
            var withdrawal = withdrawlsService.ProcessWidrawal(processDto);
            if (withdrawal != null)
            {
                return Ok(withdrawal);
            }
            return NotFound();
        }
    }
}
