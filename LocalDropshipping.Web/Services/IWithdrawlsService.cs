using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;

namespace LocalDropshipping.Web.Services
{
    public interface IWithdrawlsService
    {
        Withdrawals RequestWithdrawal(Withdrawals withdrawal);
        Withdrawals GetWithdrawalRequestsById(int withdrawalId);
        Withdrawals GetWithdrawalRequestsByUserId(int userId);
        Withdrawals ProcessWidrawal(ProcessWidrawalDto processDto);

    }
}
