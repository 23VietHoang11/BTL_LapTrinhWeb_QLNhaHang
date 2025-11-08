// File: HoaDonCreateEditViewModel.cs

using System.ComponentModel.DataAnnotations;
using System;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    public class HoaDonCreateEditViewModel
    {
        public int IdhoaDon { get; set; }

        [Display(Name = "Khách hàng (ID)")]
        // Dùng int? để chấp nhận giá trị null cho khách vãng lai
        public int? IdkhachHang { get; set; }

        [Required]
        [Display(Name = "Trạng Thái")]
        public string TrangThai { get; set; } = "Mới"; // Mặc định là Mới

        // Các trường chỉ hiển thị, không cần required
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
    }
}