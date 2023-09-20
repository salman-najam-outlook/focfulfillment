using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LocalDropshipping.Web.Services
{
    public class WithdrawlsService : IWithdrawlsService
    {
        private readonly LocalDropshippingContext _context;

        public WithdrawlsService(LocalDropshippingContext context)
        {
            _context = context;
        }


        public Withdrawals GetWithdrawalRequestsById(int withdrawalId)
        {
            return _context.Withdrawals.FirstOrDefault(x => x.WithdrawalId == withdrawalId);
        }

        public Withdrawals GetWithdrawalRequestsByUserEmail(string userEmail)
        {
            return _context.Withdrawals.FirstOrDefault(x => x.UserEmail == userEmail);
        }

        public Withdrawals ProcessWithdrawal(ProcessWidrawalDto processDto)
        {
            var withdrawal = _context.Withdrawals.FirstOrDefault(x => x.WithdrawalId == processDto.WithdrawalId);
            if (withdrawal != null)
            {
                withdrawal.TransactionId = processDto.TransactionId;
                withdrawal.ProcessedBy = processDto.ProcessedBy.ToString();
                //withdrawal.paymentStatus = PaymentStatus.UnPaid;
                withdrawal.UpdatedDate = DateTime.Now;

                _context.SaveChanges();
            }
            return withdrawal;
        }

        public bool WithdrawalRequest(string email)
        {
            if (email != null)
            {
                Withdrawals withdrawals = new Withdrawals
                {
                    UserEmail = email,
                    AmountInPkr = 5000,
                    PaymentStatus = PaymentStatus.Processing,
                    CreatedDate = DateTime.Now,
                    CreatedBy = email,
                };
                _context.Withdrawals.Update(withdrawals);
                var result = _context.SaveChanges();
                if(result > 0) return true;
            }
            return false;

        }

        public List<Withdrawals?> GetAll()
        {
            //var userEmail = context.Users.Select(x=>x.Email).ToList();
            var withdrawal = new List<Withdrawals?>();
            withdrawal = _context.Withdrawals.ToList();
            if (withdrawal != null)
            {
                return withdrawal;
            }
            return new List<Withdrawals>();
        }
        public Withdrawals UpdateWithdrawal(PaymentViewModel withdrawal)
        {
            Withdrawals result = (from p in _context.Withdrawals
                             where p.WithdrawalId == withdrawal.WithdrawalId
                                  select p).SingleOrDefault();

            result.UpdateBy = withdrawal.UpdatedBy;
            result.ProcessedBy = withdrawal.ProcessedBy;
            result.PaymentStatus = withdrawal.PaymentStatus;
            result.TransactionId = withdrawal.TransactionId;
            result.UpdatedDate = DateTime.Now;
            result.Reason = withdrawal.Reason;

            _context.SaveChanges();

            return result;
        }

        public bool UpdateWithpaymentStatus(PaymentStatus paymentStatus, int WithdrawalId)
        {
            var attachedWithdrawal = _context.Withdrawals.Attach(new Withdrawals { WithdrawalId = WithdrawalId });
            attachedWithdrawal.Entity.PaymentStatus = paymentStatus;
            _context.SaveChanges();
            return true;
        }

        public List<Withdrawals> GetWithdrawalByUserEmail(string email)
        {
            var withdrawals = _context.Withdrawals.Where(x => x.UserEmail == email).ToList();
            return withdrawals;
        }
    }
}
