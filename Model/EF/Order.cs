namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [DisplayName("Mã đơn hàng")]
        public long ID { get; set; }
        [DisplayName("Tài khoản khách hàng")]
        [Required(ErrorMessage = "Tài khoản khách hàng là bắt buộc")]
        public long CustomerID { get; set; }

        [DisplayName("Tổng tiền")]
        public long Total { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Tên người nhận là bắt buộc")]
        [DisplayName("Tên người nhận")]
        public string ShipName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [DisplayName("Số điện thoại người nhận")]
        public string ShipMobile { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [DisplayName("Địa chỉ người nhận")]
        public string ShipAddress { get; set; }

        [StringLength(50)]
        [DisplayName("Địa chỉ Email")]
        public string ShipEmail { get; set; }
        [DisplayName("Ngày tạo")] 
        public DateTime CreatedDate { get; set; }
        [DisplayName("Người tạo")]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [DisplayName("Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        [DisplayName("Người sửa")]
        public string ModifiedBy { get; set; }
        [DisplayName("Trạng thái")]
        public bool Status { get; set; }
    }
}
