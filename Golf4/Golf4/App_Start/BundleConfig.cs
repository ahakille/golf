using System.Web;
using System.Web.Optimization;

namespace Golf4
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
                        //"~/Scripts/jquery-2.1.1.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js",
                    "~/Scripts/bootstrap.min.js",
                    "~/Scripts/wow.min.js",
                    "~/Scripts/jquery.easing.1.3.js",
                    "~/Scripts/jquery.isotope.min.js",
                    "~/Scripts/jquery.bxslider.min.js",
                    "~/Scripts/fliplightbox.min.js",
                    "~/Scripts/functions.js",
                    "~/Scripts/fliplightbox.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.min.css",
                    "~/Content/animate.css",
                    "~/Content/font-awesome.min.css",
                    "~/Content/jquery.bxslider.css",
                    "~/Content/normalize.css",
                    "~/Content/demo.css",
                    "~/Content/set1.css",
                    "~/Content/overwrite.css",
                    "~/Content/style.css",
                    "~/Content/themes/base/datepicker.css",
                    "~/Content/themes/base/jquery-ui.css"));
        }
    }
}
