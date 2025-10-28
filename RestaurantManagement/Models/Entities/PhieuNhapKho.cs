using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class PhieuNhapKho
{
    public int IdphieuNhapKho { get; set; }

    public int? IdnhanVien { get; set; }

    public int? IdnhaCungCap { get; set; }

    public DateTime NgayNhap { get; set; }

    public decimal? TongTien { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual NhaCungCap? IdnhaCungCapNavigation { get; set; }

    public virtual NhanVien? IdnhanVienNavigation { get; set; }
}
