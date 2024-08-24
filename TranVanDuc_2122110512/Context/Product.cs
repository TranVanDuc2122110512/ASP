namespace TranVanDuc_2122110512.Context
{
    using System;
    using System.Collections.Generic;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.CartItems = new HashSet<CartItem>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public int CategoryID { get; set; }
        public int BrandID { get; set; }
        public int SupplierID { get; set; }
        public string Image { get; set; } // Đây là thuộc tính chứa tên hình ảnh

        public int Inventory { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string Dimensions { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Material { get; set; }

        // Thêm các thuộc tính liên kết với Brand và Category
        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartItem> CartItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
