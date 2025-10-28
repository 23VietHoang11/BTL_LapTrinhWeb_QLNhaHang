using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class KhachHang
{
    public int IdkhachHang { get; set; }

    public string HoTenKh { get; set; } = null!;

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public string? GioiTinh { get; set; }

    public virtual ICollection<DatBan> DatBans { get; set; } = new List<DatBan>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
}
