using GlobalAsset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GlobalAsset.Controllers.API
{
    public class BackEndAPIController : ApiController
    {
        GlobalAssetsDBDataContext db = new GlobalAssetsDBDataContext();

        [HttpPost]
        public bool BuyPackage([FromBody]BuyPackageViewModel obj)
        {
            bool retVal = false;
            try
            {
                Models.User usr = db.Users.Where(x => x.UserID == obj.UserID).FirstOrDefault();
                Models.Package pck = db.Packages.Where(x => x.PackageID == obj.PackID).FirstOrDefault();

                UserPackage usrPck = new UserPackage()
                {
                    User = usr,
                    Package = pck,
                    Amount = obj.Amount,
                    PaymentType = obj.PaymentType,
                    RecipientEmail = obj.PaymentType == "Neteller" ? "accounts@cryptinvestmentstech.com" : (obj.PaymentType == "Skrill" ? "skrillpayments@cryptinvestmentstech.com" : ""),
                    RequestedDate = DateTime.Now,
                    SendersEmail = obj.TransactionMail,
                    TransactionID = obj.TransactionID,
                    Status = Convert.ToInt32(PackageStatus.PendingApproval)
                };

                db.UserPackages.InsertOnSubmit(usrPck);
                db.SubmitChanges();
                UtilityFunctions.SendPurchaseCompletedEmail(usr, pck, usrPck);
                retVal = true;
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }

        [HttpPost]
        public bool ApprovePackage([FromBody]ApprovePackageViewModel obj)
        {
            bool retVal = false;

            try
            {
                Models.User usr = db.Users.Where(x => x.UserID == obj.UserID).FirstOrDefault();
                if(usr != null)
                {
                    if(usr.UserType.Value == Convert.ToInt32(UserTypes.Admin))
                    {
                        Models.UserPackage up = db.UserPackages.Where(x => x.UserPackageID == obj.UPID && x.Status.Value  == Convert.ToInt32(PackageStatus.PendingApproval)).FirstOrDefault();
                        if(up != null)
                        {
                            up.AcknowledgedDate = DateTime.Now;
                            up.Status = Convert.ToInt32(PackageStatus.ProcessedAndStarted);
                            up.AcknowledgeBy = usr.UserID;
                            db.SubmitChanges();
                            UtilityFunctions.SendPurchaseVerificationEmail(up.User, up.Package, up);
                            retVal = true;

                            if (!up.Package.PerDayPackage.Value)
                            {
                                double profit = 0;
                                profit = (up.Package.Interest.Value / 100.0) * up.Amount.Value;
                                Payout pay = new Payout()
                                {
                                    Amount = profit,
                                    CreatedDate = DateTime.Now,
                                    ActivatedDate = DateTime.Now.AddDays(up.Package.Duration.Value),
                                    Description = "Package " + up.Package.Name + " #"+up.UserPackageID + " Payout",
                                    User = up.User,
                                    UserPackage = up
                                };
                                db.Payouts.InsertOnSubmit(pay);
                                db.SubmitChanges();
                            }
                            else
                            {
                                List<Payout> lstPayout = new List<Payout>();
                                for(int i = 0; i < up.Package.Duration.Value; i++)
                                {
                                    lstPayout.Add(new Payout()
                                    {
                                        Amount = up.Package.PerDayAmount.Value,
                                        CreatedDate = DateTime.Now,
                                        ActivatedDate = DateTime.Now.AddDays(i+1),
                                        Description = "Daily Package Payout " + up.Package.Name + " #" + up.UserPackageID,
                                        User = up.User,
                                        UserPackage = up
                                    });
                                }
                                db.Payouts.InsertAllOnSubmit(lstPayout);
                                db.SubmitChanges();
                            }

                            if (up.User.RefferedBy.Value != 0)
                            {
                                User refUser = db.Users.Where(x => x.UserID == up.User.RefferedBy.Value).FirstOrDefault();
                                if(refUser != null)
                                {
                                    double commission = 0;
                                    commission = up.Amount.Value * 0.05;
                                    Payout pay = new Payout()
                                    {
                                        Amount = commission,
                                        CreatedDate = DateTime.Now,
                                        ActivatedDate = DateTime.Now.AddDays(28),
                                        Description = "Commission for Package " + up.Package.Name + " #" + up.UserPackageID + " deposited by " + up.User.UserName,
                                        User = refUser,
                                        UserPackage = up
                                    };
                                    db.Payouts.InsertOnSubmit(pay);
                                    db.SubmitChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return retVal;
        }

        [HttpPost]
        public bool DeclinePackage([FromBody]ApprovePackageViewModel obj)
        {
            bool retVal = false;

            try
            {
                Models.User usr = db.Users.Where(x => x.UserID == obj.UserID).FirstOrDefault();
                if (usr != null)
                {
                    if (usr.UserType.Value == Convert.ToInt32(UserTypes.Admin))
                    {
                        Models.UserPackage up = db.UserPackages.Where(x => x.UserPackageID == obj.UPID && x.Status.Value == Convert.ToInt32(PackageStatus.PendingApproval)).FirstOrDefault();
                        if (up != null)
                        {
                            up.AcknowledgedDate = DateTime.Now;
                            up.Status = Convert.ToInt32(PackageStatus.Declined);
                            up.AcknowledgeBy = usr.UserID;
                            up.DeclineReason = obj.ReasonForDecline;
                            db.SubmitChanges();
                            UtilityFunctions.SendPurchaseDeclineEmail(up.User, up.Package, up);
                            retVal = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return retVal;
        }


        [HttpPost]
        public string GetWithdrawEmail([FromBody]WithdrawalEmailViewModel obj)
        {
            string retVal = "";

            try
            {
                Models.User usr = db.Users.Where(x => x.UserID == obj.UserID).FirstOrDefault();
                if(usr != null)
                {
                    if(obj.PaymentType == "Neteller")
                    {
                        retVal = usr.NetellerAccountEmail;
                    }
                    else if (obj.PaymentType == "Skrill")
                    {
                        retVal = usr.SkrillAccountEmail;
                    }
                    else if (obj.PaymentType == "bitcoin")
                    {
                        retVal = usr.BitCoinWalletAddress;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return retVal;
        }

        [HttpPost]
        public WithdrawViewModel LoadWithdraw([FromBody]int userID)
        {
            WithdrawViewModel retVal = new WithdrawViewModel();
            try
            {
                User usr = db.Users.Where(x => x.UserID == userID).FirstOrDefault();

                var payouts = usr.Payouts.Where(x => x.ActivatedDate.Value <= DateTime.Now);
                double totalEarnings = payouts.Sum(x => x.Amount.Value);
                retVal.TotalEarnings = totalEarnings;

                var pendingwithdraws = usr.Withdrawals.Where(x => x.Status.Value == Convert.ToInt32(WithdrawStatus.PendingApproval));
                double pendingWithVal = pendingwithdraws.Sum(x => x.Amount.Value);
                retVal.PendingWithdraws = pendingWithVal;

                var successWithdraws = usr.Withdrawals.Where(x => x.Status.Value == Convert.ToInt32(WithdrawStatus.ProcessedAndApproved));
                double successWithVal = successWithdraws.Sum(x => x.Amount.Value);
                retVal.SuccessfullWithdraws = successWithVal;

                retVal.AvailbleBalance = totalEarnings - pendingWithVal - successWithVal;

                var withdrawals = usr.Withdrawals.OrderByDescending(x => x.RequestedDate).Take(5);
                retVal.RecentWithdrawals = new List<WithdrawViewModelItem>();
                foreach (var item in withdrawals)
                {
                    WithdrawViewModelItem itm = new WithdrawViewModelItem()
                    {
                        WithdrawID = item.WithdrawalID,
                        RequestedDate = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                        Amount = item.Amount.Value,
                        Status = item.Status.Value == Convert.ToInt32(WithdrawStatus.PendingApproval) ? "Pending" : item.Status.Value == Convert.ToInt32(WithdrawStatus.ProcessedAndApproved) ? "Approved" : "Declined",
                        WithdrawMethod = item.PaymentType
                    };
                    retVal.RecentWithdrawals.Add(itm);
                }
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }

        [HttpPost]
        public bool SaveWithdrawal([FromBody]WithdrawViewModel obj)
        {
            bool retVal = false;
            Models.User usr = db.Users.Where(x => x.UserID == obj.UserID).FirstOrDefault();
            if (usr != null)
            {
                var payouts = usr.Payouts.Where(x => x.ActivatedDate.Value <= DateTime.Now);
                double totalEarnings = payouts.Sum(x => x.Amount.Value);

                var pendingwithdraws = usr.Withdrawals.Where(x => x.Status.Value == Convert.ToInt32(WithdrawStatus.PendingApproval));
                double pendingWithVal = pendingwithdraws.Sum(x => x.Amount.Value);

                var successWithdraws = usr.Withdrawals.Where(x => x.Status.Value == Convert.ToInt32(WithdrawStatus.ProcessedAndApproved));
                double successWithVal = successWithdraws.Sum(x => x.Amount.Value);

                double availableBalance = totalEarnings - pendingWithVal - successWithVal;

                if(obj.Amount <= availableBalance && obj.Amount >= 10)
                {
                    Models.Withdrawal withdraw = new Withdrawal()
                    {
                        Amount = obj.Amount,
                        PaymentType = obj.PaymentMethod,
                        RecipientEmail = obj.PaymentEmail,
                        RequestedDate = DateTime.Now,
                        Status = Convert.ToInt32(WithdrawStatus.PendingApproval),
                        User = usr
                    };

                    db.Withdrawals.InsertOnSubmit(withdraw);
                    db.SubmitChanges();
                    retVal = true;
                }
            }
            return retVal;
        }

        [HttpPost]
        public ViewWithdrawalDetailsViewModel GetWithdrawalDetails([FromBody]int withdrawID)
        {
            ViewWithdrawalDetailsViewModel retVal = new ViewWithdrawalDetailsViewModel();
            try
            {
                Models.Withdrawal withdraw = db.Withdrawals.Where(x => x.WithdrawalID == withdrawID).FirstOrDefault();
                if(withdraw != null)
                {
                    retVal.Name = withdraw.User.Name;
                    retVal.UserName = withdraw.User.UserName;
                    retVal.UserID = withdraw.User.UserID;
                    retVal.ProfileImage = withdraw.User.ProfileImage;
                    retVal.RequestedDate = withdraw.RequestedDate.Value.ToString("dd MMM yyyy");
                    retVal.Amount = withdraw.Amount.Value;
                    retVal.PaymentType = withdraw.PaymentType;
                    retVal.PaymentEmail = withdraw.RecipientEmail;
                    retVal.Status = withdraw.Status.Value == Convert.ToInt32(WithdrawStatus.PendingApproval) ? "Pending" : withdraw.Status.Value == Convert.ToInt32(WithdrawStatus.ProcessedAndApproved) ? "Approved" : "Declined";
                    retVal.TransactionId = withdraw.TransactionID;
                    retVal.SendersEmail = withdraw.SendersEmail;
                    retVal.ProcessedDate = withdraw.ApprovedDate.HasValue ? withdraw.ApprovedDate.Value.ToString("dd MMM yyyy") : "";
                    retVal.DeclineReason = withdraw.ReasonForDecline;
                };
            }
            catch (Exception)
            {
                
            }
            return retVal;
        }

        [HttpPost]
        public bool ApproveWithdrawalDetails([FromBody]ViewWithdrawalDetailsViewModel obj)
        {
            bool retVal = false;
            try
            {
                Models.User usr = db.Users.Where(x => x.UserID == obj.ApprovingUserID).FirstOrDefault();
                if(usr != null && usr.UserType == Convert.ToInt32(UserTypes.Admin))
                {
                    Models.Withdrawal withdraw = db.Withdrawals.Where(x => x.WithdrawalID == obj.WithdrawID).FirstOrDefault();
                    if(withdraw != null && withdraw.Status == Convert.ToInt32(WithdrawStatus.PendingApproval))
                    {
                        withdraw.SendersEmail = obj.SendersEmail;
                        withdraw.TransactionID = obj.TransactionId;
                        withdraw.ApprovedDate = DateTime.Now;
                        withdraw.ApprovedBy = usr.UserID;
                        withdraw.Status = Convert.ToInt32(WithdrawStatus.ProcessedAndApproved);
                        db.SubmitChanges();
                        UtilityFunctions.SendWithdrawalApprovedEmail(withdraw.User, withdraw);
                        retVal = true;
                    }
                }
            }
            catch (Exception)
            {
                
            }
            return retVal;
        }

        [HttpPost]
        public bool DeclineWithdrawal([FromBody]ViewWithdrawalDetailsViewModel obj)
        {
            bool retVal = false;
            try
            {
                Models.User usr = db.Users.Where(x => x.UserID == obj.ApprovingUserID).FirstOrDefault();
                if (usr != null && usr.UserType == Convert.ToInt32(UserTypes.Admin))
                {
                    Models.Withdrawal withdraw = db.Withdrawals.Where(x => x.WithdrawalID == obj.WithdrawID).FirstOrDefault();
                    if (withdraw != null && withdraw.Status == Convert.ToInt32(WithdrawStatus.PendingApproval))
                    {
                        withdraw.ApprovedDate = DateTime.Now;
                        withdraw.ReasonForDecline = obj.DeclineReason;
                        withdraw.ApprovedBy = usr.UserID;
                        withdraw.Status = Convert.ToInt32(WithdrawStatus.Declined);
                        db.SubmitChanges();

                        UtilityFunctions.SendWithdrawalDeclinedEmail(withdraw.User, withdraw);

                        retVal = true;
                    }
                }
            }
            catch (Exception)
            {

            }
            return retVal;
        }

        [HttpPost]
        public bool SendInvitations([FromBody]SendInvitationViewModel obj)
        {
            bool retVal = false;
            List<string> addrList = new List<string>();
            try
            {
                Models.User usr = db.Users.Where(x => x.UserID == obj.UserID).FirstOrDefault();
                if(usr != null)
                {
                    if (obj.EmailAddressList.Contains(","))
                    {
                        addrList = obj.EmailAddressList.Split(',').ToList();
                    }
                    else
                    {
                        addrList.Add(obj.EmailAddressList);
                    }
                    UtilityFunctions.SendInvitationEmail(usr, addrList);
                    retVal = true;
                }
            }
            catch (Exception)
            {

            }
            return retVal;
        }
    }
}
