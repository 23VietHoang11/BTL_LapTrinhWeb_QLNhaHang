using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    public class NhanVienBaseViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [Display(Name = "Họ và tên")]
        public string HoTenNv { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string Sdt { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chức vụ")]
        [Display(Name = "Chức vụ")]
        public string ChucVu { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? NgaySinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số CCCD/CMND")]
        [Display(Name = "CCCD/CMND")]
        public string Cccd { get; set; }

        [Display(Name = "Giới tính")]
        public string? GioiTinh { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày vào làm")]
        public DateTime? NgayVaoLam { get; set; } = DateTime.Now;

        [Display(Name = "Lương cơ bản")]
        public decimal? Luong { get; set; }
    }
}