using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    public class NhanVienEditViewModel : NhanVienBaseViewModel
    {
        public int IdnhanVien { get; set; }

        // Chỉ dùng để hiển thị, không cho phép sửa tên đăng nhập
        [Display(Name = "Tên đăng nhập")]
        public string? TenDangNhap { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự")]
        [Display(Name = "Mật khẩu mới (Bỏ trống nếu không đổi)")]
        public string? MatKhauMoi { get; set; }
    }
}   