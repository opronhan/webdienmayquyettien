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
    
    public partial class CashBill
    {
        public CashBill()
        {
            this.CashBillDetails = new HashSet<CashBillDetail>();
        }

        public int ID { get; set; }
        public string BillCode { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Tên khách hàng không được nhiều hơn 500 ký tự hoặc ít hơn 1 ký tự")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải là 10 số")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Địa chỉ không được nhiều hơn 500 ký tự hoặc ít hơn 10 ký tự")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng ngày tháng")]
        public System.DateTime Date { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập người giao hàng")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Người giao hàng không được nhiều hơn 500 ký tự hoặc ít hơn 1 ký tự")]
        public string Shipper { get; set; }
        public string Note { get; set; }
        public int GrandTotal { get; set; }
    
        public virtual ICollection<CashBillDetail> CashBillDetails { get; set; }
    }
}
