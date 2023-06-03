using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OnlineShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["SoluongOnline"] = 0;
            Application["count_visit"] = 0;
        }
        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 5;
            Application.Lock();
            Application["SoluongOnline"] = (Convert.ToInt32(Application["SoluongOnline"])) + 1;
            //Application["SoluongTruycap"] = (Convert.ToInt32(Application["SoluongTruycap"])) + 1;
            Application.UnLock();
            int count_visit = 0;
            if (System.IO.File.Exists(Server.MapPath("~/Content/count_visit.txt")) == false)
            {
                count_visit = 1;
            }
            else
            {
                System.IO.StreamReader read = new System.IO.StreamReader(Server.MapPath("~/Content/count_visit.txt"));
                count_visit = int.Parse(read.ReadLine());
                read.Close();
                count_visit++;
            }
            Application.Lock();
            Application["count_visit"] = count_visit;
            Application.UnLock();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(Server.MapPath("~/Content/count_visit.txt"));
            writer.WriteLine(count_visit);
            writer.Close();

        }
        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["SoluongOnline"] = (Convert.ToInt32(Application["SoluongOnline"])) - 1;
            Application.UnLock();

        }
    }
       
}
