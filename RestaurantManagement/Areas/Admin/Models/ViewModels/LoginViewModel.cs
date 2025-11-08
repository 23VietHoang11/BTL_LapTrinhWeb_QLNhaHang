using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ tôi")]
        public bool RememberMe { get; set; }

        // Có thể thêm thuộc tính này để lưu thông báo lỗi từ server, nếu cần.
        public string ErrorMessage { get; set; }
    }
}