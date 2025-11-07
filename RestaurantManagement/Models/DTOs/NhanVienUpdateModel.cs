namespace RestaurantManagement.Models.DTOs
{
    public class NhanVienUpdateModel
    {
        public string? MatKhau { get; set; } // Sẽ là NULL nếu không muốn đổi

        // Thông tin NhanVien
        public string HoTenNv { get; set; }
        public string Sdt { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public string? ChucVu { get; set; }
        public DateOnly? NgayVaoLam { get; set; }
        public decimal? Luong { get; set; }
        public DateOnly? NgaySinh { get; set; }
        public string? Cccd { get; set; }
        public string GioiTinh { get; set; }
    }
}
