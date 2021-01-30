using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalAsset.Models;
using System.IO;

namespace GlobalAsset.Controllers
{
    public class UserController : Controller
    {
        GlobalAssetsDBDataContext db = new GlobalAssetsDBDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.IsError = false;
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserViewModel vm)
        {
            ViewBag.IsError = false;
            ViewBag.Error = "";

            try
            {
                User usr = db.Users.Where(x => x.UserName == vm.UserName && x.Password == UtilityFunctions.Encryptdata(vm.Password)).FirstOrDefault();
                if (usr != null)
                {
                    if (usr.Active.Value && usr.IsMailVerified.Value)
                    {
                        Session[UtilityFunctions.UserSession] = usr;
                        Response.Redirect("~/BackEnd/Index/", false);
                    }
                    else
                    {
                        Response.Redirect("~/User/VerificationEmailSent?userID=" + usr.UserID, false);
                    }
                    
                }
                else
                {
                    ViewBag.IsError = true;
                    ViewBag.Error = "Username or Password invalid. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.Error = "Oops!, Something went wrong. Please try again.";
            }

            return View();
        }

        [HttpGet]
        public ActionResult SignUp(string referral = "")
        {
            ViewBag.IsError = false;
            ViewBag.Error = "";
            ViewBag.Ref = referral;
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserViewModel signup)
        {
            ViewBag.IsError = false;
            ViewBag.Error = "";
            ViewBag.Ref = signup.ReferralName;

            try
            {
                if (!db.Users.Any(x => x.Email == signup.Email))
                {
                    if (!db.Users.Any(x => x.UserName == signup.UserName))
                    {
                        int refUserID = 0;

                        if (!string.IsNullOrEmpty(signup.ReferralName))
                        {
                            string refusername = UtilityFunctions.Decryptdata(signup.ReferralName);
                            User refUsr = db.Users.Where(x => x.UserName == refusername).FirstOrDefault();
                            if (refUsr != null)
                            {
                                refUserID = refUsr.UserID;
                            }
                        }

                        User usr = new User()
                        {
                            Title = signup.Title,
                            Name = signup.Name,
                            Address = signup.Address,
                            Country = signup.Country,
                            Email = signup.Email,
                            Mobile = signup.Mobile,
                            Telephone = signup.Telephone,
                            UserName = signup.UserName,
                            Password = UtilityFunctions.Encryptdata(signup.Password),
                            RefferedBy = refUserID,
                            UserType = Convert.ToInt32(UserTypes.SystemUser),
                            SignedUpDate = DateTime.Now,
                            Active = false,
                            IsMailVerified = false
                        };

                        db.Users.InsertOnSubmit(usr);
                        db.SubmitChanges();
                        UtilityFunctions.SendVerificationEmail(usr);
                        Response.Redirect("~/User/VerificationEmailSent?userID=" + usr.UserID, false);
                    }
                    else
                    {
                        ViewBag.IsError = true;
                        ViewBag.Error = "Username already exists in the system. Please use a different Username and try again.";
                    }
                }
                else
                {
                    ViewBag.IsError = true;
                    ViewBag.Error = "Email entered already exists in the system. Please use a different email and try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.Error = "Oops!, Something went wrong. Please try again.";
            }

            return View();
        }

        [HttpGet]
        public ActionResult VerificationEmailSent(int userID = 0)
        {
            User usr = db.Users.Where(x => x.UserID == userID).FirstOrDefault();
            UserViewModel vm = new UserViewModel();
            if (usr != null)
            {
                vm.UserID = usr.UserID;
                vm.Name = usr.Name;
            }
            else
            {
                vm.UserID = 0;
                vm.Name = "No User";
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult VerificationEmailSent(UserViewModel vm)
        {
            User usr = db.Users.Where(x => x.UserID == vm.UserID).FirstOrDefault();
            if(usr != null)
            {
                UtilityFunctions.SendVerificationEmail(usr);
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult VerifyEmail(string data)
        {
            ViewBag.IsError = false;
            if (!string.IsNullOrEmpty(data))
            {
                string username = UtilityFunctions.Decryptdata(data);
                User usr = db.Users.Where(x => x.UserName == username).FirstOrDefault();
                if (usr != null)
                {
                    usr.Active = true;
                    usr.IsMailVerified = true;
                    db.SubmitChanges();
                    ViewBag.Name = usr.Name;
                }
                else
                {
                    ViewBag.IsError = true;
                }
            }
            else
            {
                ViewBag.IsError = true;
            }
            return View();
        }

        [HttpGet]
        public ActionResult MyProfile()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveProfileImage(int userID, HttpPostedFileBase file)
        {
            bool retVal = false;
            try
            {
                string path = Server.MapPath("~") + "//Uploads//ProfileImages//" + userID + Path.GetExtension(file.FileName);
                file.SaveAs(path);

                User usr = db.Users.Where(x => x.UserID == userID).FirstOrDefault();

                if(usr != null)
                {
                    usr.ProfileImage = Path.GetExtension(file.FileName);
                    db.SubmitChanges();

                    usr = (GlobalAsset.Models.User)Session[UtilityFunctions.UserSession];
                    usr.ProfileImage = Path.GetExtension(file.FileName);
                    Session[UtilityFunctions.UserSession] = usr;

                    retVal = true;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool SignOut()
        {
            bool retVal = false;
            try
            {
                if (Session[UtilityFunctions.UserSession] != null)
                {
                    Session.Abandon();
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                //LogClass.WriteErrorLog(ex);
            }
            return retVal;
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            ViewBag.IsError = false;
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string emailAddress)
        {
            ViewBag.IsError = false;
            ViewBag.Error = "";
            User usr = db.Users.Where(x => x.Email == emailAddress).FirstOrDefault();
            if(usr != null)
            {
                foreach(var item in usr.PasswordResetRequests.Where(x => x.RequestStatus == Convert.ToInt32(PasswordResetRequestStatus.Pending)))
                {
                    item.RequestStatus = Convert.ToInt32(PasswordResetRequestStatus.Canceled);
                }
                db.SubmitChanges();

                PasswordResetRequest prr = new PasswordResetRequest()
                {
                    User = usr,
                    RequestedDate = DateTime.Now,
                    RequestStatus = Convert.ToInt32(PasswordResetRequestStatus.Pending)
                };

                db.PasswordResetRequests.InsertOnSubmit(prr);
                db.SubmitChanges();
                int passwordRequestID = prr.RequestID;
                UtilityFunctions.SendPasswordResetEmail(usr, passwordRequestID);
                string emailadd = UtilityFunctions.Encryptdata(usr.Email);
                Response.Redirect("~/User/ResetPasswordEmailSent?emailaddress=" + emailadd, false);
            }
            else
            {
                ViewBag.IsError = true;
                ViewBag.Error = "Email address you entered doesn't assosiate with a user account. Please enter valied email address or try register with new email address.";
            }
            return View();
        }

        [HttpGet]
        public ActionResult ResetPasswordEmailSent(string emailaddress)
        {
            ViewBag.email = UtilityFunctions.Decryptdata(emailaddress);
            return View();
        }

        [HttpGet]
        public ActionResult ResetPasswordViaEmail(string paramem = "", string paramprr = "")
        {
            ViewBag.paramem = paramem;
            ViewBag.paramprr = paramprr;
            ViewBag.IsError = false;
            ViewBag.Error = "";

            return View();
        }

        [HttpPost]
        public ActionResult ResetPasswordViaEmail(ResetPasswordUserViewModel vm)
        {
            ViewBag.paramem = vm.ParamEm;
            ViewBag.paramprr = vm.ParamPrr;
            ViewBag.IsError = false;
            ViewBag.Error = "";
            try
            {
                string paraEmail = UtilityFunctions.Decryptdata(vm.ParamEm);
                int prrid = Convert.ToInt32(UtilityFunctions.Decryptdata(vm.ParamPrr));
                PasswordResetRequest prr = db.PasswordResetRequests.Where(x => x.RequestID == prrid && x.RequestStatus == Convert.ToInt32(PasswordResetRequestStatus.Pending) && x.User.Email == paraEmail).FirstOrDefault();
                if (prr != null)
                {
                    if(vm.Password == vm.ConfirmPassword)
                    {
                        User usr = prr.User;
                        usr.Password = UtilityFunctions.Encryptdata(vm.Password);

                        prr.RequestStatus = Convert.ToInt32(PasswordResetRequestStatus.PasswordReset);
                        prr.AcknowledgeDate = DateTime.Now;
                        db.SubmitChanges();
                        Response.Redirect("~/User/Login", false);
                    }
                    else
                    {
                        ViewBag.IsError = true;
                        ViewBag.Error = "Password and Confirm Password mismatched";
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.IsError = true;
                ViewBag.Error = "Oops!, Something went wrong. Please try again.";
            }
            return View();
        }

        [HttpGet]
        public ActionResult CancelPasswordResetRequest(string paramem = "", string paramprr = "")
        {
            try
            {
                string email = UtilityFunctions.Decryptdata(paramem);
                int prrid = Convert.ToInt32(UtilityFunctions.Decryptdata(paramprr));
                PasswordResetRequest prr = db.PasswordResetRequests.Where(x => x.RequestID == prrid && x.RequestStatus == Convert.ToInt32(PasswordResetRequestStatus.Pending) && x.User.Email == email).FirstOrDefault();
                if(prr != null)
                {
                    prr.RequestStatus = Convert.ToInt32(PasswordResetRequestStatus.Canceled);
                    prr.AcknowledgeDate = DateTime.Now;
                }
                db.SubmitChanges();
            }
            catch (Exception)
            {
                
            }

            return View();
        }

        [HttpGet]
        public ActionResult TestPage()
        {
            return View();
        }
    }
}