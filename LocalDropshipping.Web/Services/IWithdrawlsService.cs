using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;

namespace LocalDropshipping.Web.Services
{
    public interface IWithdrawlsService
    {
        Withdrawals RequestWithdrawal(Withdrawals withdrawal);
        Withdrawals GetWithdrawalRequestsById(int withdrawalId);
        Withdrawals GetWithdrawalRequestsByUserId(string userId);
        Withdrawals ProcessWidrawal(ProcessWidrawalDto processDto);

        List<Withdrawals?> GetAll();

    }
}
