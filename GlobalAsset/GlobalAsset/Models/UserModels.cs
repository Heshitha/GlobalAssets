using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAsset.Models
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ReferralName { get; set; }
        public string NetellerEmail { get; set; }
        public string SkrillEmail { get; set; }
        public string BitCoinAddress { get; set; }
        public string ProfileImage { get; set; }
    }

    public class ResetPasswordUserViewModel
    {
        public string ParamEm { get; set; }
        public string ParamPrr { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}