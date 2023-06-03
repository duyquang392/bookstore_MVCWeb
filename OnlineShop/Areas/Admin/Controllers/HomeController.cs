using Model.Dao;
using OnlineShop.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            DateTime date = DateTime.Now;
            var homeModel = new HomeModel();
            homeModel.newOrder = new OrderDao().CountOrderInMonth(date.Month, date.Year);
            homeModel.newUser = new UserDao().CountCustomerInMonth(date.Month, date.Year);
            homeModel.totalSale = new OrderDao().TotalSaleInMonth(date.Month, date.Year);
            homeModel.recentOrders = new OrderDao().OrdersInMonth(date.Month, date.Year);
            return View(homeModel);
        }
    }
}