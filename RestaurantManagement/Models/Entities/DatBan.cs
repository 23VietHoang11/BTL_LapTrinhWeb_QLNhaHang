using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class DatBan
{
    public int IddatBan { get; set; }

    public int? IdkhachHang { get; set; }

    public int? IdbanAn { get; set; }

    public DateTime ThoiGian { get; set; }

    public int? SoLuongKh { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual BanAn? IdbanAnNavigation { get; set; }

    public virtual KhachHang? IdkhachHangNavigation { get; set; }
}
