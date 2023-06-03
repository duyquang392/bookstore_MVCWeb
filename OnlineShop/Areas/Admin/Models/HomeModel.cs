using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class HomeModel
    {
        public int newUser { get; set; }
        public int newOrder { get; set; }
        public long totalSale { get; set; }
        public IEnumerable<Order> recentOrders { get; set; }

    }
}