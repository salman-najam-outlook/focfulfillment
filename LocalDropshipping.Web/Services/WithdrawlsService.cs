using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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

        public Withdrawals GetWithdrawalRequestsByUserEmail(string userEmail)
        {
            return context.Withdrawals.FirstOrDefault(x => x.UserEmail == userEmail);
        }

        public Withdrawals ProcessWidrawal(ProcessWidrawalDto processDto)
        {
            var withdrawal = context.Withdrawals.FirstOrDefault(x => x.WithdrawalId == processDto.WithdrawalId);
            if (withdrawal != null)
            {
                withdrawal.TransactionId = processDto.TransactionId;
                withdrawal.ProcessedBy = processDto.ProcessedBy.ToString();
                //withdrawal.paymentStatus = PaymentStatus.UnPaid;
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
                    //paymentStatus = withdrawal.paymentStatus = PaymentStatus.Paid,

                    CreatedDate = DateTime.Now,
                    CreatedBy = "1",
                };
                context.Withdrawals.Add(withdrawals);
                context.SaveChanges();

                return withdrawals;
            }
            return null;

        }

        public List<Withdrawals?> GetAll()
        {
            //var userEmail = context.Users.Select(x=>x.Email).ToList();
            var withdrawal = new List<Withdrawals?>();
            withdrawal = context.Withdrawals.ToList();
            if (withdrawal != null)
            {
                return withdrawal;
            }
            return new List<Withdrawals>();
        }

        public Withdrawals UpdateWithDrawal(PaymentViewModel withdrawal)
        {
            // Attach the entity to the context
            var attachedWithdrawal = context.Withdrawals.Attach(new Withdrawals { WithdrawalId = withdrawal.WithdrawalId });

            // Mark specific properties as modified
            attachedWithdrawal.Entity.UpdateBy = withdrawal.UpdatedBy;
            attachedWithdrawal.Entity.ProcessedBy = withdrawal.ProcessedBy;
            attachedWithdrawal.Entity.paymentStatus = withdrawal.PaymentStatus;
            attachedWithdrawal.Entity.Reason = withdrawal.Reason;
            attachedWithdrawal.Entity.TransactionId = withdrawal.TransactionId;
            attachedWithdrawal.Entity.UpdatedDate = DateTime.Now;

            context.SaveChanges();

            return attachedWithdrawal.Entity;
        }

        public bool UpdateWithpaymentStatus(PaymentStatus paymentStatus, int WithdrawalId)
        {
            var attachedWithdrawal = context.Withdrawals.Attach(new Withdrawals { WithdrawalId = WithdrawalId });
            attachedWithdrawal.Entity.paymentStatus = paymentStatus;
            context.SaveChanges();
            return true;
        }
    }
}
