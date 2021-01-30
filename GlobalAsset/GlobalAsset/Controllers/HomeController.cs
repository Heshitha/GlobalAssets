using GlobalAsset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlobalAsset.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            Response.Redirect("https://www.cminingfarms.com/");
            ViewBag.ShowSuccess = false;
            return View();
        }

        [HttpPost]
        public ActionResult Index(IndexVM indexvm)
        {
            ViewBag.ShowSuccess = false;
            try
            {
                UtilityFunctions.SendContactUsEmail(indexvm);
                ViewBag.ShowSuccess = true;

            }
            catch (Exception)
            {
                
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}