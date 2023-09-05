using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LocalDropshipping.Web.Services
{
    public class WithdrawlsService : IWithdrawlsService
    {
        private readonly LocalDropshippingContext context;

        public WithdrawlsService(LocalDropshippingContext context)
        {
            this.context = context;
        }

        public Withdrawals GetWithdrawalRequestsById(int withdrawalId)
        {
            return context.Withdrawals.FirstOrDefault(x => x.WithdrawalId == withdrawalId);
        }

        public Withdrawals GetWithdrawalRequestsByUserId(int userId)
        {
            return context.Withdrawals.FirstOrDefault(x => x.UserId == userId);
        }

        public Withdrawals ProcessWidrawal(ProcessWidrawalDto processDto)
        {
            var withdrawal = context.Withdrawals.FirstOrDefault(x => x.WithdrawalId == processDto.WithdrawalId);
            if (withdrawal != null)
            {
                withdrawal.TransactionId = processDto.TransactionId;
                withdrawal.ProcessedBy = processDto.ProcessedBy;
                withdrawal.paymentStatus = PaymentStatus.UnPaid;
                withdrawal.UpdatedDate = DateTime.Now;

                context.SaveChanges();
            }
            return withdrawal;
        }

        public Withdrawals RequestWithdrawal(Withdrawals withdrawal)
        {
            if (withdrawal != null)
            {
                Withdrawals withdrawals = new Withdrawals
                {
                    AmountInPkr = withdrawal.AmountInPkr,
                    AccountTitle = withdrawal.AccountTitle,
                    AccountNumber = withdrawal.AccountNumber,
                    paymentStatus = withdrawal.paymentStatus = PaymentStatus.Paid,

                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                };
                context.Withdrawals.Add(withdrawals);
                context.SaveChanges();

                return withdrawals;
            }
            return null;

        }
    }
}
