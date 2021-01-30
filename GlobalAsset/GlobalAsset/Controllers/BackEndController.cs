using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalAsset.Models;

namespace GlobalAsset.Controllers
{
    public class BackEndController : Controller
    {
        GlobalAssetsDBDataContext db = new GlobalAssetsDBDataContext();
        // GET: BackEnd
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            DashBoardViewModel retVal = new DashBoardViewModel();
            try
            {
                User usr = (User)Session[UtilityFunctions.UserSession];
                retVal.UserID = usr.UserID;
                retVal.Name = usr.Name;
                retVal.ImageExt = usr.ProfileImage;
                retVal.Email = usr.Email;
                retVal.AccountBalance = 0;
                retVal.PendingWithdrawals = 0;
                retVal.AvailableBalance = 0;

                usr = db.Users.Where(x => x.UserID == usr.UserID).FirstOrDefault();

                var payouts = usr.Payouts.Where(x => x.ActivatedDate.Value <= DateTime.Now);
                double totalEarnings = payouts.Sum(x => x.Amount.Value);
                retVal.AccountBalance = totalEarnings;

                var pendingwithdraws = usr.Withdrawals.Where(x => x.Status.Value == Convert.ToInt32(WithdrawStatus.PendingApproval));
                double pendingWithVal = pendingwithdraws.Sum(x => x.Amount.Value);
                retVal.PendingWithdrawals = pendingWithVal;

                var successWithdraws = usr.Withdrawals.Where(x => x.Status.Value == Convert.ToInt32(WithdrawStatus.ProcessedAndApproved));
                double successWithVal = successWithdraws.Sum(x => x.Amount.Value);
                retVal.SuccessfullWithdrawals = successWithVal;

                retVal.AvailableBalance = totalEarnings - pendingWithVal - successWithVal;

                var deposits = usr.UserPackages.OrderByDescending(x => x.RequestedDate).Take(5);
                retVal.RecentDeposits = new List<DepositHistoryItem>();
                foreach(var item in deposits)
                {
                    DepositHistoryItem itm = new DepositHistoryItem()
                    {
                        DepositID = item.UserPackageID,
                        PackageName = item.Package.Name,
                        Amount = item.Amount.Value,
                        PurchasedOn = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                        Duration = item.Package.Duration.Value
                    };
                    retVal.RecentDeposits.Add(itm);
                }

                var withdrawals = usr.Withdrawals.OrderByDescending(x => x.RequestedDate).Take(5);
                retVal.RecentWithdrawals = new List<WithdrawViewModelItem>();
                foreach(var item in withdrawals)
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
            return View(retVal);
        }

        public ActionResult Payouts()
        {
            PayoutViewModel retVal = new PayoutViewModel();
            try
            {
                User usr = (User)Session[UtilityFunctions.UserSession];
                usr = db.Users.Where(x => x.UserID == usr.UserID).FirstOrDefault();

                var payouts = usr.Payouts.Where(x => x.ActivatedDate.Value <= DateTime.Now);

                retVal.PayoutList = payouts.Select(x => new PayoutViewModelItem()
                {
                    Amount = x.Amount.Value,
                    Description = x.Description,
                    PayoutDate = x.ActivatedDate.Value.ToString("dd MMM yyyy"),
                    PayoutID = x.PayoutID
                }).ToList();

                double totalEarnings = payouts.Sum(x => x.Amount.Value);
                retVal.TotalEarning = totalEarnings;

            }
            catch (Exception ex)
            {

            }
            
            return View(retVal);
        }
        public ActionResult Deposits()
        {
            List<Package> lstPackages = db.Packages.ToList();
            return View(lstPackages);
        }

