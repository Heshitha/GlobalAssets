using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAsset.Models
{
    public class BuyPackageViewModel
    {
        public int UserID { get; set; }
        public int PackID { get; set; }
        public double Amount { get; set; }
        public string PaymentType { get; set; }
        public string TransactionID { get; set; }
        public string TransactionMail { get; set; }
    }

    public class DepositsHistoryViewModel
    {
        public DepositsHistoryViewModel()
        {
            ExpiredList = new List<DepositHistoryItem>();
            PendingList = new List<DepositHistoryItem>();
            ProgressingList = new List<DepositHistoryItem>();
            DeclinedList = new List<DepositHistoryItem>();
        }
        public List<DepositHistoryItem> PendingList { get; set; }
        public List<DepositHistoryItem> ProgressingList { get; set; }
        public List<DepositHistoryItem> DeclinedList { get; set; }
        public List<DepositHistoryItem> ExpiredList { get; set; }
    }

    public class DepositHistoryItem
    {
        public int DepositID { get; set; }
        public string PackageName { get; set; }
        public double Amount { get; set; }
        public string PurchasedOn { get; set; }
        public string ApprovedOn { get; set; }
        public string ExpiredOn { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; }
    }

    public class ViewDepositViewModel
    {
        public int UserPackID { get; set; }
        public string StatusText { get; set; }
        public int Progress { get; set; }
        public string PackColor { get; set; }
        public string PackName { get; set; }
        public string SubHeading { get; set; }
        public string InvestmentText { get; set; }
        public string Interest { get; set; }
        public string Duration { get; set; }
        public double DepositedAmount { get; set; }
        public string DepositedDate { get; set; }
        public string ApprovedDate { get; set; }
        public string ExpiredDate { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionID { get; set; }
        public string TransactionEmail { get; set; }
        public bool IsAuthoried { get; set; }
        public string CompanyEmail { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public string ReasonForDecline { get; set; }
    }

    public class ApprovePackageViewModel
    {
        public int UserID { get; set; }
        public int UPID { get; set; }
        public string ReasonForDecline { get; set; }
    }

    public class DashBoardViewModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageExt { get; set; }
        public double AccountBalance { get; set; }
        public double PendingWithdrawals { get; set; }
        public double SuccessfullWithdrawals { get; set; }
        public double AvailableBalance { get; set; }
        public List<DepositHistoryItem> RecentDeposits { get; set; }
        public List<WithdrawViewModelItem> RecentWithdrawals { get; internal set; }
    }

    public class PayoutViewModelItem
    {
        public int PayoutID { get; set; }
        public string PayoutDate { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }

    public class PayoutViewModel
    {
        public PayoutViewModel()
        {
            PayoutList = new List<PayoutViewModelItem>();
        }
        public List<PayoutViewModelItem> PayoutList { get; set; }
        public double TotalEarning { get; set; }
    }

    public class WithdrawViewModel
    {
        public WithdrawViewModel()
        {
            RecentWithdrawals = new List<WithdrawViewModelItem>();
        }
        public double TotalEarnings { get; set; }
        public double SuccessfullWithdraws { get; set; }
        public double PendingWithdraws { get; set; }
        public double AvailbleBalance { get; set; }
        public List<WithdrawViewModelItem> RecentWithdrawals { get; set; }
        public int UserID { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentEmail { get; set; }
    }

    public class WithdrawViewModelItem
    {
        public int WithdrawID { get; set; }
        public string RequestedDate { get; set; }
        public double Amount { get; set; }
        public string WithdrawMethod { get; set; }
        public string Status { get; set; }
        public string ProcessedDate { get; set; }
    }

    public class WithdrawalEmailViewModel
    {
        public int UserID { get; set; }
        public string PaymentType { get; set; }
    }

    public class WithdrawHistoryViewModel
    {
        public WithdrawHistoryViewModel()
        {
            PendingWithdrawals = new List<WithdrawViewModelItem>();
            ApprovedWithdrawals = new List<WithdrawViewModelItem>();
            DeclinedWithdrawals = new List<WithdrawViewModelItem>();
        }
        public List<WithdrawViewModelItem> PendingWithdrawals { get; set; }
        public List<WithdrawViewModelItem> ApprovedWithdrawals { get; set; }
        public List<WithdrawViewModelItem> DeclinedWithdrawals { get; set; }
    }

    public class ViewWithdrawalDetailsViewModel
    {
        public int WithdrawID { get; set; }
        public int ApprovingUserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public int UserID { get; set; }
        public string RequestedDate { get; set; }
        public double Amount { get; set; }
        public string PaymentType { get; set; }
        public string PaymentEmail { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public string SendersEmail { get; set; }
        public string ProcessedDate { get; set; }
        public string DeclineReason { get; set; }
        public bool isAdmin { get; set; }
    }

    public class FriendCircleViewModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public string JoinedDate { get; set; }
    }

    public class SendInvitationViewModel
    {
        public int UserID { get; set; }
        public string EmailAddressList { get; set; }
    }
}