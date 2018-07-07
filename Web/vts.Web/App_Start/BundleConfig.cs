using System.Web.Optimization;

namespace vts.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));*/

            /*bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));*/

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            /*bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));*/

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Scripts/custom/vendor.min.js",
                      "~/Scripts/custom/theme.min.js",
                      "~/Scripts/custom/application.min.js",
                      "~/Scripts/custom/style.min.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/vendor.min.css",
                      "~/Content/css/theme.min.css",
                      "~/Content/css/application.min.css",
                      "~/Content/css/style.min.css",
                      "~/Content/css/main.css"));

            bundles.Add(new StyleBundle("~/login/css").Include(
                      "~/Content/css/login.css"));

            /*bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));*/
        }
    }
}