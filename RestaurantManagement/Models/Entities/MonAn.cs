using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class MonAn
{
    public int IdmonAn { get; set; }

    public string TenMon { get; set; } = null!;

    public decimal Gia { get; set; }

    public string? DonViTinh { get; set; }

    public string? Loai { get; set; }

    // === THÊM DÒNG NÀY VÀO ===
    public string? HinhAnh { get; set; }
    // ===========================

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual ICollection<MonAnNguyenLieu> MonAnNguyenLieus { get; set; } = new List<MonAnNguyenLieu>();
}