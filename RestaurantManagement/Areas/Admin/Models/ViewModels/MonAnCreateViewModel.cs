// Trong file: Models/ViewModels/MonAnCreateViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    public class MonAnCreateViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên mặt hàng")]
        public string TenMon { get; set; }

        public string? Loai { get; set; } // "Đồ uống", "Món chính"...

        public string? DonViTinh { get; set; } // "Ly", "Đĩa"...

        public string? GhiChu { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá bán")]
        public decimal Gia { get; set; }


        // Dùng IFormFile để nhận file ảnh từ form
        public IFormFile? ImageFile { get; set; }
        public decimal GiaBan { get; internal set; }
    }
}