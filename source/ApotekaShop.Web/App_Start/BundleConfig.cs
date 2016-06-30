using System.Web.Optimization;

namespace ApotekaShop.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
               "~/Scripts/angular.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/autocomplete").Include(
                "~/Scripts/autocomplete.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Scripts/CommonModule.js"));

            bundles.Add(new ScriptBundle("~/bundles/order").Include(
                "~/Scripts/OrderModule.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/core.css",
                "~/Content/themes/base/autocomplete.css",
                "~/Content/themes/base/base.css",
                "~/Content/themes/base/theme.css"));
        }
    }
}