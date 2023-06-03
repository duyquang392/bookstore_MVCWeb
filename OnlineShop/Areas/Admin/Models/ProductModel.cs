using Model.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class ProductModel
    {
        public Product ModelProduct { get; set; }
        [DisplayName("Tác giả")]
        public string authorName { get; set; }
        [DisplayName("Nhà xuất bản")]
        public string publishingCompanyName { get; set; }
        [DisplayName("Thể loại")]
        public string categoryName { get; set; }

    }
}