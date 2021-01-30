using System.Web;
using System.Web.Optimization;

namespace GlobalAsset
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/Style").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/Style/Fonts").Include(
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/css/ionicons.min.css"));

            //bundles.Add(new StyleBundle("~/Content/HomePageStyles").Include(
            //          "~/Content/css/animate.min.css",
            //          "~/Content/css/owl.carousel.css",
            //          "~/Content/css/owl.transitions.css",
            //          "~/Content/css/prettyPhoto.css",
            //          "~/Content/css/main.css",
            //          "~/Content/css/responsive.css"));

            bundles.Add(new StyleBundle("~/AdminLTE/css").Include(
                      "~/Content/AdminLTE/css/AdminLTE.min.css",
                      "~/Content/AdminLTE/plugins/pace/pace.min.css",
                      "~/Content/AdminLTE/plugins/iCheck/square/blue.css",
                      "~/Content/AdminLTE/plugins/datepicker/datepicker3.css",
                      "~/Content/AdminLTE/css/skins/_all-skins.min.css",
                      "~/Content/AdminLTE/css/skins/skin-blue.min.css"));

            bundles.Add(new StyleBundle("~/Content/SiteCSS").Include(
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/PrintCSS").Include(
                      "~/Content/PrintStyle.css"));

            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include(
                      "~/Content/AdminLTE/plugins/fastclick/fastclick.min.js",
                      "~/Content/AdminLTE/plugins/slimScroll/jquery.slimscroll.min.js",
                      "~/Content/AdminLTE/plugins/pace/pace.min.js",
                      "~/Content/AdminLTE/plugins/iCheck/icheck.min.js",
                      "~/Content/AdminLTE/plugins/datepicker/bootstrap-datepicker.js",
                      "~/Content/AdminLTE/js/app.min.js",
                      "~/Scripts/UtilityMethods.js"));

            //bundles.Add(new ScriptBundle("~/bundles/HomePageScripts").Include(
            //          "~/Scripts/owl.carousel.min.js",
            //          "~/Scripts/mousescroll.js",
            //          "~/Scripts/smoothscroll.js",
            //          "~/Scripts/jquery.prettyPhoto.js",
            //          "~/Scripts/jquery.isotope.min.js",
            //          "~/Scripts/jquery.inview.min.js",
            //          "~/Scripts/wow.min.js",
            //          "~/Scripts/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular")
                .Include("~/Scripts/angular.min.js")
                .Include("~/Scripts/angular-route.min.js")
                .Include("~/Scripts/angular-animate.js")
                .Include("~/Content/AdminLTE/plugins/pagination/simplePagination.js")
                .IncludeDirectory("~/Scripts/angular/Controllers", "*.js")
                .IncludeDirectory("~/Scripts/angular/Factories", "*.js")
                .Include("~/Scripts/angular/app.js"));
        }
    }
}
