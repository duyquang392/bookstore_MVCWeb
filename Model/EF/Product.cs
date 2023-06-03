namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public long ID { get; set; }

        [StringLength(250)]
        [DisplayName("Tên sách")]
        [Required(ErrorMessage = "Bạn chưa nhập tên sách")]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [DisplayName("Tác giả")]
        public long? AuthorID { get; set; }

        [DisplayName("Nhà xuất bản")]
        public long? PublishingCompanyID { get; set; }
        
        [DisplayName("Ảnh")]
        [StringLength(250)]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string MoreImages { get; set; }

        [DisplayName("Giá")]
        [Required(ErrorMessage = "Bạn chưa nhập giá sản phẩm")]
        public decimal? Price { get; set; }

        [DisplayName("Giá đã giảm")]
        public decimal? PromotionPrice { get; set; }
        
        [DisplayName("Số lượng sách")]
        [Required(ErrorMessage = "Bạn chưa nhập số lượng sách")]
        public int NumberOfBooks { get; set; }

        public bool? IncludedVAT { get; set; }
        [DisplayName("Đã bán")]
        public int SoldOut { get; set; }

        [DisplayName("Thể loại")]
        public long? CategoryID { get; set; }
        [DisplayName("Số trang")]
        public int? NumberOfPages { get; set; }

        [Column(TypeName = "ntext")]
        [DisplayName("Mô tả")]
        public string Detail { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        [DisplayName("Người tạo")]
        public string CreatedBy { get; set; }
        [DisplayName("Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        [DisplayName("Người sửa")]
        public string ModifiedBy { get; set; }

        [StringLength(250)]
        public string MetaKeywords { get; set; }

        [StringLength(250)]
        public string MetaDescriptions { get; set; }
        
        [DisplayName("Trạng thái")]
        public bool Status { get; set; }

        public DateTime? TopHot { get; set; }
        [DisplayName("Lượt xem")]
        public int? ViewCount { get; set; }
    }
}
