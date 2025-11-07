using System;

namespace RestaurantManagement.Models.DTOs
{
    public class NguyenLieuDTO
    {
        public string TenNl { get; set; }
        public string DonVi { get; set; }
        public string Loai { get; set; }

        // Thêm 3 trường mới
        public int? SoLuong { get; set; }
        public DateTime? NgayNhap { get; set; }
        public string? HinhAnh { get; set; }
    }
}