﻿using System.Web;
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
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/Customer/plugins.js",
                      "~/Scripts/Customer/designesia.js",
                      "~/Scripts/Customer/swiper.js",
                      "~/Scripts/Customer/custom-marquee.js",
                      "~/Scripts/Customer/custom-swiper-1.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/Customer/css/plugins.css",
                      "~/Content/Customer/css/colors/scheme-01.css",
                      "~/Content/Customer/css/swiper.css",
                      "~/Content/Customer/css/style.css",
                      "~/Content/Customer/css/coloring.css"
                      ));

            bundles.Add(new Bundle("~/bundles/fullcalendar").Include(
                      "~/Scripts/index.global.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/calendar.js"
                      ));
        }
    }
}
