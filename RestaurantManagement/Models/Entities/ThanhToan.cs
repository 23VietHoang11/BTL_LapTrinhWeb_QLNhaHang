using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class ThanhToan
{
    public int IdthanhToan { get; set; }

    public int IdhoaDon { get; set; }

    public int? IdkhachHang { get; set; }

    public DateTime NgayThanhToan { get; set; }

    public decimal SoTien { get; set; }

    public string? PhuongThuc { get; set; }

    public virtual HoaDon IdhoaDonNavigation { get; set; } = null!;

    public virtual KhachHang? IdkhachHangNavigation { get; set; }
}
