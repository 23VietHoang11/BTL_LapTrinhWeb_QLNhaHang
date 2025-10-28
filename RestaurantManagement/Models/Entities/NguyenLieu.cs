using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class NguyenLieu
{
    public int IdnguyenLieu { get; set; }

    public string TenNl { get; set; } = null!;

    public string? DonVi { get; set; }

    public string? Loai { get; set; }

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual ICollection<MonAnNguyenLieu> MonAnNguyenLieus { get; set; } = new List<MonAnNguyenLieu>();

    public virtual ICollection<TonKho> TonKhos { get; set; } = new List<TonKho>();

    public virtual ICollection<NhaCungCap> IdnhaCungCaps { get; set; } = new List<NhaCungCap>();
}
