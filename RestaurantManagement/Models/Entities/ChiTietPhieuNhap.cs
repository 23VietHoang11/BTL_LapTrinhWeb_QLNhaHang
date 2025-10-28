using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class ChiTietPhieuNhap
{
    public int IdphieuNhapKho { get; set; }

    public int IdnguyenLieu { get; set; }

    public double SoLuong { get; set; }

    public decimal DonGiaNhap { get; set; }

    public virtual NguyenLieu IdnguyenLieuNavigation { get; set; } = null!;

    public virtual PhieuNhapKho IdphieuNhapKhoNavigation { get; set; } = null!;
}
