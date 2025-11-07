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

    public DateTime? NgayVaoLam { get; set; }

    public decimal? Luong { get; set; }

    // --- BỔ SUNG CÁC TRƯỜNG CÒN THIẾU TỪ FORM ---
    public DateTime? NgaySinh { get; set; } // Thêm trường này cho Ngày sinh

    public string? Cccd { get; set; } // Thêm trường này cho Số CCCD

    public string? GioiTinh { get; set; }
    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<PhieuNhapKho> PhieuNhapKhos { get; set; } = new List<PhieuNhapKho>();

    public virtual TaiKhoan? TaiKhoan { get; set; }
}
