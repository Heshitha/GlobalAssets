using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GlobalAsset.Models;

namespace GlobalAsset.Controllers.API
{
    public class UserAPIController : ApiController
    {
        GlobalAssetsDBDataContext db = new GlobalAssetsDBDataContext();

        [HttpPost]
        public UserViewModel GetUserDetails([FromBody]int userID)
        {
            UserViewModel retVal = new UserViewModel();
            try
            {
                var result = db.Users.Where(x=> x.UserID == userID).FirstOrDefault();
                if (result != null)
                {
                    retVal = new UserViewModel()
                    {
                        UserID = result.UserID,
                        Title = result.Title,
                        Name = result.Name,
                        Address = result.Address,
                        Country = result.Country,
                        Email = result.Email,
                        Mobile = result.Mobile,
                        Telephone = result.Telephone,
                        UserName = result.UserName,
                        NetellerEmail = result.NetellerAccountEmail,
                        SkrillEmail = result.SkrillAccountEmail,
                        BitCoinAddress = result.BitCoinWalletAddress,
                        ProfileImage = result.ProfileImage
                    };
                }
            }
            catch (Exception ex)
            {
                //LogClass.WriteErrorLog(ex);
            }
            return retVal;
        }

        [HttpPost]
        public bool ChangePassword([FromBody]UserViewModel user)
        {
            bool retVal = false;
            try
            {
                Models.User usr = db.Users.Where(x => x.UserID == user.UserID).FirstOrDefault();
                if (usr != null && user.NewPassword == user.ConfirmPassword && usr.Password == UtilityFunctions.Encryptdata(user.Password))
                {
                    usr.Password = UtilityFunctions.Encryptdata(user.NewPassword);
                    db.SubmitChanges();

                    retVal = true;
                }
                else
                {
                    retVal = false;
                }
            }
            catch (Exception ex)
            {
                //LogClass.WriteErrorLog(ex);
            }
            return retVal;
        }

        [HttpPost]
        public bool ModifyAccountDetails([FromBody]UserViewModel user)
        {
            bool retVal = false;
            try
            {
                Models.User usr = db.Users.Where(x => x.UserID == user.UserID).FirstOrDefault();
                if (usr != null)
                {
                    usr.NetellerAccountEmail = user.NetellerEmail;
                    usr.SkrillAccountEmail = user.SkrillEmail;
                    usr.BitCoinWalletAddress = user.BitCoinAddress;
                    db.SubmitChanges();

                    retVal = true;
                }
                else
                {
                    retVal = false;
                }
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }

        [HttpPost]
        public bool UpdateUserDetails([FromBody]UserViewModel user)
        {
            bool retVal = false;
            try
            {
                Models.User usr = db.Users.Where(x => x.UserID == user.UserID).FirstOrDefault();
                if (usr != null)
                {
                    usr.Title = user.Title;
                    usr.Address = user.Address;
                    usr.Country = user.Country;
                    usr.Mobile = user.Mobile;
                    usr.Telephone = user.Telephone;
                    db.SubmitChanges();

                    retVal = true;
                }
                else
                {
                    retVal = false;
                }
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }
    }
}
