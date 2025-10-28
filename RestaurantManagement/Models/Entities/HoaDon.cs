using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class HoaDon
{
    public int IdhoaDon { get; set; }

    public int? IdkhachHang { get; set; }

    public int? IdnhanVien { get; set; }

    public int? IdbanAn { get; set; }

    public int? IddatBan { get; set; }

    public DateTime NgayLap { get; set; }

    public decimal? TongTien { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? ThoiGianGiao { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual BanAn? IdbanAnNavigation { get; set; }

    public virtual DatBan? IddatBanNavigation { get; set; }

    public virtual KhachHang? IdkhachHangNavigation { get; set; }

    public virtual NhanVien? IdnhanVienNavigation { get; set; }

    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
}
