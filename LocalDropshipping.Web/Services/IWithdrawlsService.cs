﻿using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Models;

namespace LocalDropshipping.Web.Services
{
    public interface IWithdrawlsService
    {
        Withdrawals RequestWithdrawal(Withdrawals withdrawal);
        Withdrawals GetWithdrawalRequestsById(int withdrawalId);
        Withdrawals GetWithdrawalRequestsByUserEmail(string userEmail);
        Withdrawals ProcessWidrawal(ProcessWidrawalDto processDto);
        List<Withdrawals?> GetAll();
        Withdrawals UpdateWithDrawal(PaymentViewModel withdrawal);
        bool UpdateWithpaymentStatus(PaymentStatus paymentStatus,int WithdrawalId);
    }
}
