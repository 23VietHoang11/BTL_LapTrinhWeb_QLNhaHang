using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class TaiKhoan
{
    public int IdtaiKhoan { get; set; }

    public int IdnhanVien { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhauHash { get; set; } = null!;

    public string? TrangThai { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual NhanVien IdnhanVienNavigation { get; set; } = null!;

    public virtual ICollection<Quyen> Idquyens { get; set; } = new List<Quyen>();
}
