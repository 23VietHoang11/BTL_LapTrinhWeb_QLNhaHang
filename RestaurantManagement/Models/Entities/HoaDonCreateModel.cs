namespace RestaurantManagement.Models.DTOs
{
    public class HoaDonCreateModel
    {
        // public int IdbanAn { get; set; } // <-- XÓA DÒNG NÀY

        public string SdtKhachHang { get; set; }
        public string HoTenKhachHang { get; set; }
        public int SoLuongKhach { get; set; }
        public string? GhiChu { get; set; }
        public DateTime ThoiGian { get; set; }
        public decimal TongTien { get; set; }
    }
}