using System;
using System.ComponentModel.DataAnnotations;
namespace RestaurantManagement.Areas.Admin.Models
{
    public class DangKiNV
    {
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email Đăng nhập")]
        [RegularExpression(@"^[\w\.\-]+@([\w\-]+\.)+[\w\-]{2,4}$", ErrorMessage = "Định dạng email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} phải dài ít nhất {2} ký tự.", MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "Mật khẩu phải chứa ít nhất 6 ký tự, bao gồm chữ và số")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        // --- Phần Thông tin Nhân viên (Mở rộng) ---

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [Display(Name = "Họ và Tên")]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Họ và tên chỉ được chứa chữ cái và khoảng trắng")]
        public string FullName { get; set; } // Ví dụ: Đỗ Hoàng Hiếu

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^(0|\+84)(3|5|7|8|9)[0-9]{8}$", ErrorMessage = "Số điện thoại Việt Nam không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày Sinh")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        [Display(Name = "Giới Tính")]
        [RegularExpression(@"^(Nam|Nữ|Khác)$", ErrorMessage = "Giới tính chỉ có thể là 'Nam', 'Nữ' hoặc 'Khác'")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số CCCD/CMND")]
        [Display(Name = "Số CCCD/CMND")]
        [StringLength(12, MinimumLength = 9, ErrorMessage = "Số CCCD/CMND phải từ 9 đến 12 số")]
        [RegularExpression(@"^\d{9,12}$", ErrorMessage = "Số CCCD/CMND chỉ được chứa số, độ dài 9–12")]
        public string NationalId { get; set; }

        [Display(Name = "Địa chỉ")]
        [RegularExpression(@"^[\w\s,./\-À-ỹ]+$", ErrorMessage = "Địa chỉ không được chứa ký tự đặc biệt")]
        public string Address { get; set; }
    }
}
