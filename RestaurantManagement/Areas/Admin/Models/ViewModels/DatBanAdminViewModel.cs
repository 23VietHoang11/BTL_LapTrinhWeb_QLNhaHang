namespace RestaurantManagement.Models.ViewModels
{
    public class DatBanAdminViewModel
    {
        public int IDDatBan { get; set; }
        public string? TenKhachHang { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public DateTime ThoiGian { get; set; }
        public int SoLuongKH { get; set; }
        public string? GhiChu { get; set; }
        public string? TrangThai { get; set; }
        public int? IDBanAn { get; set; }
    }
}
