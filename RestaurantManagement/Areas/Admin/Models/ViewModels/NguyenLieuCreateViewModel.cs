using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    public class NguyenLieuCreateViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên nguyên liệu")]
        public string TenNl { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đơn vị tính")]
        public string DonVi { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập loại nguyên liệu")]
        public string Loai { get; set; }

        public int SoLuong { get; set; } = 0;

        public DateTime? NgayNhap { get; set; } = DateTime.Now;

        // Dùng để upload ảnh
        public IFormFile? HinhAnh { get; set; }
    }
}