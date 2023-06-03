using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class OrdelModel
    {
        public Order ModelOrder { get; set; }
        public List<OrderDetail> ModelOrderDetail { get; set; }
        public List<Product> ModelProduct { get; set; }
    }
}