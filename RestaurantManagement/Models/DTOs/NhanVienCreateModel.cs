namespace RestaurantManagement.Models.DTOs
{
    public class NhanVienCreateModel
    {
        // Thông tin cho bảng TaiKhoan
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; } // Mật khẩu gốc từ form

        // Thông tin cho bảng NhanVien
        public string HoTenNv { get; set; }
        public string Sdt { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public string? ChucVu { get; set; }
        public DateOnly? NgayVaoLam { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public decimal? Luong { get; set; }
        public DateOnly? NgaySinh { get; set; }
        public string? Cccd { get; set; }
        // Giới tính (từ form HTML)
        public string GioiTinh { get; set; }
    }
}