        public ActionResult DepositsHistory()
        {
            DepositsHistoryViewModel vm = new DepositsHistoryViewModel();
            try
            {
                var usrSession = (GlobalAsset.Models.User)Session[UtilityFunctions.UserSession];
                if (usrSession != null)
                {
                    Models.User usr = db.Users.Where(x => x.UserID == usrSession.UserID).FirstOrDefault();
                    if (usr != null)
                    {
                        var ToBeApproved = usr.UserPackages.Where(x => x.Status == Convert.ToInt32(PackageStatus.PendingApproval));
                        foreach(var item in ToBeApproved)
                        {
                            DepositHistoryItem itm = new DepositHistoryItem()
                            {
                                DepositID = item.UserPackageID,
                                Amount = item.Amount.Value,
                                Duration = item.Package.Duration.Value,
                                PackageName = item.Package.Name,
                                Status = "Pending",
                                PurchasedOn = item.RequestedDate.Value.ToString("dd MMM yyyy")
                            };
                            vm.PendingList.Add(itm);
                        }

                        var ApprovedList = usr.UserPackages.Where(x => x.Status == Convert.ToInt32(PackageStatus.ProcessedAndStarted) && x.AcknowledgedDate.Value.AddDays(x.Package.Duration.Value) >= DateTime.Now);
                        foreach (var item in ApprovedList)
                        {
                            DepositHistoryItem itm = new DepositHistoryItem()
                            {
                                DepositID = item.UserPackageID,
                                Amount = item.Amount.Value,
                                Duration = item.Package.Duration.Value,
                                PackageName = item.Package.Name,
                                Status = "Approved",
                                PurchasedOn = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                                ApprovedOn = item.AcknowledgedDate.Value.ToString("dd MMM yyyy")
                            };
                            vm.ProgressingList.Add(itm);
                        }

                        var DeclinedList = usr.UserPackages.Where(x => x.Status == Convert.ToInt32(PackageStatus.Declined));
                        foreach (var item in DeclinedList)
                        {
                            DepositHistoryItem itm = new DepositHistoryItem()
                            {
                                DepositID = item.UserPackageID,
                                Amount = item.Amount.Value,
                                Duration = item.Package.Duration.Value,
                                PackageName = item.Package.Name,
                                Status = "Declined",
                                PurchasedOn = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                                ApprovedOn = item.AcknowledgedDate.Value.ToString("dd MMM yyyy")
                            };
                            vm.DeclinedList.Add(itm);
                        }

                        var ExpiredList = usr.UserPackages.Where(x => x.Status == Convert.ToInt32(PackageStatus.ProcessedAndStarted) && x.AcknowledgedDate.Value.AddDays(x.Package.Duration.Value) < DateTime.Now);
                        foreach (var item in ExpiredList)
                        {
                            DepositHistoryItem itm = new DepositHistoryItem()
                            {
                                DepositID = item.UserPackageID,
                                Amount = item.Amount.Value,
                                Duration = item.Package.Duration.Value,
                                PackageName = item.Package.Name,
                                Status = "Expired",
                                PurchasedOn = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                                ApprovedOn = item.AcknowledgedDate.Value.ToString("dd MMM yyyy"),
                                ExpiredOn = item.AcknowledgedDate.Value.AddDays(item.Package.Duration.Value).ToString("dd MMM yyyy")
                            };
                            vm.ExpiredList.Add(itm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return View(vm);
        }

        public ActionResult ViewDeposit(int upid)
        {
            ViewDepositViewModel vm = new ViewDepositViewModel();
            try
            {
                UserPackage up = db.UserPackages.Where(x => x.UserPackageID == upid).FirstOrDefault();
                Models.User sesUser = (Models.User)Session[UtilityFunctions.UserSession];
                if(up.User.UserID == sesUser.UserID || sesUser.UserType == Convert.ToInt32(UserTypes.Admin))
                {
                    vm.UserPackID = up.UserPackageID;

                    if (up.Status.Value == Convert.ToInt32(PackageStatus.PendingApproval))
                    {
                        vm.StatusText = "Pending Approval";
                        vm.Progress = 100;
                    }
                    else if (up.Status.Value == Convert.ToInt32(PackageStatus.ProcessedAndStarted) && up.AcknowledgedDate.Value.AddDays(up.Package.Duration.Value) >= DateTime.Now)
                    {
                        vm.StatusText = "Progressing";
                        vm.ApprovedDate = up.AcknowledgedDate.Value.ToString("dd MMM yyyy");
                        int packDuration = up.Package.Duration.Value;
                        var timesp = DateTime.Now.Subtract(up.AcknowledgedDate.Value).TotalDays;
                        int progress = Convert.ToInt32((timesp / packDuration) * 100);
                        vm.Progress = progress;

                    }
                    else if (up.Status.Value == Convert.ToInt32(PackageStatus.ProcessedAndStarted) && up.AcknowledgedDate.Value.AddDays(up.Package.Duration.Value) < DateTime.Now)
                    {
                        vm.StatusText = "Expired";
                        vm.ApprovedDate = up.AcknowledgedDate.Value.ToString("dd MMM yyyy");
                        vm.ExpiredDate = up.AcknowledgedDate.Value.AddDays(up.Package.Duration.Value).ToString("dd MMM yyyy");
                        vm.Progress = 100;
                    }
                    else if (up.Status.Value == Convert.ToInt32(PackageStatus.Declined))
                    {
                        vm.StatusText = "Declined";
                        vm.ApprovedDate = up.AcknowledgedDate.Value.ToString("dd MMM yyyy");
                        vm.Progress = 100;
                    }

                    vm.PackColor = up.Package.PackageColor;
                    vm.PackName = up.Package.Name;
                    vm.SubHeading = up.Package.SubHeading;
                    vm.InvestmentText = up.Package.InvestmentText;
                    vm.Interest = up.Package.Interest.ToString();
                    vm.Duration = up.Package.Duration.ToString();
                    vm.DepositedAmount = up.Amount.Value;
                    vm.DepositedDate = up.RequestedDate.Value.ToString("dd MMM yyyy");
                    vm.PaymentMethod = up.PaymentType;
                    vm.TransactionID = up.TransactionID;
                    vm.TransactionEmail = up.SendersEmail;
                    vm.CompanyEmail = up.RecipientEmail;
                    vm.UserID = up.User.UserID;
                    vm.Name = up.User.Name;
                    vm.UserName = up.User.UserName;
                    vm.ProfileImage = up.User.ProfileImage;
                    vm.ReasonForDecline = up.DeclineReason;
                    vm.IsAuthoried = true;
                }
                else
                {
                    vm.IsAuthoried = false;
                }
            }
            catch (Exception ex)
            {

            }
            return View(vm);
        }
        public ActionResult AdminPendingDeposits()
        {
            var depositList = db.UserPackages.Where(x => x.Status.Value == Convert.ToInt32(PackageStatus.PendingApproval));
            List<DepositHistoryItem> retVal = new List<DepositHistoryItem>();
            foreach(var item in depositList)
            {
                DepositHistoryItem itm = new DepositHistoryItem()
                {
                    Amount = item.Amount.Value,
                    DepositID = item.UserPackageID,
                    Duration = item.Package.Duration.Value,
                    PackageName = item.Package.Name,
                    PurchasedOn = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                    Status = "Pending"
                };
                retVal.Add(itm);
            }
            return View(retVal);
        }

        public ActionResult AdminProcessingDeposits()
        {
            var depositList = db.UserPackages.Where(x => x.Status == Convert.ToInt32(PackageStatus.ProcessedAndStarted) && x.AcknowledgedDate.Value.AddDays(x.Package.Duration.Value) >= DateTime.Now);
            List<DepositHistoryItem> retVal = new List<DepositHistoryItem>();
            foreach (var item in depositList)
            {
                DepositHistoryItem itm = new DepositHistoryItem()
                {
                    DepositID = item.UserPackageID,
                    Amount = item.Amount.Value,
                    Duration = item.Package.Duration.Value,
                    PackageName = item.Package.Name,
                    Status = "Approved",
                    PurchasedOn = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                    ApprovedOn = item.AcknowledgedDate.Value.ToString("dd MMM yyyy")
                };
                retVal.Add(itm);
            }
            return View(retVal);
        }

        public ActionResult AdminDeclinedDeposits()
        {
            var depositList = db.UserPackages.Where(x => x.Status == Convert.ToInt32(PackageStatus.Declined));
            List<DepositHistoryItem> retVal = new List<DepositHistoryItem>();
            foreach (var item in depositList)
            {
                DepositHistoryItem itm = new DepositHistoryItem()
                {
                    DepositID = item.UserPackageID,
                    Amount = item.Amount.Value,
                    Duration = item.Package.Duration.Value,
                    PackageName = item.Package.Name,
                    Status = "Declined",
                    PurchasedOn = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                    ApprovedOn = item.AcknowledgedDate.Value.ToString("dd MMM yyyy")
                };
                retVal.Add(itm);
            }
            return View(retVal);
        }

        public ActionResult AdminCompletedDeposits()
        {
            var depositList = db.UserPackages.Where(x => x.Status == Convert.ToInt32(PackageStatus.ProcessedAndStarted) && x.AcknowledgedDate.Value.AddDays(x.Package.Duration.Value) < DateTime.Now);
            List<DepositHistoryItem> retVal = new List<DepositHistoryItem>();
            foreach (var item in depositList)
            {
                DepositHistoryItem itm = new DepositHistoryItem()
                {
                    DepositID = item.UserPackageID,
                    Amount = item.Amount.Value,
                    Duration = item.Package.Duration.Value,
                    PackageName = item.Package.Name,
                    Status = "Expired",
                    PurchasedOn = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                    ApprovedOn = item.AcknowledgedDate.Value.ToString("dd MMM yyyy"),
                    ExpiredOn = item.AcknowledgedDate.Value.AddDays(item.Package.Duration.Value).ToString("dd MMM yyyy")
                };
                retVal.Add(itm);
            }
            return View(retVal);
        }

        public ActionResult AdminApprovedWithdrawals()
        {
            WithdrawHistoryViewModel retVal = new WithdrawHistoryViewModel();
            try
            {
                User usr = (User)Session[UtilityFunctions.UserSession];

                if (usr.UserType == Convert.ToInt32(UserTypes.Admin))
                {
                    var processedWithdrawals = db.Withdrawals.Where(x => x.Status == Convert.ToInt32(WithdrawStatus.ProcessedAndApproved));
                    foreach (var item in processedWithdrawals)
                    {
                        WithdrawViewModelItem itm = new WithdrawViewModelItem()
                        {
                            WithdrawID = item.WithdrawalID,
                            RequestedDate = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                            ProcessedDate = item.ApprovedDate.Value.ToString("dd MMM yyyy"),
                            Amount = item.Amount.Value,
                            Status = "Approved",
                            WithdrawMethod = item.PaymentType
                        };
                        retVal.ApprovedWithdrawals.Add(itm);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(retVal);
        }

        public ActionResult DeclinedWithdrawals()
        {
            WithdrawHistoryViewModel retVal = new WithdrawHistoryViewModel();
            try
            {
                User usr = (User)Session[UtilityFunctions.UserSession];

                if (usr.UserType == Convert.ToInt32(UserTypes.Admin))
                {
                    var declinedWithdrawals = db.Withdrawals.Where(x => x.Status == Convert.ToInt32(WithdrawStatus.Declined));
                    foreach (var item in declinedWithdrawals)
                    {
                        WithdrawViewModelItem itm = new WithdrawViewModelItem()
                        {
                            WithdrawID = item.WithdrawalID,
                            RequestedDate = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                            ProcessedDate = item.ApprovedDate.Value.ToString("dd MMM yyyy"),
                            Amount = item.Amount.Value,
                            Status = "Declined",
                            WithdrawMethod = item.PaymentType
                        };
                        retVal.DeclinedWithdrawals.Add(itm);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(retVal);
        }

        public ActionResult AdminPendingWithdrawals()
        {
            WithdrawHistoryViewModel retVal = new WithdrawHistoryViewModel();
            try
            {
                User usr = (User)Session[UtilityFunctions.UserSession];

                if(usr.UserType == Convert.ToInt32(UserTypes.Admin))
                {
                    var pendingWithdrawals = db.Withdrawals.Where(x => x.Status == Convert.ToInt32(WithdrawStatus.PendingApproval));
                    foreach (var item in pendingWithdrawals)
                    {
                        WithdrawViewModelItem itm = new WithdrawViewModelItem()
                        {
                            WithdrawID = item.WithdrawalID,
                            RequestedDate = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                            Amount = item.Amount.Value,
                            Status = "Pending",
                            WithdrawMethod = item.PaymentType
                        };
                        retVal.PendingWithdrawals.Add(itm);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(retVal);
        }

        public ActionResult Withdraw()
        {
            WithdrawViewModel retVal = new WithdrawViewModel();
            try
            {
                User usr = (User)Session[UtilityFunctions.UserSession];
                usr = db.Users.Where(x => x.UserID == usr.UserID).FirstOrDefault();

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
            catch(Exception ex)
            {

            }
            return View(retVal);
        }
        public ActionResult WithdrawalHistory()
        {
            WithdrawHistoryViewModel retVal = new WithdrawHistoryViewModel();
            try
            {
                User usr = (User)Session[UtilityFunctions.UserSession];
                usr = db.Users.Where(x => x.UserID == usr.UserID).FirstOrDefault();

                var pendingWithdrawals = usr.Withdrawals.Where(x => x.Status == Convert.ToInt32(WithdrawStatus.PendingApproval)).OrderByDescending(x => x.RequestedDate);
                foreach (var item in pendingWithdrawals)
                {
                    WithdrawViewModelItem itm = new WithdrawViewModelItem()
                    {
                        WithdrawID = item.WithdrawalID,
                        RequestedDate = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                        Amount = item.Amount.Value,
                        Status = "Pending",
                        WithdrawMethod = item.PaymentType
                    };
                    retVal.PendingWithdrawals.Add(itm);
                }

                var processedWithdrawals = usr.Withdrawals.Where(x => x.Status == Convert.ToInt32(WithdrawStatus.ProcessedAndApproved)).OrderByDescending(x => x.RequestedDate);
                foreach (var item in processedWithdrawals)
                {
                    WithdrawViewModelItem itm = new WithdrawViewModelItem()
                    {
                        WithdrawID = item.WithdrawalID,
                        RequestedDate = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                        ProcessedDate = item.ApprovedDate.Value.ToString("dd MMM yyyy"),
                        Amount = item.Amount.Value,
                        Status =  "Approved",
                        WithdrawMethod = item.PaymentType
                    };
                    retVal.ApprovedWithdrawals.Add(itm);
                }

                var declinedWithdrawals = usr.Withdrawals.Where(x => x.Status == Convert.ToInt32(WithdrawStatus.Declined)).OrderByDescending(x => x.RequestedDate);
                foreach (var item in declinedWithdrawals)
                {
                    WithdrawViewModelItem itm = new WithdrawViewModelItem()
                    {
                        WithdrawID = item.WithdrawalID,
                        RequestedDate = item.RequestedDate.Value.ToString("dd MMM yyyy"),
                        ProcessedDate = item.ApprovedDate.Value.ToString("dd MMM yyyy"),
                        Amount = item.Amount.Value,
                        Status = "Declined",
                        WithdrawMethod = item.PaymentType
                    };
                    retVal.DeclinedWithdrawals.Add(itm);
                }
            }
            catch (Exception ex)
            {

            }
            return View(retVal);
        }

        public ActionResult ViewWithdrawal(int withid)
        {
            ViewBag.AdminID = 0;
            ViewBag.withid = withid;
            User usr = (User)Session[UtilityFunctions.UserSession];
            if(usr.UserType == Convert.ToInt32(UserTypes.Admin))
            {
                ViewBag.AdminID = usr.UserID;
            }
            return View();
        }

        public ActionResult BuyPackage(int packid)
        {
            Package pack = db.Packages.Where(x => x.PackageID == packid).FirstOrDefault();
            return View(pack);
        }
        public ActionResult InviteFriends()
        {
            return View();
        }

        public ActionResult MyFriendCircle()
        {
            List<FriendCircleViewModel> retVal = new List<FriendCircleViewModel>();
            try
            {
                User usr = (User)Session[UtilityFunctions.UserSession];

                var friendsList = db.Users.Where(x => x.RefferedBy == usr.UserID);
                foreach(var item in friendsList)
                {
                    FriendCircleViewModel itm = new FriendCircleViewModel()
                    {
                        UserID = item.UserID,
                        JoinedDate = item.SignedUpDate.Value.ToString("dd MMM yyyy"),
                        Name = item.Name,
                        UserName = item.UserName,
                        ProfileImage = item.ProfileImage
                    };
                    retVal.Add(itm);
                }

            }
            catch (Exception ex)
            {
                
            }
            return View(retVal);
        }
    }
}