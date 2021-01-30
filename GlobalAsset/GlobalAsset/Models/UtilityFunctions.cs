using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace GlobalAsset.Models
{
    
    public enum UserTypes
    {
        Admin = 1,
        SystemUser = 2
    }

    public enum PackageStatus
    {
        PendingApproval = 1,
        ProcessedAndStarted = 2,
        Expired = 3,
        Declined = 4
    }

    public enum WithdrawStatus
    {
        PendingApproval = 1,
        ProcessedAndApproved = 2,
        Declined = 3
    }

    public enum PasswordResetRequestStatus
    {
        Pending = 1,
        PasswordReset = 2,
        Canceled = 3
    }

    public class UtilityFunctions
    {
        public const string UserSession = "UserSession";

        public static string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        public static string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

        public static bool SendMail()
        {
            SmtpClient smtpClient = new SmtpClient("mail.cryptinvestmentstech.com", 25);

            smtpClient.Credentials = new System.Net.NetworkCredential("noreply@cryptinvestmentstech.com", "noreply@crypt");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = false;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("noreply@cryptinvestmentstech.com", "Crypt Investments Tech");
            mail.To.Add(new MailAddress("heshithachathuranga@gmail.com"));

            smtpClient.Send(mail);

            return true;
        }

        public static bool SendGmail(string subject, string body, string toEmail, bool ishtml)
        {
            using (MailMessage mm = new MailMessage("no-reply@cminingfarm.com", toEmail))
            {
                mm.Subject = subject;
                mm.Body = body;
                //if (fuAttachment.HasFile)
                //{
                //    string FileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
                //    mm.Attachments.Add(new Attachment(fuAttachment.PostedFile.InputStream, FileName));
                //}
                mm.IsBodyHtml = ishtml;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mail.cminingfarm.com";
                smtp.EnableSsl = false;
                NetworkCredential NetworkCred = new NetworkCredential("no-reply@cminingfarm.com", "abc@123");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 8889;
                smtp.Send(mm);
            }
            return true;
        }

        public static bool SendVerificationEmail(User user)
        {
            bool retVal = false;
            try
            {
                string subject = "Welcome to CMiningFarms.com";
                string body = "";
                string verificationurl = string.Format("https://{0}/User/VerifyEmail?data={1}", HttpContext.Current.Request.Url.Authority, Encryptdata(user.UserName));

                //string body = "<h3>Hi " + user.Name + ",</h3>";
                //body += "<p>Welcome to cryptinvestmentstech.com, Please click the following link and verify your email address</p>";
                //body += "<a href=\""+verificationurl+ "\">" + verificationurl + "</a>";

                body += "<!DOCTYPE html>";
                body += "<html style=\"\" class=\" js flexbox flexboxlegacy canvas canvastext webgl no-touch geolocation postmessage websqldatabase indexeddb hashchange history draganddrop websockets rgba hsla multiplebgs backgroundsize borderimage borderradius boxshadow textshadow opacity cssanimations csscolumns cssgradients cssreflections csstransforms csstransforms3d csstransitions fontface generatedcontent video audio localstorage sessionstorage webworkers applicationcache svg inlinesvg smil svgclippaths\">";
                body += "<head>";
                body += "<meta name=\"viewport\" content=\"width=device-width\">";
                body += "<title>Sign Up - CMiningFarms.com</title>";
                body += "<link href=\"http://www.cminingfarm.com/Content/bootstrap.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/font-awesome.min.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/ionicons.min.css\" rel=\"stylesheet\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/AdminLTE/css/AdminLTE.min.css\" rel=\"stylesheet\">";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/modernizr-2.6.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-1.10.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-3.1.1.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/bootstrap.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/respond.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Content/AdminLTE/js/app.min.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/UtilityMethods.js\"></script>";
                body += "<link href=\"http://www.cminingfarm.com/Content/site.css\" rel=\"stylesheet\">";
                body += "</head>";
                body += "<body class=\"register-page  pace-running pace-done\" style=\"\"><div class=\"pace  pace-inactive\"><div class=\"pace-progress\" data-progress-text=\"100%\" data-progress=\"99\" style=\"transform: translate3d(100%, 0px, 0px);\">";
                body += "<div class=\"pace-progress-inner\"></div>";
                body += "</div>";
                body += "<div class=\"pace-activity\"></div></div>";
                body += "<div class=\"register-box1 register-box\">";
                body += "<div class=\"register-logo\">";
                body += "<a href=\"https://CMiningFarms.com\" target=\"_blank\"><img src=\"http://www.cminingfarm.com/Content/crypto/images/logo/logo.png\" alt=\"CMiningFarms.com\"></a>";
                body += "</div>";
                body += "<div class=\"register-box-body\" style=\"width:720px\">";
                body += "<div class=\"row\">";
                body += "<div class=\"col-md-12\">";
                body += "<form method=\"post\">";
                body += "<h2>Hi! "+ user.Name + "</h2>";
                body += "<p class=\"lead\">We welcome you to the CMiningFarms.com family, Here we have new way of investment. This email is to verify your email account. Please click the following button to proceed with your new way of earning money. Enjoy :)</p>";
                body += "<a class=\"btn btn-warning btn-flat\" href=\""+ verificationurl + "\" target=\"_blank\">Click Here To Verify</a>";
                body += "<hr>";
                body += "<p class=\"text-yellow\">Button doesn't work, do not worry. Copy and paste below link in your browser. You are good to go.</p>";
                body += "<p class=\"text-blue\">"+ verificationurl + "</p>";
                body += "</form>";
                body += "</div>";
                body += "</div>";
                body += "</div>";
                body += "<!-- /.form-box -->";
                body += "</div>";
                body += "</body></html>";


                retVal = SendGmail(subject, body, user.Email, true);
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }

        public static bool SendPurchaseCompletedEmail(User user, Package pack, UserPackage up)
        {
            bool retVal = false;
            try
            {
                string subject = "Your Purchase of CMiningFarms.com";
                string body = "";

                body += "<!DOCTYPE html>";
                body += "<html style=\"\" class=\" js flexbox flexboxlegacy canvas canvastext webgl no-touch geolocation postmessage websqldatabase indexeddb hashchange history draganddrop websockets rgba hsla multiplebgs backgroundsize borderimage borderradius boxshadow textshadow opacity cssanimations csscolumns cssgradients cssreflections csstransforms csstransforms3d csstransitions fontface generatedcontent video audio localstorage sessionstorage webworkers applicationcache svg inlinesvg smil svgclippaths\">";
                body += "<head>";
                body += "<meta name=\"viewport\" content=\"width=device-width\">";
                body += "<title>Sign Up - CMiningFarms.com</title>";
                body += "<link href=\"http://www.cminingfarm.com/Content/bootstrap.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/font-awesome.min.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/ionicons.min.css\" rel=\"stylesheet\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/AdminLTE/css/AdminLTE.min.css\" rel=\"stylesheet\">";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/modernizr-2.6.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-1.10.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-3.1.1.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/bootstrap.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/respond.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Content/AdminLTE/js/app.min.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/UtilityMethods.js\"></script>";
                body += "<link href=\"http://www.cminingfarm.com/Content/site.css\" rel=\"stylesheet\">";
                body += "</head>";
                body += "<body class=\"register-page  pace-running pace-done\" style=\"\"><div class=\"pace  pace-inactive\"><div class=\"pace-progress\" data-progress-text=\"100%\" data-progress=\"99\" style=\"transform: translate3d(100%, 0px, 0px);\">";
                body += "<div class=\"pace-progress-inner\"></div>";
                body += "</div>";
                body += "<div class=\"pace-activity\"></div></div>";
                body += "<div class=\"register-box1 register-box\">";
                body += "<div class=\"register-logo\">";
                body += "<a href=\"https://CMiningFarms.com\" target=\"_blank\"><img src=\"http://www.cminingfarm.com/Content/crypto/images/logo/logo.png\" alt=\"CMiningFarms.com\"></a>";
                body += "</div>";
                body += "<div class=\"register-box-body\" style=\"width:720px\">";
                body += "<div class=\"row\">";
                body += "<div class=\"col-md-12\">";
                body += "<form method=\"post\">";
                body += "<h2>Hi! " + user.Name + "</h2>";
                body += "<p class=\"lead\">Thank you for your purchasing of investment package. Our agents will approve it once we verify your payment.</p>";
                body += "<h3>Package Details</h3>";
                body += "<p class=\"lead\">Package Name : " + pack.Name + "</p>";
                body += "<p class=\"lead\">Amount Invested : $" + up.Amount + "</p>";
                body += "<p class=\"lead\">Duration : " + pack.Duration + " Days</p>";
                body += "<p class=\"lead\">Payment Method : " + up.PaymentType + "</p>";
                body += "<p class=\"lead\">Payment Transaction ID : " + up.TransactionID + "</p>";
                body += "<p class=\"lead\">Transaction Email : " + up.SendersEmail + "</p>";
                body += "<p class=\"lead\">Purchased Date : " + up.RequestedDate + "</p>";
                body += "</form>";
                body += "</div>";
                body += "</div>";
                body += "</div>";
                body += "<!-- /.form-box -->";
                body += "</div>";
                body += "</body></html>";


                retVal = SendGmail(subject, body, user.Email, true);
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }

        public static bool SendPurchaseVerificationEmail(User user, Package pack, UserPackage up)
        {
            bool retVal = false;
            try
            {
                string subject = "Your Deposit Activated Successfully";
                string body = "";

                body += "<!DOCTYPE html>";
                body += "<html style=\"\" class=\" js flexbox flexboxlegacy canvas canvastext webgl no-touch geolocation postmessage websqldatabase indexeddb hashchange history draganddrop websockets rgba hsla multiplebgs backgroundsize borderimage borderradius boxshadow textshadow opacity cssanimations csscolumns cssgradients cssreflections csstransforms csstransforms3d csstransitions fontface generatedcontent video audio localstorage sessionstorage webworkers applicationcache svg inlinesvg smil svgclippaths\">";
                body += "<head>";
                body += "<meta name=\"viewport\" content=\"width=device-width\">";
                body += "<title>Sign Up - CMiningFarms.com</title>";
                body += "<link href=\"http://www.cminingfarm.com/Content/bootstrap.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/font-awesome.min.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/ionicons.min.css\" rel=\"stylesheet\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/AdminLTE/css/AdminLTE.min.css\" rel=\"stylesheet\">";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/modernizr-2.6.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-1.10.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-3.1.1.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/bootstrap.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/respond.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Content/AdminLTE/js/app.min.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/UtilityMethods.js\"></script>";
                body += "<link href=\"http://www.cminingfarm.com/Content/site.css\" rel=\"stylesheet\">";
                body += "</head>";
                body += "<body class=\"register-page  pace-running pace-done\" style=\"\"><div class=\"pace  pace-inactive\"><div class=\"pace-progress\" data-progress-text=\"100%\" data-progress=\"99\" style=\"transform: translate3d(100%, 0px, 0px);\">";
                body += "<div class=\"pace-progress-inner\"></div>";
                body += "</div>";
                body += "<div class=\"pace-activity\"></div></div>";
                body += "<div class=\"register-box1 register-box\">";
                body += "<div class=\"register-logo\">";
                body += "<a href=\"https://CMiningFarms.com\" target=\"_blank\"><img src=\"http://www.cminingfarm.com/Content/crypto/images/logo/logo.png\" alt=\"CMiningFarms.com\"></a>";
                body += "</div>";
                body += "<div class=\"register-box-body\" style=\"width:720px\">";
                body += "<div class=\"row\">";
                body += "<div class=\"col-md-12\">";
                body += "<form method=\"post\">";
                body += "<h2>Hi! " + user.Name + "</h2>";
                body += "<p class=\"lead\">Your payment successfully verified by our agents. Requested package has been updated.</p>";
                body += "<h3>Package Details</h3>";
                body += "<p class=\"lead\">Package Name : " + pack.Name + "</p>";
                body += "<p class=\"lead\">Amount Invested : $" + up.Amount + "</p>";
                body += "<p class=\"lead\">Duration : " + pack.Duration + " Days</p>";
                body += "<p class=\"lead\">Payment Method : " + up.PaymentType + "</p>";
                body += "<p class=\"lead\">Payment Transaction ID : " + up.TransactionID + "</p>";
                body += "<p class=\"lead\">Transaction Email : " + up.SendersEmail + "</p>";
                body += "<p class=\"lead\">Purchased Date : " + up.RequestedDate + "</p>";
                body += "<p class=\"lead\">Approved Date : " + up.AcknowledgedDate + "</p>";
                body += "</form>";
                body += "</div>";
                body += "</div>";
                body += "</div>";
                body += "<!-- /.form-box -->";
                body += "</div>";
                body += "</body></html>";


                retVal = SendGmail(subject, body, user.Email, true);
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }

        public static bool SendPurchaseDeclineEmail(User user, Package pack, UserPackage up)
        {
            bool retVal = false;
            try
            {
                string subject = "Your Deposit Declined By CMiningFarms.com";
                string body = "";

                body += "<!DOCTYPE html>";
                body += "<html style=\"\" class=\" js flexbox flexboxlegacy canvas canvastext webgl no-touch geolocation postmessage websqldatabase indexeddb hashchange history draganddrop websockets rgba hsla multiplebgs backgroundsize borderimage borderradius boxshadow textshadow opacity cssanimations csscolumns cssgradients cssreflections csstransforms csstransforms3d csstransitions fontface generatedcontent video audio localstorage sessionstorage webworkers applicationcache svg inlinesvg smil svgclippaths\">";
                body += "<head>";
                body += "<meta name=\"viewport\" content=\"width=device-width\">";
                body += "<title>Sign Up - CMiningFarms.com</title>";
                body += "<link href=\"http://www.cminingfarm.com/Content/bootstrap.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/font-awesome.min.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/ionicons.min.css\" rel=\"stylesheet\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/AdminLTE/css/AdminLTE.min.css\" rel=\"stylesheet\">";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/modernizr-2.6.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-1.10.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-3.1.1.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/bootstrap.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/respond.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Content/AdminLTE/js/app.min.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/UtilityMethods.js\"></script>";
                body += "<link href=\"http://www.cminingfarm.com/Content/site.css\" rel=\"stylesheet\">";
                body += "</head>";
                body += "<body class=\"register-page  pace-running pace-done\" style=\"\"><div class=\"pace  pace-inactive\"><div class=\"pace-progress\" data-progress-text=\"100%\" data-progress=\"99\" style=\"transform: translate3d(100%, 0px, 0px);\">";
                body += "<div class=\"pace-progress-inner\"></div>";
                body += "</div>";
                body += "<div class=\"pace-activity\"></div></div>";
                body += "<div class=\"register-box1 register-box\">";
                body += "<div class=\"register-logo\">";
                body += "<a href=\"https://dev.CMiningFarms.com\" target=\"_blank\"><img src=\"http://www.cminingfarm.com/Content/crypto/images/logo/logo.png\" alt=\"CMiningFarms.com\"></a>";
                body += "</div>";
                body += "<div class=\"register-box-body\" style=\"width:720px\">";
                body += "<div class=\"row\">";
                body += "<div class=\"col-md-12\">";
                body += "<form method=\"post\">";
                body += "<h2>Hi! " + user.Name + "</h2>";
                body += "<p class=\"lead\">We regret to inform you that your withdrawal request declined due to reason specified.</p>";
                body += "<h3>Package Details</h3>";
                body += "<p class=\"lead\">Package Name : " + pack.Name + "</p>";
                body += "<p class=\"lead\">Amount Invested : $" + up.Amount + "</p>";
                body += "<p class=\"lead\">Duration : " + pack.Duration + " Days</p>";
                body += "<p class=\"lead\">Payment Method : " + up.PaymentType + "</p>";
                body += "<p class=\"lead\">Payment Transaction ID : " + up.TransactionID + "</p>";
                body += "<p class=\"lead\">Transaction Email : " + up.SendersEmail + "</p>";
                body += "<p class=\"lead\">Purchased Date : " + up.RequestedDate + "</p>";
                body += "<p class=\"lead\">Processed Date : " + up.AcknowledgedDate + "</p>";
                body += "<p class=\"lead\">Reason For Decline : " + up.DeclineReason + "</p>";
                body += "</form>";
                body += "</div>";
                body += "</div>";
                body += "</div>";
                body += "<!-- /.form-box -->";
                body += "</div>";
                body += "</body></html>";


                retVal = SendGmail(subject, body, user.Email, true);
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }

        public static bool SendWithdrawalApprovedEmail(User user, Withdrawal withdraw)
        {
            bool retVal = false;
            try
            {
                string subject = "Your Withdrawal Approved By CMiningFarms.com";
                string body = "";

                body += "<!DOCTYPE html>";
                body += "<html style=\"\" class=\" js flexbox flexboxlegacy canvas canvastext webgl no-touch geolocation postmessage websqldatabase indexeddb hashchange history draganddrop websockets rgba hsla multiplebgs backgroundsize borderimage borderradius boxshadow textshadow opacity cssanimations csscolumns cssgradients cssreflections csstransforms csstransforms3d csstransitions fontface generatedcontent video audio localstorage sessionstorage webworkers applicationcache svg inlinesvg smil svgclippaths\">";
                body += "<head>";
                body += "<meta name=\"viewport\" content=\"width=device-width\">";
                body += "<title>Sign Up - CMiningFarms.com</title>";
                body += "<link href=\"http://www.cminingfarm.com/Content/bootstrap.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/font-awesome.min.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/ionicons.min.css\" rel=\"stylesheet\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/AdminLTE/css/AdminLTE.min.css\" rel=\"stylesheet\">";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/modernizr-2.6.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-1.10.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-3.1.1.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/bootstrap.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/respond.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Content/AdminLTE/js/app.min.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/UtilityMethods.js\"></script>";
                body += "<link href=\"http://www.cminingfarm.com/Content/site.css\" rel=\"stylesheet\">";
                body += "</head>";
                body += "<body class=\"register-page  pace-running pace-done\" style=\"\"><div class=\"pace  pace-inactive\"><div class=\"pace-progress\" data-progress-text=\"100%\" data-progress=\"99\" style=\"transform: translate3d(100%, 0px, 0px);\">";
                body += "<div class=\"pace-progress-inner\"></div>";
                body += "</div>";
                body += "<div class=\"pace-activity\"></div></div>";
                body += "<div class=\"register-box1 register-box\">";
                body += "<div class=\"register-logo\">";
                body += "<a href=\"https://CMiningFarms.com\" target=\"_blank\"><img src=\"http://www.cminingfarm.com/Content/crypto/images/logo/logo.png\" alt=\"CMiningFarms.com\"></a>";
                body += "</div>";
                body += "<div class=\"register-box-body\" style=\"width:720px\">";
                body += "<div class=\"row\">";
                body += "<div class=\"col-md-12\">";
                body += "<form method=\"post\">";
                body += "<h2>Hi! " + user.Name + "</h2>";
                body += "<p class=\"lead\">We are happy to inform you that your withdrawal request approved by our agents. We have transferred requested amount to following account</p>";
                body += "<h3>Withdraw Details</h3>";
                body += "<p class=\"lead\">Name : " + user.Name + "</p>";
                body += "<p class=\"lead\">Amount : $" + withdraw.Amount + "</p>";
                body += "<p class=\"lead\">Requested Date : " + withdraw.RequestedDate.Value.ToString("dd MMM yyyy") + "</p>";
                body += "<p class=\"lead\">Payment Method : " + withdraw.PaymentType + "</p>";
                body += "<p class=\"lead\">Payment Transaction ID : " + withdraw.TransactionID + "</p>";
                body += "<p class=\"lead\">Recipient Email : " + withdraw.RecipientEmail + "</p>";
                body += "<p class=\"lead\">Senders Email : " + withdraw.SendersEmail + "</p>";
                body += "<p class=\"lead\">Approved Date : " + withdraw.ApprovedDate.Value.ToString("dd MMM yyyy") + "</p>";
                body += "</form>";
                body += "</div>";
                body += "</div>";
                body += "</div>";
                body += "<!-- /.form-box -->";
                body += "</div>";
                body += "</body></html>";


                retVal = SendGmail(subject, body, user.Email, true);
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }

        public static bool SendWithdrawalDeclinedEmail(User user, Withdrawal withdraw)
        {
            bool retVal = false;
            try
            {
                string subject = "Your Withdrawal Declined By CMiningFarms.com";
                string body = "";

                body += "<!DOCTYPE html>";
                body += "<html style=\"\" class=\" js flexbox flexboxlegacy canvas canvastext webgl no-touch geolocation postmessage websqldatabase indexeddb hashchange history draganddrop websockets rgba hsla multiplebgs backgroundsize borderimage borderradius boxshadow textshadow opacity cssanimations csscolumns cssgradients cssreflections csstransforms csstransforms3d csstransitions fontface generatedcontent video audio localstorage sessionstorage webworkers applicationcache svg inlinesvg smil svgclippaths\">";
                body += "<head>";
                body += "<meta name=\"viewport\" content=\"width=device-width\">";
                body += "<title>Sign Up - CMiningFarms.com</title>";
                body += "<link href=\"http://www.cminingfarm.com/Content/bootstrap.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/font-awesome.min.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/ionicons.min.css\" rel=\"stylesheet\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/AdminLTE/css/AdminLTE.min.css\" rel=\"stylesheet\">";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/modernizr-2.6.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-1.10.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-3.1.1.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/bootstrap.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/respond.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Content/AdminLTE/js/app.min.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/UtilityMethods.js\"></script>";
                body += "<link href=\"http://www.cminingfarm.com/Content/site.css\" rel=\"stylesheet\">";
                body += "</head>";
                body += "<body class=\"register-page  pace-running pace-done\" style=\"\"><div class=\"pace  pace-inactive\"><div class=\"pace-progress\" data-progress-text=\"100%\" data-progress=\"99\" style=\"transform: translate3d(100%, 0px, 0px);\">";
                body += "<div class=\"pace-progress-inner\"></div>";
                body += "</div>";
                body += "<div class=\"pace-activity\"></div></div>";
                body += "<div class=\"register-box1 register-box\">";
                body += "<div class=\"register-logo\">";
                body += "<a href=\"https://CMiningFarms.com\" target=\"_blank\"><img src=\"http://www.cminingfarm.com/Content/crypto/images/logo/logo.png\" alt=\"CMiningFarms.com\"></a>";
                body += "</div>";
                body += "<div class=\"register-box-body\" style=\"width:720px\">";
                body += "<div class=\"row\">";
                body += "<div class=\"col-md-12\">";
                body += "<form method=\"post\">";
                body += "<h2>Hi! " + user.Name + "</h2>";
                body += "<p class=\"lead\">We are regret to inform you that your withdrawal request declined by our agents. Please find below details to find out more details</p>";
                body += "<h3>Withdraw Details</h3>";
                body += "<p class=\"lead\">Name : " + user.Name + "</p>";
                body += "<p class=\"lead\">Amount : $" + withdraw.Amount + "</p>";
                body += "<p class=\"lead\">Requested Date : " + withdraw.RequestedDate.Value.ToString("dd MMM yyyy") + "</p>";
                body += "<p class=\"lead\">Payment Method : " + withdraw.PaymentType + "</p>";
                body += "<p class=\"lead\">Recipient Email : " + withdraw.RecipientEmail + "</p>";
                body += "<p class=\"lead\">Reason For Decline : " + withdraw.ReasonForDecline + "</p>";
                body += "<p class=\"lead\">Processed Date : " + withdraw.ApprovedDate.Value.ToString("dd MMM yyyy") + "</p>";
                body += "</form>";
                body += "</div>";
                body += "</div>";
                body += "</div>";
                body += "<!-- /.form-box -->";
                body += "</div>";
                body += "</body></html>";


                retVal = SendGmail(subject, body, user.Email, true);
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }

        public static bool SendContactUsEmail(IndexVM vm)
        {
            bool retVal = false;
            try
            {
                string subject = "Someone Want's Contact Us";
                string body = "";

                body += "<!DOCTYPE html>";
                body += "<html style=\"\" class=\" js flexbox flexboxlegacy canvas canvastext webgl no-touch geolocation postmessage websqldatabase indexeddb hashchange history draganddrop websockets rgba hsla multiplebgs backgroundsize borderimage borderradius boxshadow textshadow opacity cssanimations csscolumns cssgradients cssreflections csstransforms csstransforms3d csstransitions fontface generatedcontent video audio localstorage sessionstorage webworkers applicationcache svg inlinesvg smil svgclippaths\">";
                body += "<head>";
                body += "<meta name=\"viewport\" content=\"width=device-width\">";
                body += "<title>Sign Up - CMiningFarms.com</title>";
                body += "<link href=\"http://www.cminingfarm.com/Content/bootstrap.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/font-awesome.min.css\" rel=\"stylesheet\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/css/ionicons.min.css\" rel=\"stylesheet\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css\">";
                body += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css\">";
                body += "<link href=\"http://www.cminingfarm.com/Content/AdminLTE/css/AdminLTE.min.css\" rel=\"stylesheet\">";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/modernizr-2.6.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-1.10.2.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/jquery-3.1.1.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/bootstrap.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/respond.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Content/AdminLTE/js/app.min.js\"></script>";
                body += "<script src=\"http://www.cminingfarm.com/Scripts/UtilityMethods.js\"></script>";
                body += "<link href=\"http://www.cminingfarm.com/Content/site.css\" rel=\"stylesheet\">";
                body += "</head>";
                body += "<body class=\"register-page  pace-running pace-done\" style=\"\"><div class=\"pace  pace-inactive\"><div class=\"pace-progress\" data-progress-text=\"100%\" data-progress=\"99\" style=\"transform: translate3d(100%, 0px, 0px);\">";
                body += "<div class=\"pace-progress-inner\"></div>";
                body += "</div>";
                body += "<div class=\"pace-activity\"></div></div>";
                body += "<div class=\"register-box1 register-box\">";
                body += "<div class=\"register-logo\">";
                body += "<a href=\"https://CMiningFarms.com\" target=\"_blank\"><img src=\"http://www.cminingfarm.com/Content/crypto/images/logo/logo.png\" alt=\"CMiningFarms.com\"></a>";
                body += "</div>";
                body += "<div class=\"register-box-body\" style=\"width:720px\">";
                body += "<div class=\"row\">";
                body += "<div class=\"col-md-12\">";
                body += "<form method=\"post\">";
                body += "<h2>Hi! Admin</h2>";
                body += "<p class=\"lead\">Please find the details below</p>";
                body += "<h3>Withdraw Details</h3>";
                body += "<p class=\"lead\">Name : " + vm.Name + "</p>";
                body += "<p class=\"lead\">Email : " + vm.Email + "</p>";
                body += "<p class=\"lead\">Phone : " + vm.Phone + "</p>";
                body += "<p class=\"lead\">Subject : " + vm.Subject + "</p>";
                body += "<p class=\"lead\">Message : " + vm.Message + "</p>";
                body += "</form>";
                body += "</div>";
                body += "</div>";
                body += "</div>";
                body += "<!-- /.form-box -->";
                body += "</div>";
                body += "</body></html>";


                retVal = SendGmail(subject, body, "contactus@CMiningFarms.com", true);
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }
        public static bool SendInvitationEmail(User user, List<string> addressList)
        {
            bool retVal = false;
            try
            {
                string subject = user.Name + "' invited you to join CMiningFarms.com";
                string sharingUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/User/SignUp?referral=" + GlobalAsset.Models.UtilityFunctions.Encryptdata(user.UserName);
                string body = "";

                body += "<!DOCTYPE html>";
                body += "<html>";
                body += "<head>";
                body += "<meta name=\"viewport\" content=\"width=device-width\" />";
                body += "<title>TestPage</title>";
                body += "<style>";
                body += "div{font-family:'Source Sans Pro','Helvetica Neue',Helvetica,Arial,sans-serif;}";
                body += ".button {background-color: #4CAF50; /* Green */";
                body += "border: none;";
                body += "color: white;";
                body += "padding: 6px 32px;";
                body += "text-align: center;";
                body += "text-decoration: none;";
                body += "display: block;";
                body += "-webkit-transition-duration: 0.4s; /* Safari */";
                body += "transition-duration: 0.4s;";
                body += "cursor: pointer;}";
                body += ".button2 {background-color: #f39c12; ";
                body += "border: 1px solid #e08e0b;}";
                body += ".button2:hover {background-color: #e08e0b;";
                body += "color: white;}";
                body += "</style>";
                body += "</head>";
                body += "<body>";
                body += "<div style=\"text-align: justify;\">";
                body += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color: white; color: black; font-family: arial, sans-serif; font-size: 16px; width: 653px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td align=\"center\" class=\"m_3828373467436816841header\" style=\"margin: 0px;\" valign=\"top\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 653px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td align=\"left\" style=\"margin: 0px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; width: 600px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"margin: 0px; min-width: 100%; width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-bottom: 3px; width: 600px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"min-width: 100%; width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "</div>";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr><td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-top: 3px; width: 600px;\" valign=\"top\"></td></tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "<tr>";
                body += "<td align=\"center\" class=\"m_3828373467436816841header\" style=\"margin: 0px;\" valign=\"top\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 653px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td align=\"left\" style=\"margin: 0px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; width: 600px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"background-color: #434343; margin: 0px; min-width: 100%; width: 640px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-bottom: 3px; width: 600px;\" valign=\"top\">";
                body += "<div class=\"separator\" style=\"clear: both; text-align: center;\">";
                body += "<br />";
                body += "</div>";
                body += "<a href=\"https://CMiningFarms.com\" style=\"font-family: &quot;Times New Roman&quot;; text-align: center;\"><img alt=\"SSS\" border=\"0\" height=\"132\" src=\"http://www.cminingfarm.com/Content/images/email_header.jpg\" width=\"640\" /></a>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr><td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-top: 3px; width: 600px;\" valign=\"top\"></td></tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"background-color: #e0e0e0; margin: 0px; min-width: 100%; width: 640px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr><td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-right: 3px; width: 297px;\" valign=\"top\"></td><td class = \"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-left: 3px; width: 297px;\" valign=\"top\"></td></tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"background-color: #e0e0e0; margin: 0px; min-width: 100%; width: 640px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 10px;\">";
                body += "<div style=\"text-align: justify;\">";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "Hi,";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<br />";
                body += "</div>";
                body += "<div style=\"text-align: justify; font-size:0.9em\">";
                body += "Your friend <b>"+user.Name+ "</b> would like to invite you to join <b>CMiningFarms.com</b>. Sign up using following link and try out our easy investment schemes.";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<br />";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<a href=\""+ sharingUrl + "\" target=\"_blank\" class=\"button button2\">Click here to sign up</a>";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<br />";
                body += "</div>";
                body += "<div style=\"text-align: justify; font-size:0.9em\">";
                body += "Bit Mining Investment Limited is an international extension of a successful business and our head office is located in United Kingdom. We aim giving a large amount of people from different countries of the world to use our services. We are diversified company and providing solution to many sector such as sale mining hardware, rent our mining capacities, runs local BitCoin exchange office and trading on crypto and forex market. Recently we launched this system for our customers to invest in crypto, because most of people are seeking about investing their money on crypto. We are offering trusted income who investing on our system.";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<br />";
                body += "</div>";
                body += "<div style=\"text-align: justify; font-size:0.9em\">";
                body += "Bitcoins are created as a reward for a process known as mining. They can be exchanged for other currencies, products, and services. As of February 2015, over 100,000 merchants and vendors accepted bitcoin as payment. Research produced by the University of Cambridge estimates that in 2017, there are 2.9 to 5.8 million unique users using a cryptocurrency wallet, most of them using bitcoin.";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<br />";
                body += "</div>";
                body += "<div style=\"text-align: justify; font-size:0.9em\">";
                body += "Thank you for taking your valuable time to consider this and hope to hear from you soon.";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<br />";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "Cheers,";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "CMiningFarms Team";
                body += "</div>";
                body += "</div>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 580px;\"></table>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 580px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 580px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"font-size: 16px; margin: 0px; padding-right: 0px; width: 290px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"min-width: 100%; width: 290px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841col\" style=\"min-width: 100%; width: 290px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 290px;\"><tbody></tbody></table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"font-size: 16px; margin: 0px; padding-left: 0px; width: 290px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"min-width: 100%; width: 290px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841col\" style=\"min-width: 100%; width: 290px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 290px;\"></table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"background-color: #434343; margin: 0px; min-width: 100%; width: 640px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 15px 10px 10px 15px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 575px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 575px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-bottom: 3px; width: 575px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"margin: 0px; min-width: 100%; width: 575px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "<table bgcolor=\"#434343\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 575px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table align=\"right\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841drop-off\" style=\"width: 201px;\"><tbody></tbody></table>";
                body += "</td>";
                body += "</tr>";
                body += "<tr><td style=\"margin: 0px;\"><a href=\"https://CMiningFarms.com\" imageanchor=\"1\" style=\"text-align: center;\"><img border=\"0\" data-original-height=\"49\" data-original-width=\"225\" height=\"42\" src=\"http://www.cminingfarm.com/Content/crypto/images/logo/logo.png\" width=\"200\" /></a></td></tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"margin: 0px; min-width: 100%; width: 640px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 10px 15px 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 570px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 570px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-right: 0px; width: 285px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"min-width: 100%; width: 285px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841col\" style=\"min-width: 100%; width: 285px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td align=\"left\" bgcolor=\"#FFFFFF\" class=\"m_3828373467436816841social-ico\" style=\"margin: 0px;\" valign=\"top\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841tableBlock\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td align=\"center\" style=\"margin: 0px;\">";
                body += "<div class=\"separator\" style=\"clear: both; text-align: center;\">";
                //body += "<a href=\"https://www.facebook.com/CRYPT-Investments-TECH-622807294754921/\" target=\"_blank\"><img border=\"0\" data-original-height=\"250\" data-original-width=\"250\" height=\"40\" src=\"https://image.ibb.co/bAHkN7/unnamed_1.png\" width=\"40\" /></a>";
                //body += "&nbsp;<a href=\"https://twitter_link/\" target=\"_blank\"><img border=\"0\" data-original-height=\"250\" data-original-width=\"250\" height=\"40\" src=\"https://image.ibb.co/bLKbUn/unnamed_3.png\" width=\"40\" /></a>&nbsp;<a href=\"https://instergram_link/\" target=\"_blank\"><img border=\"0\" data-original-height=\"250\" data-original-width=\"250\" height=\"40\" src=\"https://image.ibb.co/kfKbUn/unnamed_2.png\" style=\"font-size: 16px; text-align: justify;\" width=\"40\" /></a>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                //body += "&nbsp;<a href=\"https://youtube_link/\" target=\"_blank\"><img border=\"0\" data-original-height=\"250\" data-original-width=\"250\" height=\"40\" src=\"https://image.ibb.co/kUvGUn/unnamed.png\" style=\"text-align: center;\" width=\"40\" /></a>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-left: 0px; width: 285px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"min-width: 100%; width: 285px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841inner\" style=\"width: 285px;\">";
                body += "<tbody>";
                body += "<tr><td height=\"5\" style=\"line-height: 5px; margin: 0px;\"></td></tr>";
                body += "<tr><td align=\"center\" style=\"margin: 0px;\"><br /></td></tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</body>";
                body += "</html>";

                foreach(var address in addressList)
                {
                    retVal = SendGmail(subject, body, address, true);
                }
                
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }

        public static bool SendPasswordResetEmail(User user, int prrid)
        {
            bool retVal = false;
            try
            {
                string subject = "Password Reset Request";
                //string sharingUrl = "https://" + HttpContext.Current.Request.Url.Authority + "/User/SignUp?referral=" + GlobalAsset.Models.UtilityFunctions.Encryptdata(user.UserName);

                string resetUrl = "https://" + HttpContext.Current.Request.Url.Authority + "/User/ResetPasswordViaEmail?paramem=" + GlobalAsset.Models.UtilityFunctions.Encryptdata(user.Email)+ "&paramprr="+ GlobalAsset.Models.UtilityFunctions.Encryptdata(prrid.ToString());

                string cancelUrl = "https://" + HttpContext.Current.Request.Url.Authority + "/User/CancelPasswordResetRequest?paramem=" + GlobalAsset.Models.UtilityFunctions.Encryptdata(user.Email) + "&paramprr=" + GlobalAsset.Models.UtilityFunctions.Encryptdata(prrid.ToString());

                string body = "";

                body += "<!DOCTYPE html>";
                body += "<html>";
                body += "<head>";
                body += "<meta name=\"viewport\" content=\"width=device-width\" />";
                body += "<title>TestPage</title>";
                body += "<style>";
                body += "div{font-family:'Source Sans Pro','Helvetica Neue',Helvetica,Arial,sans-serif;}";
                body += ".button {background-color: #4CAF50; /* Green */";
                body += "border: none;";
                body += "color: white;";
                body += "padding: 6px 32px;";
                body += "text-align: center;";
                body += "text-decoration: none;";
                body += "display: block;";
                body += "-webkit-transition-duration: 0.4s; /* Safari */";
                body += "transition-duration: 0.4s;";
                body += "cursor: pointer;}";
                body += ".button2 {background-color: #f39c12; ";
                body += "border: 1px solid #e08e0b;}";
                body += ".button2:hover {background-color: #e08e0b;";
                body += "color: white;}";
                body += ".button3 {background-color: #b30000; ";
                body += "border: 1px solid #cc0000;}";
                body += ".button3:hover {background-color: #ff0000;";
                body += "color: white;}";
                body += "</style>";
                body += "</head>";
                body += "<body>";
                body += "<div style=\"text-align: justify;\">";
                body += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color: white; color: black; font-family: arial, sans-serif; font-size: 16px; width: 653px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td align=\"center\" class=\"m_3828373467436816841header\" style=\"margin: 0px;\" valign=\"top\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 653px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td align=\"left\" style=\"margin: 0px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; width: 600px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"margin: 0px; min-width: 100%; width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-bottom: 3px; width: 600px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"min-width: 100%; width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "</div>";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr><td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-top: 3px; width: 600px;\" valign=\"top\"></td></tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "<tr>";
                body += "<td align=\"center\" class=\"m_3828373467436816841header\" style=\"margin: 0px;\" valign=\"top\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 653px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td align=\"left\" style=\"margin: 0px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; width: 600px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"background-color: #434343; margin: 0px; min-width: 100%; width: 640px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-bottom: 3px; width: 600px;\" valign=\"top\">";
                body += "<div class=\"separator\" style=\"clear: both; text-align: center;\">";
                body += "<br />";
                body += "</div>";
                body += "<a href=\"https://CMiningFarms.com\" style=\"font-family: &quot;Times New Roman&quot;; text-align: center;\"><img alt=\"SSS\" border=\"0\" height=\"132\" src=\"https://preview.ibb.co/iuNUVS/SSS.jpg\" width=\"640\" /></a>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr><td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-top: 3px; width: 600px;\" valign=\"top\"></td></tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"background-color: #e0e0e0; margin: 0px; min-width: 100%; width: 640px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 600px;\">";
                body += "<tbody>";
                body += "<tr><td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-right: 3px; width: 297px;\" valign=\"top\"></td><td class = \"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-left: 3px; width: 297px;\" valign=\"top\"></td></tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"background-color: #e0e0e0; margin: 0px; min-width: 100%; width: 640px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 10px;\">";
                body += "<div style=\"text-align: justify;\">";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "Hi, "+user.Name;
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<br />";
                body += "</div>";
                body += "<div style=\"text-align: justify; font-size:0.9em\">";
                body += "Password reset request has been invoked on your account in <b>CRYPTINVESTMENTTECH.com</b>. Please click the following url and it will direct you to password reset form";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<br />";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<a href=\"" + resetUrl + "\" target=\"_blank\" class=\"button button2\">Click here to reset password</a>";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<br />";
                body += "</div>";
                body += "<div style=\"text-align: justify; font-size:0.9em\">";
                body += "If you haven't request this action please click the following link to cancel it.";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<br />";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<a href=\"" + cancelUrl + "\" target=\"_blank\" class=\"button button3\">Click here to cancel the request</a>";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "<br />";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "Cheers,";
                body += "</div>";
                body += "<div style=\"text-align: left; font-size:0.9em\">";
                body += "CRYPTINVESTMENTTECH Team";
                body += "</div>";
                body += "</div>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 580px;\"></table>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 580px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 580px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"font-size: 16px; margin: 0px; padding-right: 0px; width: 290px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"min-width: 100%; width: 290px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841col\" style=\"min-width: 100%; width: 290px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 290px;\"><tbody></tbody></table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"font-size: 16px; margin: 0px; padding-left: 0px; width: 290px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"min-width: 100%; width: 290px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841col\" style=\"min-width: 100%; width: 290px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 290px;\"></table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"background-color: #434343; margin: 0px; min-width: 100%; width: 640px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 15px 10px 10px 15px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 575px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 575px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-bottom: 3px; width: 575px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"margin: 0px; min-width: 100%; width: 575px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "<table bgcolor=\"#434343\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 575px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table align=\"right\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841drop-off\" style=\"width: 201px;\"><tbody></tbody></table>";
                body += "</td>";
                body += "</tr>";
                body += "<tr><td style=\"margin: 0px;\"><a href=\"https://image.ibb.co/ff02H7/29852722_961198410714347_241832788_n.jpg\" imageanchor=\"1\" style=\"text-align: center;\"><img border=\"0\" data-original-height=\"49\" data-original-width=\"225\" height=\"42\" src=\"https://image.ibb.co/ff02H7/29852722_961198410714347_241832788_n.jpg\" width=\"200\" /></a></td></tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"margin: 0px; min-width: 100%; width: 640px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px; padding: 10px 15px 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 570px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td style=\"margin: 0px;\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 570px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-right: 0px; width: 285px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"min-width: 100%; width: 285px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841col\" style=\"min-width: 100%; width: 285px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td align=\"left\" bgcolor=\"#FFFFFF\" class=\"m_3828373467436816841social-ico\" style=\"margin: 0px;\" valign=\"top\">";
                body += "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841tableBlock\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td align=\"center\" style=\"margin: 0px;\">";
                body += "<div class=\"separator\" style=\"clear: both; text-align: center;\">";
                body += "<a href=\"https://www.facebook.com/CRYPT-Investments-TECH-622807294754921/\" target=\"_blank\"><img border=\"0\" data-original-height=\"250\" data-original-width=\"250\" height=\"40\" src=\"https://image.ibb.co/bAHkN7/unnamed_1.png\" width=\"40\" /></a>";
                //body += "&nbsp;<a href=\"https://twitter_link/\" target=\"_blank\"><img border=\"0\" data-original-height=\"250\" data-original-width=\"250\" height=\"40\" src=\"https://image.ibb.co/bLKbUn/unnamed_3.png\" width=\"40\" /></a>&nbsp;<a href=\"https://instergram_link/\" target=\"_blank\"><img border=\"0\" data-original-height=\"250\" data-original-width=\"250\" height=\"40\" src=\"https://image.ibb.co/kfKbUn/unnamed_2.png\" style=\"font-size: 16px; text-align: justify;\" width=\"40\" /></a>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                //body += "&nbsp;<a href=\"https://youtube_link/\" target=\"_blank\"><img border=\"0\" data-original-height=\"250\" data-original-width=\"250\" height=\"40\" src=\"https://image.ibb.co/kUvGUn/unnamed.png\" style=\"text-align: center;\" width=\"40\" /></a>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "<td class=\"m_3828373467436816841responsive-td\" style=\"margin: 0px; padding-left: 0px; width: 285px;\" valign=\"top\">";
                body += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841stylingblock-content-wrapper\" style=\"min-width: 100%; width: 285px;\">";
                body += "<tbody>";
                body += "<tr>";
                body += "<td class=\"m_3828373467436816841stylingblock-content-wrapper m_3828373467436816841camarker-inner\" style=\"margin: 0px;\">";
                body += "<div class=\"m_3828373467436816841stylingblock-content-wrapper\">";
                body += "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"m_3828373467436816841inner\" style=\"width: 285px;\">";
                body += "<tbody>";
                body += "<tr><td height=\"5\" style=\"line-height: 5px; margin: 0px;\"></td></tr>";
                body += "<tr><td align=\"center\" style=\"margin: 0px;\"><br /></td></tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</table>";
                body += "</div>";
                body += "</body>";
                body += "</html>";

                retVal = SendGmail(subject, body, user.Email, true);
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }
    }
}