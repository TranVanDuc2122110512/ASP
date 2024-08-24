namespace TranVanDuc_2122110512.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        // Thuộc tính để lưu tên tệp hình ảnh
        public string Image { get; set; }

        // Thuộc tính không phải là thuộc tính cơ sở dữ liệu, chỉ để tải lên hình ảnh
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        // Thuộc tính để lưu ID danh mục cha
        public int? Parent_Id { get; set; }

        // Điều này nếu bạn muốn chỉ định mối quan hệ với danh mục cha
        [ForeignKey("Parent_Id")]
        public virtual Category ParentCategory { get; set; }

        // Điều này nếu bạn muốn chỉ định các danh mục con
        public virtual ICollection<Category> SubCategories { get; set; }
    }
}
