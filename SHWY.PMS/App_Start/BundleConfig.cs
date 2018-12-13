using System.Web;
using System.Web.Optimization;

namespace SHWY.PMS
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/json2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/EasyUiCss").Include(
"~/Content/jquery-easyui-1.6.7/themes/icon.css"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryEasyUi").Include(
                "~/Content/jquery-easyui-1.6.7/jquery.easyui.min.js",
                "~/Content/jquery-easyui-1.6.7/locale/easyui-lang-zh_CN.js"));
            bundles.Add(new ScriptBundle("~/bundles/CommonJS").Include("~/Scripts/Common.js"));

        }
    }
}
