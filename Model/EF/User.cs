namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public long ID { get; set; }

        [StringLength(50)]
        [DisplayName("Tài khoản")]
        [Required(ErrorMessage = "Bạn chưa nhập tài khoản")]
        public string UserName { get; set; }

        [StringLength(32)]
        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string Password { get; set; }

        [StringLength(20)]
        [DisplayName("Phân quyền người dùng")]
        public string GroupID { get; set; }

        [StringLength(50)]
        [DisplayName("Họ tên")]
        [Required(ErrorMessage = "Bạn chưa nhập họ tên")]
        public string Name { get; set; }

        [StringLength(50)]
        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ")]
        public string Address { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại")]
        [RegularExpression(@"\+?(0|84)\d{9}", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        public int? ProvinceID { get; set; }

        public int? DistrictID { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        [DisplayName("Người tạo")]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
        [DisplayName("Trạng thái")]
        public bool Status { get; set; }
    }
}
