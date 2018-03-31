using System.Web;
using System.Web.Optimization;

namespace ADV.InternetCrawler.ControlPanel
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.2.1.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/clipboard.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/ModularCode").Include(
                        "~/Scripts/ModularCode/vendor.js",
                        "~/Scripts/ModularCode/app.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/DataTables/dataTables.responsive.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/Custom").Include(
                        "~/Scripts/date.format.js"
                        ));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // готово к выпуску, используйте средство сборки по адресу https://modernizr.com, чтобы выбрать только необходимые тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/DataTables").Include(
                    "~/Content/DataTables/css/jquery.dataTables.css",
                    "~/Content/DataTables/css/responsive.bootstrap.css"
                         ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      //"~/Content/site.css",
                      "~/Content/ModularCode/vendor.css",
                      "~/Content/ModularCode/app-blue.css"
                      ));
        }
    }
}
