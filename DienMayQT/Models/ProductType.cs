﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DienMayQT.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class ProductType
    {
        public ProductType()
        {
            this.Products = new HashSet<Product>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mã loại sản phẩm")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Mã loại sản phẩm phải là 3 ký tự")]
        public string ProductTypeCode { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên loại sản phẩm")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Tên loại sản phẩm phải ít hơn 20 ký tự và nhiều hơn 4 ký tự")]
        public string ProductTypeName { get; set; }
    
        public virtual ICollection<Product> Products { get; set; }
    }
}
