using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class ChiTietHoaDon
{
    public int IdhoaDon { get; set; }

    public int IdmonAn { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public string? GhiChu { get; set; }

    public virtual HoaDon IdhoaDonNavigation { get; set; } = null!;

    public virtual MonAn IdmonAnNavigation { get; set; } = null!;
}
