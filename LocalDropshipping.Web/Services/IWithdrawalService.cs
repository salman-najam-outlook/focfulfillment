using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Helpers;
using LocalDropshipping.Web.Models;

namespace LocalDropshipping.Web.Services
{
    public interface IWithdrawalService
    {
        bool WithdrawalRequest(string email);
        Withdrawals GetWithdrawalRequestsById(int withdrawalId);
        Withdrawals GetWithdrawalRequestsByUserEmail(string userEmail);
        Withdrawals ProcessWithdrawal(ProcessWidrawalDto processDto);
        List<Withdrawals?> GetAll();
        Withdrawals UpdateWithdrawal(PaymentViewModel withdrawal);
        bool UpdateWithpaymentStatus(PaymentStatus paymentStatus,int WithdrawalId);
        List<Withdrawals> GetWithdrawalByUserEmail(string email);
        List<Withdrawals> GetFilteredWithdrawals(Pagination pagination);
    }
}
