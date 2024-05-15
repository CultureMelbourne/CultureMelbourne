using System.Web;
using System.Web.Optimization;

namespace TP03MainProj
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"
                      ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
          "~/Content/bootstrap.min.css",
          "~/Content/site.css",
          "~/Content/Customer/css/plugins.css",
          "~/Content/Mycss/responsive.css",
          "~/Content/Mycss/style.css"
          ));

                    bundles.Add(new StyleBundle("~/Content/edu").Include(
        "~/Content/Customer/css/style.css"
        ));

            bundles.Add(new StyleBundle("~/Content/singlecss").Include(
                "~/Content/Mycss/home.css"
                ));

            bundles.Add(new Bundle("~/bundles/fullcalendar").Include(
                      "~/Scripts/index.global.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/calendar.js"
                      ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                              "~/Content/Mycss/mycss.css",
                              "~/Content/Mycss/Education.css"));

            bundles.Add(new ScriptBundle("~/bundles/educationJS").Include(
            "~/Scripts/edu.js",
            "~/Scripts/charts.js",
            "~/Scripts/sidebar.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/scrollToTopStyle").Include(
                "~/Content/scrollToTop.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/quizstyle").Include(
                              "~/Content/Mycss/quiz.css"
                ));


            bundles.Add(new ScriptBundle("~/bundles/scrollToTopJS").Include(
                "~/Scripts/scrollToTop.js"
                ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
