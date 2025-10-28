using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class NhanVien
{
    public int IdnhanVien { get; set; }

    public string HoTenNv { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public string? ChucVu { get; set; }

    public DateOnly? NgayVaoLam { get; set; }

    public decimal? Luong { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<PhieuNhapKho> PhieuNhapKhos { get; set; } = new List<PhieuNhapKho>();

    public virtual TaiKhoan? TaiKhoan { get; set; }
}
