using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class NhaCungCap
{
    public int IdnhaCungCap { get; set; }

    public string TenNcc { get; set; } = null!;

    public string? DiaChiNcc { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<PhieuNhapKho> PhieuNhapKhos { get; set; } = new List<PhieuNhapKho>();

    public virtual ICollection<NguyenLieu> IdnguyenLieus { get; set; } = new List<NguyenLieu>();
}
